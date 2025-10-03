using UnityEngine;
using PixelCrushers.DialogueSystem;

public class FlagConditionDialogueTrigger : MonoBehaviour
{
    [Header("�Q�Ƃ���t���O")]
    public FlagData flag;

    [Header("���ۂ�Dialogue System Trigger")]
    public DialogueSystemTrigger dialogueTrigger;

    private void Awake()
    {
        if (dialogueTrigger == null)
        {
            dialogueTrigger = GetComponent<DialogueSystemTrigger>();
        }
    }

    private void OnEnable()
    {
        // �t���O�� false �Ȃ�g���K�[�𖳌���
        if (flag != null && !flag.IsOn)
        {
            dialogueTrigger.enabled = false;
        }
        else
        {
            dialogueTrigger.enabled = true;
        }
    }
}
