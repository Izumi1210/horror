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
    [Header("制限時間")]
    [SerializeField] float gameTimeLimit = 30.0f;
    void Start()
    {
        RefreshEnemyBoard();
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

    /// <summary>
    /// ゲーム終了までの時間を計測するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootingGameTimerCoroutine()
    {
        float elapse = 0.0f;
        while (elapse < gameTimeLimit)
        {
            yield return null;
            elapse += Time.deltaTime;
        }
        Debug.Log("ミニゲーム終了");
        isInProgress = false;
        RefreshEnemyBoard();
    }

    void EnemyAwake(EnemyBoard enemyBoard)
    {
        enemyBoard.SetStateStandby();
    }
}
