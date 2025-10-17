using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class EnemyBoard : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer standbySr;
    public SpriteRenderer activeSr;
    public SpriteRenderer killedSr;
    public EnemyBoard(SpriteRenderer standby, SpriteRenderer active, SpriteRenderer killed)
    {
        standbySr = standby;
        activeSr = active;
        killedSr = killed;
    }

    public void SetStateStandby()
    {
        standbySr.enabled = true;
        activeSr.enabled = false;
        killedSr.enabled = false;
    }

    public void SetStateActive()
    {
        standbySr.enabled = false;
        activeSr.enabled = true;
        killedSr.enabled = false;
    }

    public void SetStateKilled()
    {
        standbySr.enabled = false;
        activeSr.enabled = false;
        killedSr.enabled = true;
    }

    public void SetStateDisable()
    {
        standbySr.enabled = false;
        activeSr.enabled = false;
        killedSr.enabled = false;
    }

    /// <summary>
    /// クリックされた時に呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        SetStateKilled();
    }

    public bool GetIsActive() { return activeSr.enabled; }

    public bool GetIsStandby() { return standbySr.enabled; }

    public bool GetIskilled() { return killedSr.enabled; }

    public bool GetIsDisable() { return !standbySr.enabled && !activeSr.enabled; }
}
