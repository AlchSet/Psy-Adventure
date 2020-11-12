using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public bool isHitboxOn;
    public float range = 0.25f;

    public UnityEvent OnHitTarget;


    Transform owner;

    public bool giveOwnerPos;
    // Start is called before the first frame update
    void Start()
    {
        owner = transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHitboxOn)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, 1 << LayerMask.NameToLayer("Enemy")| 1<<LayerMask.NameToLayer("Switch")|1<<LayerMask.NameToLayer("Breakable"));

            //if(hits.Length>0)
            //{
            //    OnHitTarget.Invoke();
            //}
            foreach(Collider2D hit in hits)
            {
                if(!hit.GetComponent<Damageable>().isHitStun)
                {
                    OnHitTarget.Invoke();
                }
                //hit.GetComponent<Damageable>().DealDamage(1);

                //info.totalDmg = 1;
                //info.knockbackForce = 10;
                //info.position = collision.transform.root.position;
                ////info.position=transform.position-
                hit.GetComponent<Damageable>().DealDamage(1,10,giveOwnerPos?owner.position:transform.position);
            }

        }

    }

    private void OnDrawGizmos()
    {
        if (isHitboxOn)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.gray;
        }
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
