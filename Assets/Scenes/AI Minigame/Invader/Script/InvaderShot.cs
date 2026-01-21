using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderShot : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }
    void OnTriggerEnter2D()
    {
        Destroy(this.gameObject);
    }
}
