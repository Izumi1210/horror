using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateOrbs))]
[RequireComponent(typeof(PuzzleScoreController))]
[RequireComponent(typeof(PuzzleTimeController))]
public class PuzzleGameController : MonoBehaviour
{
    public static PuzzleGameController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Orb[][] board;
    public Vector2[,] prescribedOrbPos;
    public bool isPlayable = false;
    public bool isInProgress = false;
    [Header("いくつオーブが繋がったら消えるか")]
    public int eraseCount = 4;
    [Header("オーブが落ちる速度")]
    public float dropSpeed = 1f;
    [Header("オーブが何かの間違いで落ち切らなかった時に強制的に落とすまでの時間")]
    public float forceDropDelay = 3f;
    [Header("制限時間")]
    public float timeLimit = 60f;
    [Header("クリアに必要なスコア")]
    public int clearScore = 1000000;
    [SerializeField] GameObject startCanvas;

    GenerateOrbs generateOrbs;
    PuzzleScoreController puzzleScoreController;
    PuzzleTimeController puzzleTimeController;

    bool isErasing = false;

    void Start()
    {
        // コンポーネントの取得
        generateOrbs = GetComponent<GenerateOrbs>();
        puzzleScoreController = GetComponent<PuzzleScoreController>();
        puzzleTimeController = GetComponent<PuzzleTimeController>();
        if (generateOrbs == null)
            Debug.LogError("GenerateOrbsコンポーネントが見つかりません。");
        if (puzzleScoreController == null)
            Debug.LogError("PuzzleScoreControllerコンポーネントが見つかりません。");
        if (puzzleTimeController == null)
            Debug.LogError("PuzzleTimeControllerコンポーネントが見つかりません。");

        isPlayable = false;
        isInProgress = false;

        startCanvas.SetActive(true);
    }

    public void GameStart()
    {
        // ボードの初期化
        board = generateOrbs.InitializeBoard();
        //4つ以上繋がっているオーブがないことを確認し、もしあれば色を変える
        while (true)
        {
            bool hasConnectedOrbs = false;
            for (int x = 0; x < board.Length; x++)
            {
                if (hasConnectedOrbs) break;
                for (int y = 0; y < board[x].Length; y++)
                    if (board[x][y] != null)
                    {
                        List<Orb> connectedOrbs = board[x][y].GetConnectedOrb();
                        if (connectedOrbs.Count >= PuzzleGameController.instance.eraseCount)
                        {
                            hasConnectedOrbs = true;
                            //4つ以上繋がっているオーブがあったので色を変える
                            Debug.Log("4つ以上繋がっているオーブがあったので色を変えた");
                            board = generateOrbs.ChangeOrbColor(board, x, y);
                        }

                    }
            }
            if (!hasConnectedOrbs) break; //4つ以上繋がっているオーブがなければループを抜ける
            Debug.Log("4つ以上繋がっているオーブがあったので再度チェックする");
        }
        // オーブの規定の位置を取得
        prescribedOrbPos = generateOrbs.GetPrescribedOrbPosition();

        StartCoroutine(StartEvent());
    }

    public IEnumerator StartEvent()
    {
        // オーブを落とす
        yield return StartCoroutine(OrbDropOnGameStart());


        Debug.Log("ゲームスタート");
        isInProgress = true;
        isPlayable = true;
        puzzleTimeController.StartTimer();
    }


    public void GameOver()
    {
        isInProgress = false;
        isPlayable = false;
        StartCoroutine(ClearCheck());
    }

    IEnumerator ClearCheck()
    {
        while (isErasing)
        {
            yield return null;
        }

        if (puzzleScoreController.currentScore >= clearScore)
        {
            // 成功処理
            Debug.Log("成功");
        }
        else
        {
            // 失敗処理
            Debug.Log("失敗");
        }
    }


    /// <summary>
    /// 対象の位置がボード内でオーブがあるかどうかを確認する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsValidPos(int x, int y)
    {
        return x >= 0 && x < board.Length && board[x] != null && y >= 0 && y < board[x].Length && board[x][y] != null;
    }

    public IEnumerator EraseOrbs(List<Orb> eraseOrbs)
    {
        isErasing = true;
        isPlayable = false;
        int comboCount = 0;

        // オーブが消え続ける限りループ
        while (eraseOrbs.Count > 0)
        {
            comboCount++;
            // スコアを加算
            int erasedOrbCount = eraseOrbs.Count;
            int addScore = puzzleScoreController.CalculateScore(erasedOrbCount, comboCount);
            puzzleScoreController.AddScore(addScore);

            // オーブを消す
            foreach (Orb eraseOrb in eraseOrbs)
            {
                int[] pos = eraseOrb.pos;
                board[pos[0]][pos[1]] = null;
                Destroy(eraseOrb.gameObject);
            }
            // オーブを落とす
            yield return StartCoroutine(DropOrbs());
            // 盤面をチェックして、消せるオーブがあれば消す
            eraseOrbs = CheckBoard();
            if (eraseOrbs.Count == 0)
            {
                // 一通りオーブが消え終わったら新しいオーブを生成
                yield return StartCoroutine(generateOrbs.GenerateNewOrbs());
                // 生成後に再度盤面をチェック
                eraseOrbs = CheckBoard();
            }
        }

        isErasing = false;
        if (isInProgress) isPlayable = true;
    }

    public List<Orb> CheckBoard()
    {
        // 消すオーブをリストアップ
        List<Orb> eraseOrbs = new List<Orb>();
        bool[,] visited = new bool[board.Length, board[0].Length];
        for (int x = 0; x < board.Length; x++)
            for (int y = 0; y < board[x].Length; y++)
                if (board[x][y] != null && !visited[x, y])
                {
                    List<Orb> connectedOrbs = board[x][y].GetConnectedOrb();
                    foreach (Orb orb in connectedOrbs)
                        visited[orb.pos[0], orb.pos[1]] = true;
                    if (connectedOrbs.Count >= eraseCount)
                        eraseOrbs.AddRange(connectedOrbs);
                }
        return eraseOrbs;
    }

    
    IEnumerator DropOrbs()
    {
        List<Orb> movingOrbs = new List<Orb>();
        while (true)
        {
            movingOrbs.Clear();
            // 落ちる全てのオーブをリストアップ
            for (int x = 0; x < board.Length; x++)
            {
                bool hasEmpty = false;
                for (int y = 0; y < board[x].Length; y++)
                {
                    if (!hasEmpty)
                    {
                        if (board[x][y] == null) hasEmpty = true;
                        //Debug.Log("Checking position (" + x + ", " + y + "): " + (board[x][y] == null ? "Empty" : "Occupied"));
                    }
                    else
                        if (board[x][y] != null) movingOrbs.Add(board[x][y]);
                }
            }
            // 落ちるオーブがいなければ終了
            if (movingOrbs.Count == 0)
            {
                //Debug.Log("No more orbs to drop. Exiting drop loop.");
                break;
            }
            //Debug.Log("Dropping " + movingOrbs.Count + " orbs.");

            float duration = 0f;
            // オーブを一段落とす
            while (true)
            {
                yield return null;
                bool allLanded = true;
                duration += Time.deltaTime;
                foreach (Orb movingOrb in movingOrbs)
                {
                    int x = movingOrb.pos[0];
                    int y = movingOrb.pos[1];
                    Vector2 targetPos = prescribedOrbPos[x,y-1];
                    if (Vector2.Distance(movingOrb.transform.position, targetPos) > 0.01f && duration < forceDropDelay)
                    {
                        movingOrb.transform.position = Vector2.MoveTowards(movingOrb.transform.position, targetPos, dropSpeed * Time.deltaTime);
                        allLanded = false;
                    }
                    else
                        movingOrb.transform.position = targetPos;
                }

                // すべてのオーブが着地したら終了
                if (allLanded) break;
            }

            // ボード情報を更新
            foreach (Orb movingOrb in movingOrbs)
            {
                int x = movingOrb.pos[0];
                int y = movingOrb.pos[1];
                board[x][y - 1] = movingOrb;
                board[x][y] = null;
                movingOrb.pos[1] = y - 1;
            }
        }
    }


    IEnumerator OrbDropOnGameStart()
    {
        float duration = 0f;
        //生成したオーブを落とす
        while (true)
        {
            yield return null;
            bool allLanded = true;
            duration += Time.deltaTime;
            for (int x = 0; x < board.Length; x++)
            {
                for (int y = 0; y < board[x].Length; y++)
                {
                    Orb movingOrb = board[x][y];
                    Vector2 targetPos = prescribedOrbPos[x, y];
                    if (Vector2.Distance(movingOrb.transform.position, targetPos) > 0.01f && duration < forceDropDelay)
                    {
                        movingOrb.transform.position = Vector2.MoveTowards(movingOrb.transform.position, targetPos, dropSpeed * Time.deltaTime);
                        allLanded = false;
                    }
                    else
                        movingOrb.transform.position = targetPos;
                }
            }

            // すべてのオーブが着地したら終了
            if (allLanded) break;
        }
    }
}
