using UnityEngine;
using DG.Tweening;

public class MoveOnClick : MonoBehaviour
{
    public Transform targetObject;   // �����������I�u�W�F�N�g
    public Vector3 targetPosition;   // �ړ���̍��W
    public float duration = 1f;      // �ړ��ɂ����鎞��
    public FlagData requiredFlag;

    private void OnMouseDown()
    {
        if (targetObject != null && (requiredFlag == null || requiredFlag.IsOn))
        {
            targetObject.DOMove(targetPosition, duration);
        }
    }
}
