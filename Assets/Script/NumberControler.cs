using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextMatchSceneControllerTMP : MonoBehaviour
{
    [SerializeField] private TMP_Text text1;
    [SerializeField] private TMP_Text text2;
    [SerializeField] private TMP_Text text3;
    [SerializeField] private string correct1 = "1";
    [SerializeField] private string correct2 = "2";
    [SerializeField] private string correct3 = "3";
    [SerializeField] private string nextSceneName = "NextScene";

    private bool isCleared = false;

    void Update()
    {
        if (isCleared) return; // 1回だけ判定

        // 現在の内容をログ出力して確認
        Debug.Log($"現在: {text1.text}, {text2.text}, {text3.text}");

        // 正解判定
        if (text1.text == correct1 && text2.text == correct2 && text3.text == correct3)
        {
            Debug.Log("✅ 正解！シーン遷移します");
            isCleared = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
