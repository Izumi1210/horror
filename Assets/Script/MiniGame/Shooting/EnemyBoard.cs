using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))] // ←クリック可能にする
public class EnemyBoard : MonoBehaviour, IPointerClickHandler
{
    public enum State { Wait, Ready, Attacking, Killed, Disable }
    State currentState = State.Disable;

    Animator enemyAnimator;
    SpriteRenderer enemySpriteRenderer;

    [SerializeField] float waitTime = 1.0f;
    [SerializeField] float attackInterval = 2.0f;
    [SerializeField] int attackPower = 1;

    Coroutine waitCoroutine;

    [Header("効果音")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sfxEnemyAttack;
    [SerializeField] AudioClip sfxEnemyKilled;

    [SerializeField] Damage damageEffect;

    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (damageEffect == null) damageEffect = FindObjectOfType<Damage>();
    }

    public void SetStateWait()
    {
        currentState = State.Wait;
        enemySpriteRenderer.enabled = true;
        enemyAnimator.SetTrigger("Wait");

        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);

        waitCoroutine = StartCoroutine(WaitRoutine());
    }

    IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        // 撃破済み/無効状態ならここで終了
        if (currentState == State.Killed || currentState == State.Disable)
        {
            waitCoroutine = null;
            yield break;
        }

        SetStateReady();

        while (currentState != State.Killed && currentState != State.Disable)
        {
            yield return new WaitForSeconds(attackInterval);

            // 攻撃前に撃破されていれば終了
            if (currentState == State.Killed || currentState == State.Disable) break;

            SetStateAttacking();

            // 攻撃アニメイベントから EnemyAttack() が呼ばれる想定
        }

        waitCoroutine = null;
    }

    public void SetStateReady()
    {
        currentState = State.Ready;
        enemyAnimator.SetTrigger("Ready");
    }

    public void SetStateAttacking()
    {
        currentState = State.Attacking;
        enemyAnimator.SetTrigger("Attack");

        if (audioSource != null && sfxEnemyAttack != null)
            audioSource.PlayOneShot(sfxEnemyAttack);
    }

    void EnemyAttack()
    {
        if (ShootingController.instance != null)
        {
            ShootingController.instance.PlayerHPChange(-attackPower);
        }

        if (damageEffect != null)
            damageEffect.ShowDamage();
    }

    public void SetStateKilled()
    {
        if (currentState == State.Killed || currentState == State.Disable) return;

        // 現在の State を保持
        State prevState = currentState;
        currentState = State.Killed;

        // 待機コルーチンを止める
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        // アニメーション
        if (enemyAnimator != null)
        {
            if (prevState == State.Wait)
                enemyAnimator.SetTrigger("DamagedWhileWait");
            else
                enemyAnimator.SetTrigger("DamagedWhileShoot");
        }

        // 撃破音
        if (audioSource != null && sfxEnemyKilled != null)
            audioSource.PlayOneShot(sfxEnemyKilled);
    }


    public void SetStateDisable()
    {
        currentState = State.Disable;
        enemySpriteRenderer.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == State.Wait || currentState == State.Ready || currentState == State.Attacking)
        {
            SetStateKilled();
        }
    }

    public State GetState() => currentState;
}
