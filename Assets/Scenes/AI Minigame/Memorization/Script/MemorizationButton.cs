using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemorizationButton : MonoBehaviour, IPointerClickHandler
{
    [Header("‚±‚Ìƒ{ƒ^ƒ“‚ÉŠ„‚è“–‚Ä‚ç‚ê‚½Symbol")]
    [SerializeField] private MemorizationSymbol symbolInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(symbolInfo.name);
        MemorizationGameController.instance.ButtonCheck(symbolInfo);
    }

}
