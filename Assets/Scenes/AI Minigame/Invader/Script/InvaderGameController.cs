using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGameController : MonoBehaviour
{
    public static InvaderGameController instance;

    [SerializeField] Transform enemyLeftLimit;
    [SerializeField] Transform enemyRightLimit;
    [HideInInspector] public float leftLimitX;
    [HideInInspector] public float rightLimitX;

    private void Awake()
    {
        // Singletonƒpƒ^[ƒ“
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        // Enemy‚ÌˆÚ“®”ÍˆÍ‚ÌÝ’è
        leftLimitX = enemyLeftLimit.position.x;
        rightLimitX = enemyRightLimit.position.x;
        if(leftLimitX > rightLimitX)
            Debug.LogAssertion("leftLimit‚ÆrightLimit‚ª‹t‚É‚È‚Á‚Ä‚¢‚Ü‚·");
    }
}
