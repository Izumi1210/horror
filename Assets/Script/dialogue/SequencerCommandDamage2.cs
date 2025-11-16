using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandDamage2 : SequencerCommand
{
    void Start()
    {
        var dmg = GameObject.FindObjectOfType<Damage2>();
        if (dmg != null)
        {
            dmg.ShowDamage2();  // 赤表示実行
        }
        else
        {
            Debug.LogWarning("Damage2 script not found in scene.");
        }

        Stop(); // このシーケンスコマンドを終了
    }
}
