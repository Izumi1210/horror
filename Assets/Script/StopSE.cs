using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandStopSE : SequencerCommand
{
    void Start()
    {
        string clipName = GetParameter(0); // Sequence欄の引数
        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();

        foreach (var s in sources)
        {
            if (s.isPlaying && s.clip != null)
            {
                // 引数が空なら全部止める
                if (string.IsNullOrEmpty(clipName))
                {
                    s.Stop();
                }
                // クリップ名が一致するものだけ止める
                else if (s.clip.name == clipName)
                {
                    s.Stop();
                }
            }
        }

        Stop(); // このコマンド自体を終了
    }
}
