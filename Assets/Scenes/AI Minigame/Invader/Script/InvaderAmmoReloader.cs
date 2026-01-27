using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderAmmoReloader : MonoBehaviour
{
    [SerializeField] float reloadTime;
    [SerializeField] int maxAmmoNum;
    [SerializeField] GameObject reloadAmmoPrefab;
    float elapsedTime;

    private void FixedUpdate()
    {
        // ゲーム進行中でなければリターン
        if (!InvaderGameController.instance.isInProgress)
            return;

        if (InvaderGameController.instance.AmmoList.Count > maxAmmoNum)
        {
            elapsedTime = 0f;
            return;
        }
        else
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime >= reloadTime)
            {
                elapsedTime = 0f;
                InvaderGameController.instance.AddAmmoIcon(reloadAmmoPrefab);
            }
        }
    }
}
