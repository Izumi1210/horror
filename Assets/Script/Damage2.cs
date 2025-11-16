using UnityEngine;
using UnityEngine.UI;

public class Damage2 : MonoBehaviour
{
    [SerializeField] Image DamageImg;

    void Start()
    {
        DamageImg.color = Color.clear;
    }

    // 外部から呼び出し（赤く固定表示）
    public void ShowDamage2()
    {
        DamageImg.color = new Color(0.7f, 0f, 0f, 0.7f);
    }
}
