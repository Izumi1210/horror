using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandStopSEManual : SequencerCommand
{
    void Start()
    {
        SequencerCommandPlaySE.StopSE();
        Stop();
    }
}
