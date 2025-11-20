using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlagResetterVerbose : MonoBehaviour
{
    [SerializeField] private FlagList flagList;   // Inspectorで設定
    [SerializeField] private bool resetOnSceneLoad = true;
    [SerializeField] private bool alsoResetOnStart = true;

    private void OnEnable()
    {
        if (resetOnSceneLoad)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDisable()
    {
        if (resetOnSceneLoad)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        if (alsoResetOnStart)
        {
            // Start 時点のアクティブシーンがこのオブジェクトの属するシーンならリセット
            if (gameObject.scene == SceneManager.GetActiveScene())
            {
                ResetFlagsWithLogs("Start");
            }
            else
            {
                Debug.Log($"[SceneFlagResetterVerbose] Start: object is in scene '{gameObject.scene.name}', activeScene is '{SceneManager.GetActiveScene().name}'");
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // sceneLoaded イベントで来た scene が、このオブジェクトのシーンと一致するならリセット
        // （Additive ロードや DontDestroyOnLoad の場合でも安全に判定）
        if (scene == gameObject.scene)
        {
            ResetFlagsWithLogs($"OnSceneLoaded ({scene.name})");
        }
        else
        {
            Debug.Log($"[SceneFlagResetterVerbose] OnSceneLoaded: loaded='{scene.name}' but this object is in '{gameObject.scene.name}'");
        }
    }

    private void ResetFlagsWithLogs(string reason)
    {
        if (flagList == null)
        {
            Debug.LogWarning("[SceneFlagResetterVerbose] FlagList is not assigned in inspector.");
            return;
        }

        var flags = flagList.Flags;
        if (flags == null || flags.Count == 0)
        {
            Debug.Log($"[SceneFlagResetterVerbose] {reason}: FlagList has no flags.");
            return;
        }

        Debug.Log($"[SceneFlagResetterVerbose] {reason}: Resetting {flags.Count} flags from FlagList '{flagList.name}'.");

        for (int i = 0; i < flags.Count; i++)
        {
            var f = flags[i];
            if (f == null)
            {
                Debug.Log($"  [{i}] null entry in list");
                continue;
            }

            // 名前と現在値を出力（ScriptableObject.name を利用）
            Debug.Log($"  [{i}] '{f.name}' before: {f.IsOn}");

            // 直接リセット（FlagData の public メソッドを使う）
            f.SetFlagStatus(false);

            Debug.Log($"  [{i}] '{f.name}' after: {f.IsOn}");
        }

        Debug.Log($"[SceneFlagResetterVerbose] {reason}: Reset complete.");
    }
}
