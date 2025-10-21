using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandSetTextSpeed : SequencerCommand
{
    void Start()
    {
        // 第一引数をfloatとして取得
        float newSpeed = GetParameterAsFloat(0);
        DialogueLua.SetVariable("_typewriterSpeed", newSpeed);

        Stop(); // コマンド終了
    }
}
