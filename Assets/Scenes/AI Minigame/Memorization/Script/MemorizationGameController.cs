using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorizationGameController : MonoBehaviour
{
    public static MemorizationGameController instance;

    private void Awake()
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

    private void OnDestroy()
    {
        instance = null;
    }

    [Header("全てのMemorizationSymbolを入れる")]
    [SerializeField] private MemorizationSymbol[] memorizationSymbolsInfo;
    [Header("Syambolを生成する場所")]
    [SerializeField] private Transform symbolGeneratePoint;
    [Header("答えるためのボタン")]
    [SerializeField] private GameObject buttons;

    [Header("プレイするレベルの情報")]
    [SerializeField] private MemorizationGameLevel levelInfo;

    [Header("ゲームが始まるまでの待機時間")]
    [SerializeField] private float initialWaitTime = 1f;

    // 自動で初期化され、同じ添字が同じSymbolを指すようにする
    private GameObject[] memorizationSymbols;   // 生成したSymbolを入れる配列
    private string[] memorizationSymbolNames;   // 生成したSymbolの名前を入れる配列

    private MemorizationSymbol[] correctSymbols;  // 答えのSymbolを入れる配列

    private void Start()
    {
        InitializeSymbols();
        ButtonsInitialize();

        if (levelInfo == null)
        {
            Debug.LogError("レベルの情報が設定されていません。");
            return;
        }

        StartCoroutine(DisplaySymbol());
    }


    IEnumerator DisplaySymbol()
    {
        yield return new WaitForSeconds(initialWaitTime);  // 最初の待機時間

        int totalSymbols = levelInfo.DisplayOrder.Count;
        for (int i = 0; i < totalSymbols; i++)
        {
            int symbolIndex = GetSymbolIndex(levelInfo.DisplayOrder[i].name);
            if (symbolIndex != -1)
            {
                memorizationSymbols[symbolIndex].SetActive(true);  // Symbolを表示
                yield return new WaitForSeconds(levelInfo.DisplayTimePerSymbol);  // 次のSymbolを表示する前の待機時間
                memorizationSymbols[symbolIndex].SetActive(false); // Symbolを非表示
            }
            else
            {
                Debug.LogWarning($"Symbol '{levelInfo.DisplayOrder[i].name}' が見つかりませんでした。");
            }
        }

        Debug.Log("全てのSymbolの表示が完了しました。");

        yield return new WaitForSeconds(initialWaitTime);

        // ここでプレイヤーの入力を受け付ける処理を開始する
        buttons.SetActive(true);  // 答えるためのボタンを表示
    }


    /// <summary>
    /// Symbolを生成し、名前とPrefabを対応させる。全てのSymbolは非表示にする。
    /// </summary>
    void InitializeSymbols()
    {
        buttons.SetActive(false);  // 答えるためのボタンを非表示

        int symbolCount = memorizationSymbolsInfo.Length;

        memorizationSymbols = new GameObject[symbolCount];
        memorizationSymbolNames = new string[symbolCount];

        for (int i = 0; i < symbolCount; i++)
        {
            MemorizationSymbol symbol = memorizationSymbolsInfo[i];
            
            if (symbol == null)
            {
                Debug.LogWarning($"MemorizationSymbolInfoの添字{i}がnullです。");
                continue;
            }
            else
            {
                memorizationSymbols[i] = Instantiate(memorizationSymbolsInfo[i].SymbolPrefab, symbolGeneratePoint);
                memorizationSymbolNames[i] = memorizationSymbolsInfo[i].name;
                memorizationSymbols[i].SetActive(false);
            }
        }
    }


    /// <summary>
    /// 名前からSymbolの添字を返す。見つからない場合は-1を返す
    /// </summary>
    /// <param name="symbolName"></param>
    /// <returns></returns>
    int GetSymbolIndex(string symbolName)
    {
        for (int i = 0; i < memorizationSymbolNames.Length; i++)
        {
            if (memorizationSymbolNames[i] == symbolName)
            {
                return i;
            }
        }
        return -1;  // 見つからない場合は-1を返す
    }


    private int symbolCount; // プレイヤーが答えるべきSymbolの数
    private void ButtonsInitialize()
    {
        symbolCount = levelInfo.DisplayOrder.Count;
        correctSymbols = new MemorizationSymbol[symbolCount];
        for (int i = 0; i < symbolCount; i++)
        {
            correctSymbols[i] = levelInfo.DisplayOrder[i];
        }
    }

    private int currentAnswerIndex = 0;  // プレイヤーが現在答えるべきSymbolの添字
    public void ButtonCheck(MemorizationSymbol symbolInfo)
    {
        if(currentAnswerIndex < symbolCount)
        {
            if (symbolInfo == correctSymbols[currentAnswerIndex])
            {
                Debug.Log("正解");
                // 正解したときの処理

                currentAnswerIndex++;
            }
            else
            {
                Debug.Log("不正解");
                // 不正解のときの処理
            }

            if (currentAnswerIndex == symbolCount)
            {
                Debug.Log("全て正解しました");
                // 全て正解したときの処理
            }
        }
    }
}
