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

    // ▼▼▼ 効果音 ▼▼▼
    [Header("効果音")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sfxEnemyAttack;   // 敵が攻撃するとき
    [SerializeField] AudioClip sfxEnemyKilled;   // 敵を倒したとき
    // ▲▲▲ 効果音 ▲▲▲

    // ▼ ダメージ演出
    [SerializeField] Damage damageEffect;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();

        if (enemySpriteRenderer == null)
            Debug.LogAssertion("SpriteRendererがアタッチされていません。");

        // AudioSource が未設定なら自動取得
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Damage を自動取得（シーン上のどこかにあるやつ）
        if (damageEffect == null)
            damageEffect = FindObjectOfType<Damage>();

        if (damageEffect == null)
            Debug.LogError("Damage がシーンに見つかりません。画面赤点滅が動きません。");
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

        // 敵攻撃SE
        if (audioSource != null && sfxEnemyAttack != null)
            audioSource.PlayOneShot(sfxEnemyAttack);
    }

    public void SetStateKilled()
    {
        enemySpriteRenderer.enabled = true;

        if (currentState == State.Wait)
            enemyAnimator.SetTrigger("DamagedWhileWait");
        else if (currentState == State.Attacking || currentState == State.Ready)
            enemyAnimator.SetTrigger("DamagedWhileShoot");

        // 撃破SE
        if (audioSource != null && sfxEnemyKilled != null)
            audioSource.PlayOneShot(sfxEnemyKilled);

        // 攻撃を止める
        if (waitCoroutine != null)
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
        // ダメージ処理
        ShootingController.instance.PlayerHPChange(-attackPower);

        // ★ 画面を赤くする演出（確実に null で落ちない）
        if (damageEffect != null)
            damageEffect.ShowDamage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == State.Wait || currentState == State.Ready || currentState == State.Attacking)
        {
            SetStateKilled();  // 撃破音が鳴る
        }
    }

    public State GetState() { return currentState; }
}
