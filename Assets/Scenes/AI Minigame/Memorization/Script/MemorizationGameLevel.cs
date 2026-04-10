using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevel", menuName = "AIMiniGame/MemorizationGameLevel")]
public class MemorizationGameLevel : ScriptableObject
{
    [Header("表示する順番")]
    [SerializeField] private MemorizationSymbol[] displayOrder;
    // IReadOnlyListとして公開することで、外部からは「読み取り」と「Count（数）」の取得しかできなくなる　by Gemini
    public IReadOnlyList<MemorizationSymbol> DisplayOrder => displayOrder;

    [Header("一つのSymbolを表示する時間")]
    [SerializeField] private float displayTimePerSymbol = 1f;
    public float DisplayTimePerSymbol => displayTimePerSymbol;

}
