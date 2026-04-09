using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SlidePuzzleTile : MonoBehaviour, IPointerDownHandler
{
    // ボード内でのタイルの位置を示す変数
    public Vector2Int currentPosition;

    //左(X-1) 右(X+1) 上(Y+1) 下(Y-1)に相当する
    [Header("左、右、上、下 の順で接続可能かを示すフラグ（0: 接続不可, 1: 接続可能）")]
    public int[] connectableFlag = new int[4]; 

    public virtual void OnPointerDown(PointerEventData _eventData)
    {
        Slide();
    }

    void Slide()
    {
        Vector2Int voidPos = SlidePuzzleController.instance.VoidCheck(currentPosition);
        if (voidPos != currentPosition)
        {
            SlidePuzzleController.instance.SwapTile(currentPosition, voidPos);
            SlidePuzzleController.instance.CheckClear();
        }
    }

    /// <summary>
    /// このタイルが空であるかを示すメソッド。通常のタイルは空ではないため、falseを返す。空のタイルはこのクラスを継承しているため、必要に応じてオーバーライドしてtrueを返すようにする。
    /// </summary>
    /// <returns></returns>
    public virtual bool isVoid()
    {
        return false;
    }

    /// <summary>
    /// このタイルがゴールであるかを示すメソッド。通常のタイルはゴールではないため、falseを返す。ゴールのタイルはこのクラスを継承しているため、必要に応じてオーバーライドしてtrueを返すようにする。
    /// </summary>
    /// <returns></returns>
    public virtual bool isGoal()
    {
        return false;
    }
}
