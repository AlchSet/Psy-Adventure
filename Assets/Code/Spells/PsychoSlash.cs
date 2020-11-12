using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoSlash : MonoBehaviour
{


    public bool flipEachTurn = true;

    Animator anim;
    Controller2D controller;
    SpriteRenderer r;

    bool flip;

    public Hitbox[] hitboxes;

    AudioSource sfx;


    bool isReady = true;

    float elapsed;


    public Player.PlayerAction onStartWpn;
    public Player.PlayerAction onEndWpn;


    Vector2 originalPos;

    public Vector2 upOffset;
    public Vector2 downOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;

    public float fireDelay = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = transform.root.GetComponent<Controller2D>();
        r = GetComponentInChildren<SpriteRenderer>();
        sfx = GetComponent<AudioSource>();

        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= fireDelay)
            {
                isReady = true;
            }
        }
    }


    public void Fire()
    {

        if (isReady)
        {
            isReady = false;
            elapsed = 0;
            //StartCoroutine(ExecuteWPN());
            sfx.PlayOneShot(sfx.clip);

            transform.localPosition = originalPos;

            switch (controller.faceDir)
            {
                case Controller2D.Direction.North:

                    
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    transform.localPosition = (Vector2)transform.localPosition + upOffset;
                    //g.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);


                    break;
                case Controller2D.Direction.South:
                    
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.localPosition = (Vector2)transform.localPosition + downOffset;
                    //g.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 7, ForceMode2D.Impulse);
                    break;

                case Controller2D.Direction.East:
                    
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    transform.localPosition = (Vector2)transform.localPosition + rightOffset;
                    //g.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 7, ForceMode2D.Impulse);
                    break;

                case Controller2D.Direction.West:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    transform.localPosition = (Vector2)transform.localPosition + leftOffset;
                    //g.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 7, ForceMode2D.Impulse);
                    break;






            }


            if(flipEachTurn)
            {
                if (flip)
                {
                    Vector3 t = transform.localScale;
                    t.x = -t.x;
                    transform.localScale = t;
                    //r.flipX = true;
                }
                else
                {
                    Vector3 t = transform.localScale;
                    t.x = Mathf.Abs(t.x);
                    transform.localScale = t;
                    //r.flipX = false;
                }

                flip = !flip;
            }
      


            anim.Play("PsychoSlash", 0, 0);


        }

    }

    IEnumerator ExecuteWPN()
    {
        yield return new WaitForSeconds(0.15f);
        isReady = true;
    }


    public void ActivateHitBoxes()
    {
        foreach (Hitbox h in hitboxes)
        {
            h.isHitboxOn = true;
        }
    }

    public void DeActivateHitBoxes()
    {
        foreach (Hitbox h in hitboxes)
        {
            h.isHitboxOn = false;
        }
    }


    public void WeaponStart()
    {
        if (onStartWpn != null)
            onStartWpn.Invoke();
    }

    public void WeaponEnd()
    {
        if (onEndWpn != null)
            onEndWpn.Invoke();
    }



}
