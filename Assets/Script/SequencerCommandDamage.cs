using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

// Custom Sequencer Command: Damage()
public class SequencerCommandDa : SequencerCommand
{
    void Start()
    {
        // Damageスクリプトを持つオブジェクトを探す
        Damage dmg = GameObject.FindObjectOfType<Damage>();
        if (dmg != null)
        {
            // Damageを1回発動（クリック時と同じ挙動）
            dmg.SendMessage("Damaged", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.LogWarning("Damage script not found in scene.");
        }

        // 終了を通知
        Stop();
    }
}
