using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagActivator : MonoBehaviour
{
    [SerializeField] private FlagData flag;
    [SerializeField] private GameObject targetObject;
    void Update()
    {
        if(flag != null && targetObject != null)
        {
            targetObject.SetActive(flag.IsOn);
        }
    }
}
