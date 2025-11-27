using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleTimeController : MonoBehaviour
{
    [Header("時間の表示")]
    [SerializeField] TextMeshProUGUI timeNumber;

    // ゲームの制限時間
    float gameTimeLimit;
    // 残り時間
    float remainingTime;

    void Start()
    {
        TimerUpdate();          // 時間表示の初期化
    }


    /// <summary>
    /// 時間の表示を更新する
    /// </summary>
    public void TimerUpdate()
    {
        timeNumber.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    public void StartTimer()
    {
        StartCoroutine(PuzzleTimerCoroutine());
    }


    IEnumerator PuzzleTimerCoroutine()
    {
        gameTimeLimit = PuzzleGameController.instance.timeLimit;
        remainingTime = gameTimeLimit;
        while (remainingTime > 0)
        {
            yield return null;
            if (!PuzzleGameController.instance.isInProgress)
                yield break;
            remainingTime = Mathf.Max(0f, remainingTime -= Time.deltaTime);
            TimerUpdate();
        }
        remainingTime = 0f;
        TimerUpdate();

        PuzzleGameController.instance.GameOver();
    }
}
