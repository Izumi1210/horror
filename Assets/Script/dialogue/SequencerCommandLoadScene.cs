using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandLoadScene : SequencerCommand
{
    void Start()
    {
        string sceneName = GetParameter(0);
        SceneManager.LoadScene(sceneName);
        Stop();
    }
}
