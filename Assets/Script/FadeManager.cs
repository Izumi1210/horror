using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : MonoBehaviour
{
    #region Singleton

    private static FadeManager instance;

    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(FadeManager) + " is not found");
                }
            }
            return instance;
        }
    }

    #endregion Singleton

    public bool DebugMode = true;
    private float fadeAlpha = 0;
    private bool isFading = false;
    public Color fadeColor = Color.black;

    // フェード完了時のイベント
    public event Action OnFadeComplete;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnGUI()
    {
        if (this.isFading)
        {
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }

        if (this.DebugMode)
        {
            if (!this.isFading)
            {
                List<string> scenes = new List<string>();
                scenes.Add("SampleScene");

                if (scenes.Count == 0)
                {
                    GUI.Box(new Rect(10, 10, 200, 50), "Fade Manager(Debug Mode)");
                    GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
                    return;
                }

                GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
                GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

                int i = 0;
                foreach (string sceneName in scenes)
                {
                    if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level"))
                    {
                        LoadScene(sceneName, 1.0f);
                    }
                    GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// シーン遷移を実行するメソッド.
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="interval">フェードイン・アウトにかかる時間</param>
    public void LoadScene(string scene, float interval)
    {
        StartCoroutine(TransScene(scene, interval));
    }

    /// <summary>
    /// シーン遷移用のコルーチン.
    /// </summary>
    private IEnumerator TransScene(string scene, float interval)
    {
        this.isFading = true;
        float time = 0;

        // フェードアウト
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        // シーンの切り替え
        SceneManager.LoadScene(scene);

        // フェードイン
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return null;
        }

        this.isFading = false;

        // フェード完了を通知
        OnFadeComplete?.Invoke();
    }
}

