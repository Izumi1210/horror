using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GenerateOrbs : MonoBehaviour
{
    [Header("各色のオーブのprefab")]
    [SerializeField] GameObject redOrbPrefab;
    [SerializeField] GameObject blueOrbPrefab;
    [SerializeField] GameObject greenOrbPrefab;
    [SerializeField] GameObject yellowOrbPrefab;

    [Header("各マスの定位置を示すTransform")]
    [SerializeField] Transform[] orbPositionsX;
    [SerializeField] Transform[] orbPositionsY;
    [SerializeField] Transform[] orbGeneratePositionsX;
    [SerializeField] Transform[] orbGeneratePositionsY;

    [SerializeField] int boardSizeX = 6;
    [SerializeField] int boardSizeY = 8;


    public Orb[][] InitializeBoard()
    {
        Orb[][] board;
        board = new Orb[boardSizeX][];

        for (int x = 0; x < boardSizeX; x++)
        {
            board[x] = new Orb[boardSizeY];
            for (int y = 0; y < boardSizeY; y++)
            {
                Vector2 instantiatePos = new Vector2(orbPositionsX[x].position.x, orbPositionsY[y].position.y);
                GameObject newOrb = InstantiateOrb(x,y,instantiatePos);
                board[x][y] = newOrb.GetComponent<Orb>();
                if (board[x][y] == null)
                    Debug.LogError("Orbコンポーネントが見つかりません。");
            }
        }

        return board;
    }


    public IEnumerator GenerateNewOrbs()
    {
        Orb[][] board = PuzzleGameController.instance.board;
        // いくつオーブを生成するか決める
        int[] generateNum = new int[boardSizeX];
        for (int i = 0; i < boardSizeX; i++)
            generateNum[i] = 0;
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                if (board[x][y] == null)
                {
                    generateNum[x]++;
                }
            }
        }
        
        // オーブを生成する
        List<Orb> generatedOrbs = new List<Orb>();
        for(int x = 0; x < boardSizeX; x++)
        {
            // generateNum[x]個分オーブを生成してboardに追加してしまう
            // ただし空中に生成するので、位置はorbGeneratePositionsから取る
            for (int y = boardSizeY - generateNum[x]; y < boardSizeY; y++)
            {
                Vector2 instantiatePos = new Vector2(orbGeneratePositionsX[x].position.x, orbGeneratePositionsY[y - boardSizeY + generateNum[x]].position.y);
                GameObject newOrb = InstantiateOrb(x, y, instantiatePos);
                board[x][y] = newOrb.GetComponent<Orb>();
                if (board[x][y] == null)
                    Debug.LogError("Orbコンポーネントが見つかりません。");
                generatedOrbs.Add(board[x][y]);
            }
        }

        float duration = 0f;
        Vector2[][] prescribedOrbPos = PuzzleGameController.instance.prescribedOrbPos;
        float dropSpeed = PuzzleGameController.instance.dropSpeed;
        float forceDropDelay = PuzzleGameController.instance.forceDropDelay;
        //生成したオーブを落とす
        while (true)
        {
            yield return null;
            bool allLanded = true;
            duration += Time.deltaTime;
            foreach (Orb movingOrb in generatedOrbs)
            {
                int x = movingOrb.pos[0];
                int y = movingOrb.pos[1];
                Vector2 targetPos = prescribedOrbPos[x][y];
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

        PuzzleGameController.instance.board = board;
    }


    GameObject InstantiateOrb(int x, int y, Vector2 instantiatePos)
    {
        int orbColor = UnityEngine.Random.Range(1, 5); // ランダムに色を4色から選ぶ
        GameObject newOrb = Instantiate(
            orbColor switch
            {
                1 => redOrbPrefab,
                2 => blueOrbPrefab,
                3 => greenOrbPrefab,
                4 => yellowOrbPrefab,
                _ => null
            },
            new Vector2(instantiatePos.x, instantiatePos.y),
            Quaternion.identity,
            this.transform
        );
        newOrb.GetComponent<Orb>().pos[0] = x;
        newOrb.GetComponent<Orb>().pos[1] = y;
        return newOrb;
    }

}
