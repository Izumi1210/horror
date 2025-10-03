using UnityEngine;
using DG.Tweening;

public class ObjectAnimator : MonoBehaviour
{
    public Sequence clickSequence;
    public Sequence dialogueSequence;

    void Start()
    {
        clickSequence.Pause();
        dialogueSequence.Pause();
    }

    public void PlayClickAnimation()
    {
        clickSequence.Restart();
    }

    public void PlayDialogueAnimation()
    {
        dialogueSequence.Restart();
    }
}
