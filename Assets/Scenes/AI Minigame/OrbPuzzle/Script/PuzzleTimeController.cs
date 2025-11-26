using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleTimeController : MonoBehaviour
{
    [Header("制限時間")]
    [SerializeField] float gameTimeLimit = 20.0f;
    [Header("ミニゲームの残り時間")]
    [SerializeField] float remainingTime = 20.0f;
    [Header("時間の表示")]
    [SerializeField] TextMeshProUGUI timeNumber;

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
