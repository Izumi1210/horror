using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderNormalEnemy : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float descendingWidth = 0.1f;
    bool isMovingToRight = true;

    private void FixedUpdate()
    {
        // ゲームが進行中でなければ自分自身を破壊
        if (!InvaderGameController.instance.isInProgress)
            DestroyThis();

        if (isMovingToRight)
            transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
        else
            transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);

        if (transform.position.x > InvaderGameController.instance.rightLimitX)
        {
            isMovingToRight = false;
            transform.position = new Vector3(InvaderGameController.instance.rightLimitX, transform.position.y - descendingWidth, transform.position.z);
        }
        else if (transform.position.x < InvaderGameController.instance.leftLimitX)
        {
            isMovingToRight = true;
            transform.position = new Vector3(InvaderGameController.instance.leftLimitX, transform.position.y - descendingWidth, transform.position.z);
        }
    }

    /// <summary>
    /// 破壊されるときの処理
    /// </summary>
    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D()
    {
        // 簡易的なゲームなので、衝突したら必ずプレイヤーの弾と判定する
        OnHitByPlayerBullet();
    }

    public void OnHitByPlayerBullet()
    {
        // 敵が弾に当たったときの処理
        DestroyThis();
    }
}
