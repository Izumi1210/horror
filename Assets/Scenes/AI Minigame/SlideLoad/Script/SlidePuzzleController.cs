using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzleController : MonoBehaviour
{
    public static SlidePuzzleController instance;

    private void Awake()
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

    private void OnDestroy()
    {
        instance = null;
    }

    [Header("ボードの大きさ")]
    [SerializeField] Vector2Int boardSize;
    [Header("全てのタイルを入れる")]
    [SerializeField] SlidePuzzleTile[] tiles;
    public SlidePuzzleTile[,] board;

    [Header("スタートとゴールのタイル")]
    [SerializeField] SlidePuzzleTileStart startTile;
    [SerializeField] SlidePuzzleTileGoal goalTile;

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {
        board = new SlidePuzzleTile[boardSize.x, boardSize.y];

        foreach (var tile in tiles)
        {
            if(tile != null)
            {
                if (board[tile.currentPosition.x, tile.currentPosition.y] != null)
                    Debug.LogWarning("同じ位置に二つ以上のタイルがあります: " + tile.currentPosition);
                board[tile.currentPosition.x, tile.currentPosition.y] = tile;

            }
        }

        foreach (var tile in board)
        {
            Debug.Assert(tile != null, "Boardの初期化に失敗: Board内に空白があります");
        }
    }

    /// <summary>
    /// 周囲の4マスに空があるかを確認し、空があればその座標を返す。空がなければ元の座標を返す。
    /// </summary>
    /// <param name="checkPos">周囲に空があるかチェックする位置</param>
    /// <returns></returns>
    public Vector2Int VoidCheck(Vector2Int checkPos)
    {
        if (checkPos.x > 0 && board[checkPos.x - 1, checkPos.y].isVoid())
        {
            return new Vector2Int(checkPos.x - 1, checkPos.y);
        }
        else if (checkPos.x < boardSize.x - 1 && board[checkPos.x + 1, checkPos.y].isVoid())
        {
            return new Vector2Int(checkPos.x + 1, checkPos.y);
        }
        else if (checkPos.y > 0 && board[checkPos.x, checkPos.y - 1].isVoid())
        {
            return new Vector2Int(checkPos.x, checkPos.y - 1);
        }
        else if (checkPos.y < boardSize.y - 1 && board[checkPos.x, checkPos.y + 1].isVoid())
        {
            return new Vector2Int(checkPos.x, checkPos.y + 1);
        }

        return checkPos;
    }


    /// <summary>
    /// 二つのタイルの位置を入れ替えるメソッド。
    /// </summary>
    /// <param name="tilePos1"></param>
    /// <param name="tilePos2"></param>
    public void SwapTile(Vector2Int tilePos1, Vector2Int tilePos2)
    {
        // 見た目のpositionの入れ替え
        Vector3 pos1 = board[tilePos1.x, tilePos1.y].transform.position;
        Vector3 pos2 = board[tilePos2.x, tilePos2.y].transform.position;
        board[tilePos1.x, tilePos1.y].transform.position = pos2;
        board[tilePos2.x, tilePos2.y].transform.position = pos1;

        // タイルの参照の入れ替え
        SlidePuzzleTile temp = board[tilePos1.x, tilePos1.y];
        board[tilePos1.x, tilePos1.y] = board[tilePos2.x, tilePos2.y];
        board[tilePos2.x, tilePos2.y] = temp;

        // currentPositionの入れ替え
        board[tilePos1.x, tilePos1.y].currentPosition = tilePos1;
        board[tilePos2.x, tilePos2.y].currentPosition = tilePos2;
    }


    /// <summary>
    /// スタート地点からゴール地点に向かって、道が繋がっているかを確認するメソッド。
    /// </summary>
    /// <returns>スタートからゴールに道が繋がっているかどうか</returns>
    public void CheckClear()
    {
        // ゴールからスタートに向かって、道が繋がっているかを確認する
        if (ExploreRoad(startTile.currentPosition))
        {
            Debug.Log("クリア");
        }
    }

    /// <summary>
    /// currentPosからスタートして、ゴールにたどり着けるかを再帰的に探索するメソッド。visitedは、すでに訪れた位置を記録するためのリスト。初回呼び出し時にはnullを渡すと、メソッド内で新しいリストが作成される。
    /// </summary>
    /// <param name="currentPos">探索を開始する地点</param>
    /// <param name="visited">既に訪問済みのタイルのリスト</param>
    /// <returns></returns>
    public bool ExploreRoad(Vector2Int currentPos, HashSet<Vector2Int> visited = null)
    {
        // 初回呼び出し時にリストを初期化
        if (visited == null) visited = new HashSet<Vector2Int>();

        // 範囲外ならfalse
        if (currentPos.x < 0 || currentPos.x >= boardSize.x || currentPos.y < 0 || currentPos.y >= boardSize.y)
            return false;
        // 無限ループ防止のため、すでに訪れた位置ならfalse
        if (visited.Contains(currentPos)) return false;
        visited.Add(currentPos);

        var tile = board[currentPos.x, currentPos.y];
        // 空白ならfalse
        if (tile.isVoid())
            return false;
        // ゴールならtrue
        if (tile.isGoal())
            return true;

        // connectFlagは、x-1、x+1、y-1、y+1 の順で接続可能かを示すフラグ（0: 接続不可, 1: 接続可能）
        // このタイルから接続可能な方向に、再帰的に探索する

        // X-1方向
        if (tile.connectableFlag[0] != 0)
        {
            // X-1方向のタイルがあるかどうかと、「x+1側」が開いているか確認
            if (currentPos.x > 0 && board[currentPos.x - 1, currentPos.y].connectableFlag[1] != 0)
            {
                if ( ExploreRoad(new Vector2Int(currentPos.x - 1, currentPos.y), visited) )
                    return true;
            }
        }

        // X+1方向
        if (tile.connectableFlag[1] != 0)
        {
            // X+1方向のタイルがあるかどうかと、「x-1側」が開いているか確認
            if (currentPos.x < boardSize.x - 1 && board[currentPos.x + 1, currentPos.y].connectableFlag[0] != 0)
            {
                if ( ExploreRoad(new Vector2Int(currentPos.x + 1, currentPos.y), visited) )
                    return true;
            }
        }

        // Y-1方向
        if (tile.connectableFlag[2] != 0)
        {
            // Y-1方向のタイルがあるかどうかと、「y+1側」が開いているか確認
            if (currentPos.y > 0 && board[currentPos.x, currentPos.y - 1].connectableFlag[3] != 0)
            {
                if ( ExploreRoad(new Vector2Int(currentPos.x, currentPos.y - 1), visited) )
                    return true;
            }
        }

        // Y+1方向
        if (tile.connectableFlag[3] != 0)
        {
            // Y+1方向のタイルがあるかどうかと、「y-1側」が開いているか確認
            if (currentPos.y < boardSize.y - 1 && board[currentPos.x, currentPos.y + 1].connectableFlag[2] != 0)
            {
                if ( ExploreRoad(new Vector2Int(currentPos.x, currentPos.y + 1), visited) )
                    return true;
            }
        }

        // どの方向にもゴールがなければfalse
        return false;
    }
}
