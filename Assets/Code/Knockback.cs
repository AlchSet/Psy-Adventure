using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    Rigidbody2D body;
    public Damageable damageable;
    public float knockbackResist = 0;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void KnockBack()
    {
        if (!damageable.usePhysics)
            return;
        body.velocity = Vector2.zero;

        Vector2 dir = (Vector2)transform.position - (Vector2)damageable.info.position;
        //body.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        body.AddForce(dir.normalized * (damageable.info.knockbackForce - knockbackResist), ForceMode2D.Impulse);
    }

}
