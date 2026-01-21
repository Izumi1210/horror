using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvaderShotButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject invaderShot;
    [SerializeField] Transform shotPoint;

    public void OnPointerClick(PointerEventData eventData)
    {
        Instantiate(invaderShot, shotPoint.position, Quaternion.identity);
    }
}
