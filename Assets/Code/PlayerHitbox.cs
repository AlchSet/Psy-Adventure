using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{


    public Damageable damageable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Enemy"))
        {

            Damageable.CollisionInfo info = new Damageable.CollisionInfo();

            info.totalDmg = 1;
            info.knockbackForce = 10;
            info.position =  collision.transform.root.position;
            //info.position=transform.position-
            damageable.DealDamage(info);

            //damageable.DealDamage(1);
            Debug.Log("ENEMY HURTS ME");
        }
    }
}
