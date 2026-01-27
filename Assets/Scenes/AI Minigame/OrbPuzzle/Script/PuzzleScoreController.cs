using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleScoreController : MonoBehaviour
{
    [Header("オーブ一つの基本の点数")]
    [SerializeField] int baseScorePerOrb = 100;
    [Header("同時消し数が1増えるごとに乗算されるスコア倍率")]
    [SerializeField] float simultaneousBonus = 1.1f;
    [Header("コンボ数が1増えるごとに乗算されるスコア倍率")]
    [SerializeField] float comboBonus = 1.2f;
    [Header("スコア表示用テキスト")]
    [SerializeField] TextMeshProUGUI scoreText;
    public int currentScore = 0;
    public int MaxScore = 99999999;

    private void Start()
    {
        UpdateScore();
    }


    public int AddScore(int addAmount)
    {
        currentScore += addAmount;
        if(currentScore > MaxScore) currentScore = MaxScore;
        UpdateScore();

        return currentScore;
    }

    public void UpdateScore()
    {
        scoreText.text = currentScore.ToString();
        int length = scoreText.text.Length;
        for (int i = 0; i < 8 - length; i++)
            scoreText.text = "0" + scoreText.text;
    }


    public int CalculateScore(int erasedOrbCount, int comboCount)
    {
        // 消したオーブの数とコンボに応じてスコアを計算
        float scoref = baseScorePerOrb * Mathf.Pow(simultaneousBonus, erasedOrbCount)
                                        * Mathf.Pow(comboBonus, comboCount);
        int score = Mathf.FloorToInt(scoref);
        if (score > MaxScore) score = MaxScore;

        return score;
    }
}
