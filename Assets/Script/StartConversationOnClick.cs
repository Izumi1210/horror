using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StartConversationOnClick : MonoBehaviour
{
    public AudioSource audioSource;
    public string[] conversationNames;
    public AudioClip[] soundEffectsPerConversation;
    public StandardDialogueUI[] dialogueUIsPerConversation;

    public FlagData[] requiredFlagsPerConversation;  // 各会話に対応するフラグ

    private int clickCount = 0;
    private bool isWaiting = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnMouseDown()
    {
        if (DialogueManager.isConversationActive || isWaiting) return;

        int index = GetNextAvailableConversationIndex(clickCount);  // ← フラグに基づき次を決める
        if (index == -1) return; // すべてスキップされた

        // 効果音処理
        AudioClip clip = (soundEffectsPerConversation != null && index < soundEffectsPerConversation.Length)
            ? soundEffectsPerConversation[index]
            : null;

        if (clip != null && audioSource != null)
        {
            isWaiting = true;
            audioSource.PlayOneShot(clip);
            Invoke(nameof(PlayConversation), clip.length);
        }
        else
        {
            PlayConversation();
        }
    }

    void PlayConversation()
    {
        isWaiting = false;

        int index = GetNextAvailableConversationIndex(clickCount);  // ← 再確認
        if (index == -1) return;

        // UI を切り替える
        if (dialogueUIsPerConversation != null && index < dialogueUIsPerConversation.Length)
        {
            var selectedUI = dialogueUIsPerConversation[index];
            if (selectedUI != null)
            {
                DialogueManager.DisplaySettings.dialogueUI = selectedUI.gameObject;
            }
        }

        // 会話開始
        string conversationName = conversationNames[index];
        DialogueManager.StartConversation(conversationName);

        clickCount = index + 1; // ← スキップ分も考慮して次へ
    }

    /// <summary>
    /// 現在のclickCount以降で有効な会話indexを返す。なければ -1。
    /// </summary>
    int GetNextAvailableConversationIndex(int startIndex)
    {
        for (int i = startIndex; i < conversationNames.Length; i++)
        {
            if (requiredFlagsPerConversation == null || i >= requiredFlagsPerConversation.Length)
            {
                return i; // フラグ指定がないなら実行
            }

            var flag = requiredFlagsPerConversation[i];
            if (flag == null || flag.IsOn)
            {
                return i; // フラグがない or ON なら実行
            }
        }

        return -1; // すべてOFFまたは存在しない
    }
}
