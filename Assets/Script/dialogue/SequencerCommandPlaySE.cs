using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandPlaySE : SequencerCommand
{
    private static AudioSource seSource; // 共通AudioSource

    void Start()
    {
        string clipName = GetParameter(0);
        AudioClip clip = Resources.Load<AudioClip>(clipName);

        if (clip != null)
        {
            if (seSource == null)
            {
                GameObject obj = new GameObject("SEPlayer");
                seSource = obj.AddComponent<AudioSource>();
                seSource.playOnAwake = false;
                Object.DontDestroyOnLoad(obj);
            }

            seSource.clip = clip;
            seSource.loop = false;
            seSource.Play();
        }

        Stop(); // このコマンド自体は即終了
    }

    public static void StopSE()
    {
        if (seSource != null && seSource.isPlaying)
        {
            seSource.Stop();
        }
    }
}
