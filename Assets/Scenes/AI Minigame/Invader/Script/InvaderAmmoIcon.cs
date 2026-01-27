using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderAmmoIcon : MonoBehaviour
{
    [SerializeField] GameObject thisAmmoPrefab;

    public GameObject GetThisAmmo()
    {
        return thisAmmoPrefab;
    }
}
