using DG.Tweening;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using UnityEngine;

public class SequencerCommandScaleTween : SequencerCommand
{
    void Start()
    {
        string targetName = GetParameter(0);
        float scale = GetParameterAsFloat(1);
        float duration = GetParameterAsFloat(2, 1.0f);

        GameObject obj = GameObject.Find(targetName);
        if (obj == null) { Stop(); return; }

        obj.transform.DOScale(Vector3.one * scale, duration);
        Stop();
    }
}
