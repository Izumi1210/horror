using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToStart : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject startCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        startCanvas.SetActive(false);
        PuzzleGameController.instance.GameStart();
    }
}
