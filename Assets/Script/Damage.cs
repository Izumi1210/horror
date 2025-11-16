using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] Image DamageImg;

    void Start()
    {
        DamageImg.color = Color.clear;
    }

    void Update()
    {
        // 毎フレーム、徐々に透明に戻す
        DamageImg.color = Color.Lerp(DamageImg.color, Color.clear, Time.deltaTime);
    }

    // 外部から呼び出す用（例：Dialogue System から）
    public void ShowDamage()
    {
        DamageImg.color = new Color(0.7f, 0f, 0f, 0.7f); // 赤く点灯
    }
}
