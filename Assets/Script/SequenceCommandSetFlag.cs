using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using System.Security.Cryptography;

public class SequencerCommandSetFlag : SequencerCommand
{
    public void Start()
    {
        // Sequence ����n���ꂽ�������擾
        string flagName = GetParameter(0);
        bool value = true;
        if (parameters.Length > 1)
        {
            bool.TryParse(GetParameter(1), out value);
        }

        // FlagList �� FlagManager �o�R�Ŏ擾
        FlagList flagList = FlagManager.Instance.FlagList;
        if (flagList == null)
        {
            Debug.LogWarning("FlagList���Q�Ƃ���Ă��܂���");
            Stop();
            return;
        }

        // FlagList �̒�����Ώۃt���O��T���ăZ�b�g
        foreach (var f in flagList.Flags)
        {
            if (f.name == flagName)
            {
                f.SetFlagStatus(value);
                Debug.Log($"�t���O {flagName} �� {value} �ɂ��܂���");
                break;
            }
        }

        Stop(); // �I��
    }
}
