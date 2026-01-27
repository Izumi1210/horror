using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvaderClickToStart : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject startCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        startCanvas.SetActive(false);
        InvaderGameController.instance.GameStart();
    }
}
