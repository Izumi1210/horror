using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootingController : MonoBehaviour
{
    public static ShootingController instance;

    [SerializeField] List<EnemyBoard> enemyBoards;
    public bool isInProgress = false;

    [Header("敵の出現頻度")]
    [SerializeField] float averageEnemyActiveTime = 2.0f;
    [SerializeField] float randomRangeEnemyActiveTime = 1.0f;

    [Header("HP")]
    [SerializeField] int playerHP = 3;
    [SerializeField] GameObject[] hpUI;

    [Header("制限時間")]
    [SerializeField] float gameTimeLimit = 20.0f;
    [SerializeField] TextMeshProUGUI timeNumber;

    [Header("シーン遷移")]
    [SerializeField] string gameOverSceneName;
    [SerializeField] string clearSceneName;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        RefreshEnemyBoard();
        PlayerHPChange(0);
        TimerUpdate(gameTimeLimit); // ←引数を渡す
        StartGame();
    }


    void RefreshEnemyBoard()
    {
        foreach (var e in enemyBoards)
        {
            e.SetStateDisable();
            e.gameObject.SetActive(true); // ビルドでもクリック可能に
        }
    }

    public void StartGame()
    {
        isInProgress = true;
        StartCoroutine(EnemySpawnCoroutine());
        StartCoroutine(GameTimerCoroutine());
    }

    IEnumerator EnemySpawnCoroutine()
    {
        while (isInProgress)
        {
            float waitTime = averageEnemyActiveTime + Random.Range(-randomRangeEnemyActiveTime, randomRangeEnemyActiveTime);
            yield return new WaitForSeconds(waitTime);

            int idx = Random.Range(0, enemyBoards.Count);
            if (enemyBoards[idx].GetState() == EnemyBoard.State.Disable)
                enemyBoards[idx].SetStateWait();
        }
    }

    IEnumerator GameTimerCoroutine()
    {
        float remainingTime = gameTimeLimit;
        while (remainingTime > 0)
        {
            yield return null;
            if (!isInProgress) yield break;
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(0f, remainingTime);
            TimerUpdate(remainingTime);
        }
        Clear();
    }

    void TimerUpdate(float remainingTime)
    {
        if (timeNumber != null)
            timeNumber.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    public void PlayerHPChange(int changeAmount)
    {
        playerHP += changeAmount;
        if (playerHP < 0) playerHP = 0;

        for (int i = 0; i < hpUI.Length; i++)
            hpUI[i].SetActive(i == playerHP);

        if (playerHP <= 0) Dead();
    }

    void Dead()
    {
        isInProgress = false;
        RefreshEnemyBoard();
        SceneManager.LoadScene(gameOverSceneName);
    }

    void Clear()
    {
        isInProgress = false;
        RefreshEnemyBoard();
        SceneManager.LoadScene(clearSceneName);
    }
}
