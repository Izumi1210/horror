using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;  // ← 追加（必須）

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
            Debug.Log("✅ 正解！会話を終了してシーン遷移します");
            isCleared = true;

            // 会話を安全に停止してからシーン遷移する
            StartCoroutine(EndConversationAndLoadScene());
        }
    }

    private System.Collections.IEnumerator EndConversationAndLoadScene()
    {
        // ① 会話を停止
        DialogueManager.StopConversation();

        // ② 内部状態が終了するまで 1フレーム待つ（必要）
        yield return null;

        // ③ シーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}
