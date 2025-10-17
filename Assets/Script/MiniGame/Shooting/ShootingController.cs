using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingController : MonoBehaviour
{
    [SerializeField] List<EnemyBoard> enemyBoards;
    public bool isInProgress = false;
    [Header("敵の出現頻度")]
    [SerializeField] float averageEnemyActiveTime = 2.0f;
    [SerializeField] float RandomRangeEnemyActiveTime = 1.0f;
    [Header("敵の攻撃の速さ")]
    [SerializeField] float enemyStandbyTime = 0.5f;
    [SerializeField] float enemyActiveTime = 1.0f;

    void Start()
    {
        RefreshEnemyBorad();
        StartGame();
    }

    void RefreshEnemyBorad()
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
        StartCoroutine(ShootingGameCoroutine());
    }

    /// <summary>
    /// ミニゲーム中の敵の挙動を制御するコルーチン
    /// </summary>
    IEnumerator ShootingGameCoroutine()
    {
        while (isInProgress)
        {
            // ある程度の幅のある待機時間の後、ランダムな敵をスタンバイ状態にする
            float waitTime = averageEnemyActiveTime + UnityEngine.Random.Range(-RandomRangeEnemyActiveTime, RandomRangeEnemyActiveTime);
            yield return new WaitForSeconds(waitTime);

            int activeEnemyIndex = UnityEngine.Random.Range(0, enemyBoards.Count);
            if(enemyBoards[activeEnemyIndex].GetIsDisable())
                StartCoroutine(SingleEnemyController(enemyBoards[activeEnemyIndex]));
        }
    }

    IEnumerator SingleEnemyController(EnemyBoard enemyboard)
    {
        // スタンバイ状態にする
        enemyboard.SetStateStandby();
        yield return new WaitForSeconds(enemyStandbyTime);

        // アクティブ状態にする
        enemyboard.SetStateActive();
        yield return new WaitForSeconds(enemyActiveTime);

        // 攻撃判定
        // ここに攻撃判定の処理を追加する

        // 無効状態にする
        enemyboard.SetStateDisable();
    }
}
