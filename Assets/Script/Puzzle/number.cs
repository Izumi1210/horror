using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class numbers : MonoBehaviour
{
    [Header("Text (子オブジェクトの数字)")]
    public TextMeshProUGUI numberText; // ← インスペクターで子Textをドラッグ

    [Header("正解の値")]
    public int correctL = 1;
    public int correctM = 2;
    public int correctR = 3;

    [Header("自分がどのスロットか (1=L, 2=M, 3=R)")]
    public int me;

    [Header("クリア時にロードするシーン名")]
    public string nextSceneName;

    // 全インスタンスで共有する現在の値
    public static int hako_l;
    public static int hako_m;
    public static int hako_r;

    void Start()
    {
        // 初期値を表示
        if (numberText != null)
        {
            switch (me)
            {
                case 1: numberText.text = hako_l.ToString(); break;
                case 2: numberText.text = hako_m.ToString(); break;
                case 3: numberText.text = hako_r.ToString(); break;
            }
        }
        else
        {
            Debug.LogError($"numberTextが設定されていません ({gameObject.name})");
        }
    }

    public void clk()
    {
        if (numberText == null) return;

        int num = int.Parse(numberText.text);
        num++;
        if (num >= 10) num = 0;
        numberText.text = num.ToString();

        // 押されたスロットに応じて値を更新（static変数）
        switch (me)
        {
            case 1: hako_l = num; break;
            case 2: hako_m = num; break;
            case 3: hako_r = num; break;
        }

        // 共有変数の値を出力
        Debug.Log($"現在の値: L={hako_l}, M={hako_m}, R={hako_r}");

        // 正解判定
        if (hako_l == correctL && hako_m == correctM && hako_r == correctR)
        {
            Debug.Log("正解！シーンを切り替えます");
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("シーン名が設定されていません");
            }
        }
    }
}
