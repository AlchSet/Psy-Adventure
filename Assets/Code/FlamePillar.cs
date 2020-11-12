using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillar : MonoBehaviour
{

    bool detectPlayer;
    bool b;
    float elapsed;
    Animator anim;

    public GameObject projectile;

    Transform firepoint;

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        firepoint = transform.Find("FirePoint");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(detectPlayer)
        {
            if(!b)
            {
                anim.SetTrigger("Fire");
                b = true;
            }

            elapsed += Time.deltaTime;

            if(elapsed>=2)
            {
                b = false;
                elapsed = 0;
                
            }


        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            detectPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            detectPlayer = false;
        }
    }


    public void Fire()
    {
        GameObject g = Instantiate(projectile, firepoint.position, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().AddForce((player.position-transform.position).normalized * 10, ForceMode2D.Impulse);
    }
}
