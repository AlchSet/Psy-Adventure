using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychobeamProjectile : MonoBehaviour
{
    public Rigidbody2D body;
    ParticleSystem p;
    GameObject hitFX;
    Collider2D box;
    public Vector2 dir;
    GameObject selfCopy;

    bool hit;
    // Start is called before the first frame update
    void Awake()
    {
        box = GetComponent<Collider2D>();
        p = GetComponentInChildren<ParticleSystem>();
        body = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(0, 1, 1);
        hitFX = transform.Find("PsychoBeamHit").gameObject;
        hitFX.SetActive(false);
        StartCoroutine(Lifetime());
        selfCopy = gameObject;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}




    IEnumerator Lifetime()
    {
        Vector3 oScale = transform.localScale;
        while (true)
        {
            oScale.x = Mathf.MoveTowards(transform.localScale.x, 1, Time.deltaTime * 5);

            transform.localScale = oScale;

            if (transform.localScale.x >= 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(.8f);

        body.velocity = Vector2.zero;
        while (true)
        {
            oScale.x = Mathf.MoveTowards(transform.localScale.x, 0, Time.deltaTime * 5);

            transform.localScale = oScale;

            if (transform.localScale.x <= 0)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        p.Stop();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }


    IEnumerator End()
    {
        Vector3 oScale = transform.localScale;
        //body.velocity = Vector2.zero;
        while (true)
        {
            oScale.x = Mathf.MoveTowards(transform.localScale.x, 0, Time.deltaTime * 5);

            transform.localScale = oScale;

            if (transform.localScale.x <= 0)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        p.Stop();
        yield return new WaitForSeconds(1f);
        Destroy(hitFX);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);


        //Fix some bugs
        //Theres a bug if ray doesnt hit anything the projectile will appear at 0,0

        if(!hit)
        {
            if (collision.tag == "Glass")
            {



                RaycastHit2D h = Physics2D.Raycast(transform.position, transform.right, 2);

                Debug.Log("REFLECT=" + h.point);

                GameObject g = Instantiate(selfCopy, h.point, Quaternion.identity);

                Vector2 newVel = Vector2.Reflect(transform.right, h.normal);


                g.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(newVel.y, newVel.x) * Mathf.Rad2Deg);

                g.GetComponent<Rigidbody2D>().velocity = newVel * 8;



                Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), collision, true);
                //Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(),box,true);
                StopAllCoroutines();
                hitFX.transform.SetParent(null);
                hitFX.SetActive(true);
                StartCoroutine(End());
                hit = true;
                return;
            }




            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.GetComponent<Damageable>().DealDamage((1));

                StopAllCoroutines();
                Debug.Log("HIT");
                hitFX.transform.SetParent(null);
                hitFX.SetActive(true);
                StartCoroutine(End());
                hit = true;
            }


            if (!collision.isTrigger)
            {
                if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Bounds"))
                {
                    StopAllCoroutines();
                    Debug.Log("HIT");
                    hitFX.transform.SetParent(null);
                    hitFX.SetActive(true);
                    StartCoroutine(End());
                    hit = true;
                }

            }
            else
            {

            }
        }

      


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2);
    }
}
