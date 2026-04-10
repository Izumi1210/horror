using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewSymbol", menuName = "AIMiniGame/MemorizationSymbol")]
public class MemorizationSymbol : ScriptableObject
{
    // Symbolの名前はScriptableObjectの名前を使う

    [Header("表示するオブジェクト")]
    [SerializeField] private GameObject symbolPrefab;
    public GameObject SymbolPrefab { get => symbolPrefab; }
}
