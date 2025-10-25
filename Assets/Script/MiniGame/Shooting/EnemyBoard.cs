using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class EnemyBoard : MonoBehaviour, IPointerClickHandler
{
    public enum State
    {
        Wait,
        Active,
        Killed,
        Disable
    }

    State currentState = State.Disable;
    [SerializeField] Animator enemyAnimator;
    SpriteRenderer enemySpriteRenderer;

    void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer == null)
        {
            Debug.LogAssertion("SpriteRendererがアタッチされていません。");
        }
    }

    public void SetStateStandby()
    {
        enemySpriteRenderer.enabled = true;
        currentState = State.Wait;
        enemyAnimator.SetTrigger("Ready");
    }

    public void SetStateActive()
    {
        enemySpriteRenderer.enabled = true;
        currentState = State.Active;
    }

    public void SetStateKilled()
    {
        enemySpriteRenderer.enabled = true;
        if (currentState == State.Wait)
            enemyAnimator.SetTrigger("DamagedWhileWait");
        else if (currentState == State.Active)
            enemyAnimator.SetTrigger("DamagedWhileShoot");

        currentState = State.Killed;
    }

    public void SetStateDisable()
    {
        enemySpriteRenderer.enabled = false;
        currentState = State.Disable;
    }

    void EnemyAttack()
    {
        ShootingController.instance.PlayerHPChange(-1);
    }

    /// <summary>
    /// クリックされた時に呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == State.Wait || currentState == State.Active)
        {
            SetStateKilled();
        }
    }

    public State GetState() { return currentState; }
}
