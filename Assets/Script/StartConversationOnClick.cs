using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StartConversationOnClick : MonoBehaviour
{
    public AudioSource audioSource;                             // ���ʉ��Đ��p AudioSource
    public string[] conversationNames;                          // �Đ������b��
    public AudioClip[] soundEffectsPerConversation;             // �e��b�ɑΉ�������ʉ�
    public StandardDialogueUI[] dialogueUIsPerConversation;     // �e��b�ɑΉ�����UI

    // ���� �ύX�ӏ� ����
    public FlagData[] requiredFlagsPerConversation;             // �e��b�ɑΉ�����t���O
    // ���� �ύX�ӏ� ����

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

        // ���� �ύX�ӏ� ���� �t���O�`�F�b�N��ǉ�
        if (requiredFlagsPerConversation != null && index < requiredFlagsPerConversation.Length)
        {
            var flag = requiredFlagsPerConversation[index];
            if (flag != null && !flag.IsOn)
            {
                return; // �t���O��OFF�Ȃ��b�J�n���Ȃ�
            }
        }
        // ���� �ύX�ӏ� ����

        // ���ʉ�����
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

        // ���� �ύX�ӏ� ���� �t���O�`�F�b�N��ǉ�
        if (requiredFlagsPerConversation != null && index < requiredFlagsPerConversation.Length)
        {
            var flag = requiredFlagsPerConversation[index];
            if (flag != null && !flag.IsOn)
            {
                return; // �t���O��OFF�Ȃ��b�J�n���Ȃ�
            }
        }
        // ���� �ύX�ӏ� ����

        // UI ��؂�ւ���
        if (dialogueUIsPerConversation != null && index < dialogueUIsPerConversation.Length)
        {
            var selectedUI = dialogueUIsPerConversation[index];
            if (selectedUI != null)
            {
                DialogueManager.DisplaySettings.dialogueUI = selectedUI.gameObject;
            }
        }

        // ��b�J�n
        string conversationName = conversationNames[index];
        DialogueManager.StartConversation(conversationName);

        clickCount++;
    }
}
