using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SlidePuzzleTileStatic : SlidePuzzleTile
{
    // このタイルは動かせないが空でもない
    
    public override void OnPointerDown(PointerEventData _eventData)
    {
        // クリックしても何もしない
    }


    public override bool isVoid()
    {
        return false;
    }
}
