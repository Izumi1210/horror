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
    
    void OnTriggerEnter2D()
    {
        // ŠÈˆÕ“I‚ÈƒQ[ƒ€‚È‚Ì‚ÅAÕ“Ë‚µ‚½‚ç•K‚¸ƒvƒŒƒCƒ„[‚Ì’e‚Æ”»’è‚·‚é
        OnHitByPlayerBullet();
    }

    public void OnHitByPlayerBullet()
    {
        // “G‚ª’e‚É“–‚½‚Á‚½‚Æ‚«‚Ìˆ—
        Destroy(this.gameObject);
    }
}
