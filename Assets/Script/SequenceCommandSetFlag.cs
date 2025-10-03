using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using System.Security.Cryptography;

public class SequencerCommandSetFlag : SequencerCommand
{
    public void Start()
    {
        // Sequence から渡された引数を取得
        string flagName = GetParameter(0);
        bool value = true;
        if (parameters.Length > 1)
        {
            bool.TryParse(GetParameter(1), out value);
        }

        // FlagList を FlagManager 経由で取得
        FlagList flagList = FlagManager.Instance.FlagList;
        if (flagList == null)
        {
            Debug.LogWarning("FlagListが参照されていません");
            Stop();
            return;
        }

        // FlagList の中から対象フラグを探してセット
        foreach (var f in flagList.Flags)
        {
            if (f.name == flagName)
            {
                f.SetFlagStatus(value);
                Debug.Log($"フラグ {flagName} を {value} にしました");
                break;
            }
        }

        Stop(); // 終了
    }
}
