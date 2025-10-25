using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingController : MonoBehaviour
{
    public static ShootingController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] List<EnemyBoard> enemyBoards;
    public bool isInProgress = false;
    
    [Header("敵の出現頻度")]
    [SerializeField] float averageEnemyActiveTime = 2.0f;
    [SerializeField] float RandomRangeEnemyActiveTime = 1.0f;
    [Header("敵の攻撃の速さ")]
    [SerializeField] float enemyStandbyTime = 0.5f;
    [SerializeField] float enemyActiveTime = 1.0f;

    [Header("HP")]
    [SerializeField] int playerHP = 3;
    [Tooltip("添字が大きい方ほどHPが多い")]
    [SerializeField] GameObject[] hpUI;

    [Header("制限時間")]
    [SerializeField] const float gameTimeLimit = 20.0f;
    [Header("ミニゲームの残り時間")]
    [SerializeField] float remainingTime = 20.0f;
    [Header("時間の表示")]
    [SerializeField] TextMeshProUGUI timeNumber;

    void Start()
    {
        RefreshEnemyBoard();    // 敵の初期化
        PlayerHPChange(0);      // HP表示の初期化
        TimerUpdate();          // 時間表示の初期化
        StartGame();
    }

    /// <summary>
    /// 全ての敵を非アクティブ状態にする
    /// </summary>
    void RefreshEnemyBoard()
    {
        foreach (var enemy in enemyBoards)
            enemy.SetStateDisable();
        foreach (var enemy in enemyBoards)
            enemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// この関数を呼び出すとミニゲームが開始される
    /// </summary>
    public void StartGame()
    {
        isInProgress = true;
        foreach (var enemy in enemyBoards)
            enemy.gameObject.SetActive(true);
        StartCoroutine(ShootingGameEnemyCoroutine());
        StartCoroutine(ShootingGameTimerCoroutine());
    }

    /// <summary>
    /// ミニゲーム中の敵の挙動を制御するコルーチン
    /// </summary>
    IEnumerator ShootingGameEnemyCoroutine()
    {
        while (isInProgress)
        {
            // ある程度の幅のある待機時間の後、ランダムな敵をスタンバイ状態にする
            float waitTime = averageEnemyActiveTime + UnityEngine.Random.Range(-RandomRangeEnemyActiveTime, RandomRangeEnemyActiveTime);
            yield return new WaitForSeconds(waitTime);

            int activeEnemyIndex = UnityEngine.Random.Range(0, enemyBoards.Count);
            if(enemyBoards[activeEnemyIndex].GetState() == EnemyBoard.State.Disable)
                EnemyAwake(enemyBoards[activeEnemyIndex]);
        }
    }

    void EnemyAwake(EnemyBoard enemyBoard)
    {
        enemyBoard.SetStateStandby();
    }

    /// <summary>
    /// ゲーム終了までの時間を計測するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootingGameTimerCoroutine()
    {
        remainingTime = gameTimeLimit;
        while (remainingTime > 0)
        {
            yield return null;
            if (!isInProgress)
                yield break;
            remainingTime = Mathf.Max(0f, remainingTime -= Time.deltaTime);
            TimerUpdate();
        }
        remainingTime = 0f;
        Clear();
    }

    /// <summary>
    /// 時間の表示を更新する
    /// </summary>
    public void TimerUpdate()
    {
        timeNumber.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    /// <summary>
    /// プレイヤーのHPを変更し、表示を更新する
    /// </summary>
    public void PlayerHPChange(int changeAmount)
    {
        playerHP += changeAmount;
        for (int i = 0; i < hpUI.Length; i++)
        {
            hpUI[i].SetActive(i == playerHP);
        }
        
        if (playerHP <= 0)
        {
            hpUI[0].SetActive(true);
            Dead();
        }
    }

    /// <summary>
    /// プレイヤーが死亡した時の処理
    /// </summary>
    public void Dead()
    {
        isInProgress = false;
        TimerUpdate();
        RefreshEnemyBoard();
        Debug.Log("プレイヤー死亡");
    }

    public void Clear()
    {
        isInProgress = false;
        TimerUpdate();
        RefreshEnemyBoard();
        Debug.Log("ミニゲームクリア");
    }
}
