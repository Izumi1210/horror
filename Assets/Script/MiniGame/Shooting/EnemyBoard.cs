using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyBoard : MonoBehaviour, IPointerClickHandler
{
    public enum State
    {
        Wait,
        Ready,
        Attacking,
        Killed,
        Disable
    }

    State currentState = State.Disable;
    Animator enemyAnimator;
    SpriteRenderer enemySpriteRenderer;
    [Header("出現してから銃を構えるまでの時間")]
    [SerializeField] float waitTIme = 1.0f;
    [Header("銃を構えてから攻撃するまでの時間")]
    [SerializeField] float attackInterval = 2.0f;
    [Header("攻撃力")]
    [SerializeField] int attackPower = 1;
    Coroutine waitCoroutine;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer == null)
        {
            Debug.LogAssertion("SpriteRendererがアタッチされていません。");
        }
    }

    public void SetStateWait()
    {
        enemySpriteRenderer.enabled = true;
        currentState = State.Wait;
        enemyAnimator.SetTrigger("Wait");

        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);
        waitCoroutine = StartCoroutine(Wait());
    }

    public void SetStateReady()
    {
        enemySpriteRenderer.enabled = true;
        currentState = State.Ready;
        enemyAnimator.SetTrigger("Ready");
    }

    public void SetStateAttacking()
    {
        enemySpriteRenderer.enabled = true;
        currentState = State.Attacking;
        enemyAnimator.SetTrigger("Attack");
    }

    public void SetStateKilled()
    {
        enemySpriteRenderer.enabled = true;
        if (currentState == State.Wait)
            enemyAnimator.SetTrigger("DamagedWhileWait");
        else if (currentState == State.Attacking || currentState == State.Ready)
            enemyAnimator.SetTrigger("DamagedWhileShoot");

        // 攻撃を止める
        StopCoroutine(waitCoroutine);
        waitCoroutine = null;
        currentState = State.Killed;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTIme);
        SetStateReady();
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            SetStateAttacking();
        }
    }

    public void SetStateDisable()
    {
        enemySpriteRenderer.enabled = false;
        currentState = State.Disable;
    }

    void EnemyAttack()
    {
        ShootingController.instance.PlayerHPChange(-attackPower);
    }

    /// <summary>
    /// クリックされた時に呼び出される
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == State.Wait || currentState == State.Ready || currentState == State.Attacking)
        {
            SetStateKilled();
        }
    }

    public State GetState() { return currentState; }
}
