using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandMove : SequencerCommand
{
    void Start()
    {
        // 引数を取得
        string objectName = GetParameter(0);
        float x = GetParameterAsFloat(1);
        float y = GetParameterAsFloat(2);
        float z = GetParameterAsFloat(3);
        float duration = GetParameterAsFloat(4);

        // オブジェクトを探す
        GameObject target = GameObject.Find(objectName);
        if (target != null)
        {
            target.transform.DOMove(new Vector3(x, y, z), duration);
        }

        Stop(); // コマンド終了
    }
}
