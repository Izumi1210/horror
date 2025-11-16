using PixelCrushers.DialogueSystem.SequencerCommands;
using UnityEngine;

public class SequencerCommandDamage : SequencerCommand
{
    void Start()
    {
        var dmg = GameObject.FindObjectOfType<Damage>();
        if (dmg != null)
        {
            dmg.ShowDamage();  // Å© Ç±Ç±Ç≈åƒÇ‘ÅI
        }
        Stop();
    }
}
