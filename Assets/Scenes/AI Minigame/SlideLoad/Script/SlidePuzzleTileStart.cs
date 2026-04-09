using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SlidePuzzleTileStart : SlidePuzzleTile
{
    // スタート地点のタイル
    
    public override void OnPointerDown(PointerEventData _eventData)
    {
        // クリックしても何もしない
    }


    public override bool isVoid()
    {
        return false;
    }
}
