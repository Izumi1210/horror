using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderEnemySponer : MonoBehaviour
{
    [Serializable]
    class SpawnInfo
    {
        public GameObject enemyPrefab;  // スポーンする敵のプレハブ
        public float spawnIntervalMax;  // 最大スポーン間隔
        public float spawnIntervalMin;  // 最小スポーン間隔
        public float timer;             // この敵がスポーンするまでの時間
        public bool isSpawnable;        // この敵がスポーンするかどうか
        public SpawnInfo(GameObject _prefab, float _intervalMax,float _intervalMin, bool _isSpawnable)
        {
            enemyPrefab = _prefab;
            spawnIntervalMax = _intervalMax;
            spawnIntervalMin = _intervalMin;
            isSpawnable = _isSpawnable;
            timer = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
    }

    [SerializeField] List<SpawnInfo> enemyList; // スポーンする敵のリスト

    private void FixedUpdate()
    {
        // ゲームが進行中でなければ何もしない
        if (!InvaderGameController.instance.isInProgress)
            return;

        // 一度に複数の敵がスポーンしないようにする
        bool isSpawnable = true;

        foreach (SpawnInfo enemy in enemyList) { 
            enemy.timer -= Time.fixedDeltaTime;
            if (enemy.timer <= 0 && isSpawnable)
            {
                Instantiate(enemy.enemyPrefab, transform.position, Quaternion.identity);
                isSpawnable = false;

                enemy.timer = UnityEngine.Random.Range(enemy.spawnIntervalMin, enemy.spawnIntervalMax);
            }
        }
    }
}