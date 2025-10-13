using UnityEngine;
using UnityEngine.Playables;
using PixelCrushers.DialogueSystem.SequencerCommands;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Dialogue System のシーケンサーコマンド:
/// 使用例: Timeline(CameraRig, MyTimeline)
/// </summary>
public class SequencerCommandTimeline : SequencerCommand
{
    void Start()
    {
        // 引数を取得
        string targetName = GetParameter(0);  // 例: CameraRig
        string timelineName = GetParameter(1); // 例: MyTimeline

        // ターゲットオブジェクトを探す
        GameObject target = GameObject.Find(targetName);
        if (target == null)
        {
            Debug.LogWarning($"[SequencerCommandTimeline] '{targetName}' が見つかりません。");
            Stop();
            return;
        }

        // Timelineを取得
        PlayableDirector director = target.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogWarning($"[SequencerCommandTimeline] {targetName} に PlayableDirector がありません。");
            Stop();
            return;
        }

        // Timeline Assetを取得して再生
        if (!string.IsNullOrEmpty(timelineName))
        {
            var timelineAsset = Resources.Load<PlayableAsset>(timelineName);
            if (timelineAsset != null)
            {
                director.playableAsset = timelineAsset;
            }
            else
            {
                Debug.LogWarning($"[SequencerCommandTimeline] Timeline '{timelineName}' が Resources に見つかりません。");
            }
        }

        director.Play();

        // 即終了（このコマンドは待たない）
        Stop();
    }
}
