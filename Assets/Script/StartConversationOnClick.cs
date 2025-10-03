using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StartConversationOnClick : MonoBehaviour
{
    public AudioSource audioSource;                             // 効果音再生用 AudioSource
    public string[] conversationNames;                          // 再生する会話名
    public AudioClip[] soundEffectsPerConversation;             // 各会話に対応する効果音
    public StandardDialogueUI[] dialogueUIsPerConversation;     // 各会話に対応するUI

    // ▼▼ 変更箇所 ▼▼
    public FlagData[] requiredFlagsPerConversation;             // 各会話に対応するフラグ
    // ▲▲ 変更箇所 ▲▲

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

        int index = Mathf.Min(clickCount, conversationNames.Length - 1);

        // ▼▼ 変更箇所 ▼▼ フラグチェックを追加
        if (requiredFlagsPerConversation != null && index < requiredFlagsPerConversation.Length)
        {
            var flag = requiredFlagsPerConversation[index];
            if (flag != null && !flag.IsOn)
            {
                return; // フラグがOFFなら会話開始しない
            }
        }
        // ▲▲ 変更箇所 ▲▲

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

        int index = Mathf.Min(clickCount, conversationNames.Length - 1);

        // ▼▼ 変更箇所 ▼▼ フラグチェックを追加
        if (requiredFlagsPerConversation != null && index < requiredFlagsPerConversation.Length)
        {
            var flag = requiredFlagsPerConversation[index];
            if (flag != null && !flag.IsOn)
            {
                return; // フラグがOFFなら会話開始しない
            }
        }
        // ▲▲ 変更箇所 ▲▲

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

        clickCount++;
    }
}
