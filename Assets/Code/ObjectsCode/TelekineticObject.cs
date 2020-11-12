using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TelekineticObject : MonoBehaviour
{
    public GameObject flame;
    public Transform teleObject;
    public GameObject shadow;

    Controller2D controller;

    public Player.PlayerAction action;

    bool ready;
    bool thrown;


    public float freq = 2;
    public float range = 0.01f;

    Rigidbody2D body;

    AudioSource sfx;
    public AudioClip tapSFX;

    public ParticleSystem particles;

    public ParticleSystem breakParticles;

    ParticleSystem.EmissionModule em;

    public bool canLevitate = true;
    public bool canThrow = true;
    public bool breakOnFall;


    bool inThrow;


    public UnityEvent OnMoving;
    public UnityEvent OnStopMoving;

    Collider2D col;
    SpriteRenderer sprite;

    PhysicsMaterial2D mat1;
    PhysicsMaterial2D mat2;

    bool isBroken;


    Player p;

    // Start is called before the first frame update
    void Awake()
    {
        p = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
        //flame.SetActive(false);
        controller = GetComponent<Controller2D>();
        shadow.SetActive(false);

        body = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();

        em = particles.emission;
        em.enabled = false;

        col = GetComponent<Collider2D>();


        mat1 = new PhysicsMaterial2D("NormalMat");
        mat2 = new PhysicsMaterial2D("BounceMat");

        col.sharedMaterial = mat1;
        mat2.bounciness = 0.4f;

        sprite = teleObject.GetComponent<SpriteRenderer>();
        //col.sharedMaterial.bounciness = 0;
        //Debug.Log(em.enabled);
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 d = transform.position - p.transform.position;
        d.Normalize();
        p.UpdateDirection(d.x,d.y);


        if (!thrown)
        {
            controller.SetInputVel(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

            if (controller.isReallyMoving)
            {
                Debug.Log("MOVING");
                OnMoving.Invoke();
            }
            else
            {
                Debug.Log("NOT MOVING");
                OnStopMoving.Invoke();
            }



            if (Input.GetKeyDown(KeyCode.X) && ready)
            {
                controller.SetInputVel(Vector2.zero);
                //flame.SetActive(false);
                em.enabled = false;
                StartCoroutine(Fall());
                this.enabled = false;
            }


            if (Input.GetKeyDown(KeyCode.C) && ready && canThrow)
            {
                Debug.Log("THROW");
                thrown = true;
                controller.enabled = false;
                body.AddForce(controller.LastInputVel * 6, ForceMode2D.Impulse);
                this.enabled = false;
                //flame.SetActive(false);
                em.enabled = false;
                StartCoroutine(Throw());
            }

            if (ready)
            {
                Vector2 hover = teleObject.localPosition;
                //hover.y = hover.y + (Mathf.Cos(Time.time*2f)*0.01f);
                hover.y = hover.y + (Mathf.Cos(Time.time * freq) * range);
                teleObject.localPosition = hover;

            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("TELE ACTIVe");

        //flame.SetActive(true);
        em.enabled = true;
        controller.enabled = true;
        thrown = false;
        //StopCoroutine(Throw());
        StopAllCoroutines();
        body.velocity = Vector2.zero;
        body.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Levitate());
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (inThrow && !isBroken)
        {
            if (collision.tag == "Glass")
            {
                Debug.Log("HIT GLASS");
                collision.GetComponent<Damageable>().DealDamage(1, 0, transform.position);
                isBroken = true;
                StopAllCoroutines();
                StartCoroutine(BreakObject());
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inThrow && !isBroken)
        {


            isBroken = true;
            StopAllCoroutines();
            StartCoroutine(BreakObject());
            //Destroy(gameObject);
        }
    }

    IEnumerator Levitate()
    {
        sfx.Play();
        if (canLevitate)
        {

            shadow.SetActive(true);
            while (true)
            {
                teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 1), Time.deltaTime * 2);

                if (teleObject.localPosition.y >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }


        ready = true;
    }

    IEnumerator Fall()
    {
        sfx.Stop();

        if (canLevitate)
        {
            while (true)
            {
                teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2);

                if (teleObject.localPosition.y <= 0)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
            shadow.SetActive(false);

        }

        action();
        ready = false;
        body.bodyType = RigidbodyType2D.Kinematic;
    }

    IEnumerator Throw()
    {
        sfx.Stop();
        bool arc = false;
        bool bounce = false;
        bool end = false;
        action();
        inThrow = true;
        //teleObject.localPosition = new Vector2(0, 1f);
        while (true)
        {

            if (!arc)
            {
                teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 1.25f), Time.deltaTime * 1f);
                if (teleObject.localPosition.y >= 1.25f)
                {
                    arc = true;
                }

            }
            else
            {
                if (!bounce)
                {
                    teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2.5f);

                    if (teleObject.localPosition.y <= 0)
                    {
                        bounce = true;
                        sfx.PlayOneShot(tapSFX);
                        if (breakOnFall)
                        {
                            isBroken = true;
                            StartCoroutine(BreakObject());
                            //Destroy(gameObject);
                            break;
                        }

                    }
                }
                else
                {

                    if (!end)
                    {
                        teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0.2f), Time.deltaTime * 2f);
                        if (teleObject.localPosition.y >= 0.2f)
                        {
                            end = true;
                        }
                    }
                    else
                    {
                        teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2f);

                        if (teleObject.localPosition.y <= 0)
                        {
                            sfx.PlayOneShot(tapSFX);
                            break;
                        }

                    }


                }

            }



            yield return new WaitForEndOfFrame();
        }
        shadow.SetActive(false);
        controller.enabled = true;
        ready = false;

        thrown = false;
        controller.SetInputVel(Vector2.zero);
        body.velocity = Vector2.zero;
        body.bodyType = RigidbodyType2D.Kinematic;
        inThrow = false;
    }

    IEnumerator BreakObject()
    {
        breakParticles.Play();
        sprite.enabled = false;
        col.enabled = false;
        body.bodyType = RigidbodyType2D.Kinematic;
        body.velocity = Vector2.zero;
        controller.SetInputVel(Vector2.zero);
        shadow.SetActive(false);
        sfx.PlayOneShot(tapSFX);
        yield return null;
    }

}
//Bug when tossed towards player and he immediatelly use telekinesis again
//Happens when the player unfreezes after throw
//Worked around it by setting controller is true on enable and thrown is false
//Also StopAllCoroutines on enable fixes possition error


//Made Telekinetic objects kinematic, they become dynamic when moved with telekinesis