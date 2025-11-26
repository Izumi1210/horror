using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Orb : MonoBehaviour, IPointerClickHandler
{
    public enum OrbColor
    {
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4
    }
    [SerializeField] OrbColor orbColor;
    [SerializeField] float dropSpeed;
    
    public int[] pos = new int[2];


    /// <summary>
    /// 隣接している同じ色のオーブを取得する
    /// </summary>
    /// <returns></returns>
    public Orb[] GetNeighbor()
    {
        Orb[] neighbors = new Orb[4];
        int x = pos[0];
        int y = pos[1];
        Orb[][] board = PuzzleGameController.instance.board;

        // 上
        if (PuzzleGameController.instance.IsValidPos(x, y - 1) && board[x][y - 1].orbColor == this.orbColor)
            neighbors[0] = board[x][y - 1];

        // 右
        if (PuzzleGameController.instance.IsValidPos(x + 1, y) && board[x + 1][y].orbColor == this.orbColor)
            neighbors[1] = board[x + 1][y];

        // 下
        if (PuzzleGameController.instance.IsValidPos(x, y + 1) && board[x][y + 1].orbColor == this.orbColor)
            neighbors[2] = board[x][y + 1];

        // 左
        if (PuzzleGameController.instance.IsValidPos(x - 1, y) && board[x - 1][y].orbColor == this.orbColor)
            neighbors[3] = board[x - 1][y];

        return neighbors;
    }


    /// <summary>
    /// 繋がっている同じ色のオーブを全て取得する
    /// </summary>
    /// <param name="connectedList"></param>
    /// <returns></returns>
    public List<Orb> GetConnectedOrb(List<Orb> connectedList = null)
    {
        if (connectedList == null)
        {
            connectedList = new List<Orb>();
        }else if (connectedList.Contains(this))
        {
            return connectedList;
        }
        connectedList.Add(this);

        Orb[] neighbors = GetNeighbor();
        foreach (Orb neighbor in neighbors)
        {
            if (neighbor != null)
            {
                connectedList = neighbor.GetConnectedOrb(connectedList);
            }
        }

        return connectedList;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (PuzzleGameController.instance.isPlayable)
        {
            Debug.Log("Orb clicked at position: (" + pos[0] + ", " + pos[1] + ")");
            List<Orb> connectedList = GetConnectedOrb();
            PuzzleGameController.instance.StartCoroutine(PuzzleGameController.instance.EraseOrbs(connectedList));
        }
    }
}
