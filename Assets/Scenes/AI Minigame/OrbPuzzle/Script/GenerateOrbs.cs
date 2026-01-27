using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Orb;

public class GenerateOrbs : MonoBehaviour
{
    [Header("各色のオーブのprefab")]
    [SerializeField] GameObject redOrbPrefab;
    [SerializeField] GameObject blueOrbPrefab;
    [SerializeField] GameObject greenOrbPrefab;
    [SerializeField] GameObject yellowOrbPrefab;
    [SerializeField] GameObject purpleOrbPrefab;

    [Header("各マスの定位置を示すTransform")]
    [SerializeField] Transform[] orbPositionsX;
    [SerializeField] Transform[] orbPositionsY;
    [SerializeField] Transform[] orbGeneratePositionsX;
    [SerializeField] Transform[] orbGeneratePositionsY;

    [SerializeField] int boardSizeX = 6;
    [SerializeField] int boardSizeY = 8;

    /// <summary>
    /// boardSizeX x boardSizeYのオーブの2次元配列を初期化して返す
    /// </summary>
    /// <returns></returns>
    public Orb[][] InitializeBoard()
    {
        Orb[][] board;
        board = new Orb[boardSizeX][];

        for (int x = 0; x < boardSizeX; x++)
        {
            board[x] = new Orb[boardSizeY];
            for (int y = 0; y < boardSizeY; y++)
            {
                Vector2 instantiatePos = new Vector2(orbGeneratePositionsX[x].position.x, orbGeneratePositionsY[y].position.y);
                GameObject newOrb = InstantiateOrb(x, y, instantiatePos);
                board[x][y] = newOrb.GetComponent<Orb>();
                if (board[x][y] == null)
                    Debug.LogError("Orbコンポーネントが見つかりません。");
            }
        }

        return board;
    }

    public Vector2[,] GetPrescribedOrbPosition()
    {
        Vector2[,] prescribedOrbPos = new Vector2[boardSizeX, boardSizeY];
        for(int x = 0; x < boardSizeX; x++)
            for (int y = 0; y < boardSizeY; y++)
                prescribedOrbPos[x, y] = new Vector2(orbPositionsX[x].position.x, orbPositionsY[y].position.y);

        return prescribedOrbPos;
    }


    /// <summary>
    /// boardの(x, y)のオーブの色を変えた後のboardを返す
    /// </summary>
    /// <param name="board">オーブが含まれる二次元配列</param>
    /// <param name="x">色を変えるオーブのx座標</param>
    /// <param name="y">色を変えるオーブのy座標</param>
    /// <returns></returns>
    public Orb[][] ChangeOrbColor(Orb[][] board, int x, int y)
    {
        OrbColor oldColor = board[x][y].orbColor;
        OrbColor newColor = OrbColor.Red;   // 仮に色を割り当てて初期化
        // 今の色と違う色にする
        switch (oldColor)
        {
            case OrbColor.Red:
                newColor = OrbColor.Blue;
                break;

            case OrbColor.Blue:
                newColor = OrbColor.Green;
                break;

            case OrbColor.Green:
                newColor = OrbColor.Yellow;
                break;

            case OrbColor.Yellow:
                newColor = OrbColor.Purple;
                break;

            case OrbColor.Purple:
                newColor = OrbColor.Red;
                break;
        }

        // 古いオーブを消して新しいオーブを生成
        Destroy(board[x][y].gameObject);
        Vector2 instantiatePos = new Vector2(orbGeneratePositionsX[x].position.x, orbGeneratePositionsY[y].position.y);
        GameObject newOrb = Instantiate(
            newColor switch
            {
                OrbColor.Red => redOrbPrefab,
                OrbColor.Blue => blueOrbPrefab,
                OrbColor.Green => greenOrbPrefab,
                OrbColor.Yellow => yellowOrbPrefab,
                OrbColor.Purple => purpleOrbPrefab,
                _ => null
            },
            new Vector2(instantiatePos.x, instantiatePos.y),
            Quaternion.identity,
            this.transform
        );
        newOrb.GetComponent<Orb>().pos[0] = x;
        newOrb.GetComponent<Orb>().pos[1] = y;
        board[x][y] = newOrb.GetComponent<Orb>();
        if (board[x][y] == null)
            Debug.LogError("Orbコンポーネントが見つかりません。");

        return board;
    }

    /// <summary>
    /// board内で足りなくなった分のオーブを生成して落とす
    /// </summary>
    /// <returns></returns>
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
        Vector2[,] prescribedOrbPos = PuzzleGameController.instance.prescribedOrbPos;
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
                Vector2 targetPos = prescribedOrbPos[x,y];
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


    /// <summary>
    /// オーブのインスタンスを生成して返す
    /// </summary>
    /// <param name="x">生成するオーブの、board内のx座標</param>
    /// <param name="y">生成するオーブの、board内のy座標</param>
    /// <param name="instantiatePos">生成するオーブのWorld座標</param>
    /// <returns></returns>
    GameObject InstantiateOrb(int x, int y, Vector2 instantiatePos)
    {
        int orbColor = UnityEngine.Random.Range(1, 6); // ランダムに色を5色から選ぶ
        GameObject newOrb = Instantiate(
            orbColor switch
            {
                1 => redOrbPrefab,
                2 => blueOrbPrefab,
                3 => greenOrbPrefab,
                4 => yellowOrbPrefab,
                5 => purpleOrbPrefab,
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


    /// <summary>
    /// boardに存在するオーブを全て破壊する
    /// </summary>
    /// <param name="board"></param>
    public void DestroyBoard(Orb[][] board)
    {
        for (int x = 0; x < boardSizeX; x++)
            for (int y = 0; y < boardSizeY; y++)
                if (board[x][y] != null)
                {
                    Destroy(board[x][y].gameObject);
                    board[x][y] = null;
                }
    }
}
