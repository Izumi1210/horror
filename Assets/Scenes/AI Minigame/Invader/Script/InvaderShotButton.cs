using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvaderShotButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject invaderShot;
    [SerializeField] Transform shotPoint;
    [Header("ボタンアニメーション用")]
    [SerializeField] GameObject buttonNormal;
    [SerializeField] GameObject buttonPushed;

    void OnEnable() { ChangePushedState(false); }

    public void OnPointerDown(PointerEventData _eventData)
    {
        GameObject nextBullet = InvaderGameController.instance.GetNextAmmo();
        if (nextBullet != null){
            Instantiate(nextBullet, shotPoint.position, Quaternion.identity);
        }
        StartCoroutine(PushedRoutine());
    }


    IEnumerator PushedRoutine()
    {
        ChangePushedState(true);
        
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
                break;
            yield return null;
        }

        ChangePushedState(false);

        yield return null;
    }


    void ChangePushedState(bool _isPushed)
    {
        buttonNormal.SetActive(!_isPushed);
        buttonPushed.SetActive(_isPushed);
    }
}
