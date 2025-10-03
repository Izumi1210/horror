using UnityEngine;
using DG.Tweening;

public class FadeOnClick : MonoBehaviour
{
    public GameObject targetObject;
    public float duration = 1f;
    public FlagData requiredFlag;

    private void OnMouseDown()
    {
        if(targetObject != null && (requiredFlag == null || requiredFlag.IsOn))
        {
            var renderer = targetObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                renderer.material.DOFade(0f, duration);
            }
        }
    }
}