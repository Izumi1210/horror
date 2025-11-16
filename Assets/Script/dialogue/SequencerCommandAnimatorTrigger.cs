using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandAnimatorTrigger : SequencerCommand
{
    void Start()
    {
        Debug.Log("[CustomCommand] AnimatorTrigger START");

        string triggerName = GetParameter(0);

        Transform subject = GetSubject(0);

        Debug.Log("[CustomCommand] subject = " + subject);

        Animator animator = subject.GetComponentInChildren<Animator>();

        if (animator != null)
        {
            Debug.Log("[CustomCommand] SetTrigger: " + triggerName);
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogError("[CustomCommand] Animator not found on: " + subject.name);
        }

        Stop();
    }
}
