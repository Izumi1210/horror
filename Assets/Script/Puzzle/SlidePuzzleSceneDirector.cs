using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidePuzzleSceneDirector : MonoBehaviour
{
    // ピース
    [SerializeField] List<GameObject> pieces;
    // ゲームクリア時に表示されるボタン
    [SerializeField] GameObject buttonRetry;
    // シャッフル回数
    [SerializeField] int shuffleCount;
    [SerializeField] string clearSceneName;

    // 初期位置
    List<Vector2> startPositions;

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を保存
        startPositions = new List<Vector2>();
        foreach (var item in pieces)
        {
            startPositions.Add(item.transform.position);
        }
        // 指定回数シャッフル
        for (int i = 0; i < shuffleCount; i++)
        {
            List<GameObject> movablePieces = new List<GameObject>();

        // 0と隣接しているピースをリストに追加
        foreach (var item in pieces)
        {
            if (GetEmptyPiece(item))
            {
                movablePieces.Add(item);
            }
        }

        // 隣接するピースをランダムでいれかえる
        int rnd = Random.Range(0, movablePieces.Count);
        GameObject piece = movablePieces[rnd];
        SwapPiece(piece, pieces[0]);
        }

        // ボタン非表示
        buttonRetry.SetActive(false);
    }

    // Update is called once per frame
    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        // タッチ処理
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPoint, Vector2.zero);

            // 当たり判定があった
            if (hit2d)
            {
                // ゲームオブジェクト
                GameObject hitPiece = hit2d.collider.gameObject;
                // 0と隣接していればデータが入る
                GameObject emptyPiece = GetEmptyPiece(hitPiece);

                // 選んだピースと0番のピースを入れかえる
                SwapPiece(hitPiece, emptyPiece);
                // クリア判定
                buttonRetry.SetActive(true);

                // 正解の位置と違うピースを探す
                for (int i = 0; i < pieces.Count; i++)
                {
                    // 現在のポジション
                    Vector2 position = pieces[i].transform.position;
                    // 初期位置と違ったらボタンを非表示
                    if (position != startPositions[i])
                    {
                        buttonRetry.SetActive(false);
                    }
                }

                // クリア状態
                if (buttonRetry.activeSelf)
                {
                    Debug.Log("クリア！！");
                    SceneManager.LoadScene(clearSceneName);
                }
            }
        }
    }

    // 引数のピースが0番のピースと隣接していたら0番のピースを返す
    GameObject GetEmptyPiece(GameObject piece)
    {
        // 2点間の距離を計算
        float dist = Vector2.Distance(piece.transform.position, pieces[0].transform.position);

        // デバッグ確認用（任意）
        // Debug.Log($"{piece.name} と空白ピースの距離: {dist}");

        // 距離が 1 ± 0.1 以内なら「隣接している」とみなす
        if (Mathf.Abs(dist - 1f) < 0.1f)
        {
            return pieces[0];
        }

        return null;
    }


    // 2つのピースの位置を入れかえる
    void SwapPiece(GameObject pieceA, GameObject pieceB)
    {
        // どちらかがnullなら処理をしない
        if (pieceA == null || pieceB == null)
        {
            return;
        }

        // AとBのポジションを入れかえる
        Vector2 position = pieceA.transform.position;
        pieceA.transform.position = pieceB.transform.position;
        pieceB.transform.position = position;
    }
    // リトライボタン
    public void OnClickRetry()
    {
        SceneManager.LoadScene("SlidePuzzleScene");
    }
}