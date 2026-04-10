using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorizationGameController : MonoBehaviour
{
    [Header("全てのMemorizationSymbolを入れる")]
    [SerializeField] private MemorizationSymbol[] memorizationSymbolsInfo;
    [Header("Syambolを生成する場所")]
    [SerializeField] private Transform symbolGeneratePoint;

    [Header("プレイするレベルの情報")]
    [SerializeField] private MemorizationGameLevel levelInfo;

    [Header("ゲームが始まるまでの待機時間")]
    [SerializeField] private float initialWaitTime = 1f;

    // 自動で初期化され、同じ添字が同じSymbolを指すようにする
    private GameObject[] memorizationSymbols;   // 生成したSymbolを入れる配列
    private string[] memorizationSymbolNames;   // 生成したSymbolの名前を入れる配列

    private void Start()
    {
        InitializeSymbols();

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
    }


    /// <summary>
    /// Symbolを生成し、名前とPrefabを対応させる。全てのSymbolは非表示にする。
    /// </summary>
    void InitializeSymbols()
    {
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
}
