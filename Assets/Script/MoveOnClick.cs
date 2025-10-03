using UnityEngine;
using DG.Tweening;

public class MoveOnClick : MonoBehaviour
{
    public Transform targetObject;   // 動かしたいオブジェクト
    public Vector3 targetPosition;   // 移動先の座標
    public float duration = 1f;      // 移動にかかる時間
    public FlagData requiredFlag;

    private void OnMouseDown()
    {
        if (targetObject != null && (requiredFlag == null || requiredFlag.IsOn))
        {
            targetObject.DOMove(targetPosition, duration);
        }
    }
}
