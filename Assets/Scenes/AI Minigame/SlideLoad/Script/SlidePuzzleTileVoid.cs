using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SlidePuzzleTileVoid : SlidePuzzleTile
{
    // このタイルはそこが空であることを意味する

     public override void OnPointerDown(PointerEventData _eventData)
    {
        // 空のタイルはクリックしても何もしない
    }


    public override bool isVoid()
    {
        return true;
    }
}
