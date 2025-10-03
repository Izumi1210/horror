using UnityEngine;
using PixelCrushers.DialogueSystem;

public class FlagConditionDialogueTrigger : MonoBehaviour
{
    [Header("参照するフラグ")]
    public FlagData flag;

    [Header("実際のDialogue System Trigger")]
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
        // フラグが false ならトリガーを無効化
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
