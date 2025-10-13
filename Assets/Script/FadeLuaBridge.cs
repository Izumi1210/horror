using PixelCrushers.DialogueSystem;
using UnityEngine;

public class FadeLuaBridge : MonoBehaviour
{
    void Start()
    {
        Lua.RegisterFunction("FadeLoadScene", this, SymbolExtensions.GetMethodInfo(() => FadeLoadSceneLua("", 1.0f)));
    }

    public void FadeLoadSceneLua(string sceneName, float duration)
    {
        FadeManager.Instance.LoadScene(sceneName, duration);
    }
}
