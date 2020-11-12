using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

namespace PsyMouse
{
    public class TelekinesisObject : MonoBehaviour
    {


        public GameObject shadow;
        public Transform teleObject;

        public Transform ThrowIndicator;

        public bool ready;
        public bool thrown;


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


        public bool inThrow;

        public UnityEvent OnBegin;
        public UnityEvent OnEnd;

        public UnityEvent OnMoving;
        public UnityEvent OnStopMoving;

        Collider2D col;
        SpriteRenderer sprite;

        PhysicsMaterial2D mat1;
        PhysicsMaterial2D mat2;

        bool isBroken;


        Player p;

        bool isReady = true;

        //PsychicMouse.SelectEvent endSelect;

        PsychicMouse mouse;

        public Vector2 offset;

        Vector2 lastPos;
        public Vector2 lastDir;

        public bool aim;

        public bool throwready;

        public float power = 7;

        public bool specialmode;

        PixelPerfectCamera pixcam;

        public float hoverHeight = 1;


        Vector2 oTeleObjectPos;


        //SpringJoint2D spring;
        void Awake()
        {
            pixcam = Camera.main.GetComponent<PixelPerfectCamera>();

            if (specialmode)
            {
                //spring = transform.Find("Spring").GetComponent<SpringJoint2D>();
                //spring.transform.SetParent(null);

            }
            shadow.SetActive(false);
            body = GetComponent<Rigidbody2D>();
            sfx = GetComponent<AudioSource>();
            col = GetComponent<Collider2D>();
            sprite = teleObject.GetComponent<SpriteRenderer>();
            em = particles.emission;
            mouse = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PsychicMouse>();
            lastPos = transform.position;


            oTeleObjectPos = teleObject.localPosition;

            if (ThrowIndicator)
            {
                ThrowIndicator.gameObject.SetActive(false);
            }
        }


        // Update is called once per frame
        void Update()
        {

            //Debug.Log(body.angularVelocity);
            //Debug.Log(body.velocity);
            //body.velocity = Vector2.zero;

            if (specialmode)
            {
                mouse.MoveSpring();
                //spring.transform.position = mouse.mousePos;
                //spring.GetComponent<Rigidbody2D>().MovePosition(mouse.mousePos);

                //spring.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(spring.transform.position, mouse.mousePos+offset, Time.deltaTime * power));
            }

            if (ready)
            {

                if (!specialmode)
                {
                    //shadow.transform.position = (Vector2)teleObject.position + Vector2.down;

                    //if (ready)
                    //teleObject.position = (Vector2)transform.position + Vector2.up;


                    if (!aim)
                    {
                        body.velocity = Vector2.zero;
                        //body.MovePosition(Vector2.MoveTowards(transform.position, mouse.mousePos + offset, Time.deltaTime * power));
                        body.MovePosition(Vector2.MoveTowards(transform.position, pixcam.RoundToPixel(mouse.mousePos + offset), Time.deltaTime * power));
                        //transform.position= Vector2.MoveTowards(transform.position, mouse.mousePos + offset, Time.deltaTime * 5);
                        Debug.DrawLine(transform.position, mouse.mousePos + offset, Color.red);

                        Vector2 dir = (Vector2)transform.position - lastPos;

                        if (dir != Vector2.zero)
                        {
                            lastDir = dir.normalized;
                        }

                        lastPos = transform.position;
                    }
                    else
                    {

                        lastDir = (mouse.mousePos - (Vector2)transform.position).normalized;
                        body.velocity = Vector2.zero;
                        if (ThrowIndicator)
                        {
                            float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;

                            ThrowIndicator.rotation = Quaternion.Euler(0, 0, angle);

                        }

                    }


                    Debug.DrawRay(transform.position, lastDir, Color.red);
                }
                else
                {

                    shadow.transform.position = (Vector2)teleObject.position + Vector2.down;

                    if (aim)
                    {
                        lastDir = (mouse.mousePos - (Vector2)transform.position).normalized;
                        body.velocity = Vector2.zero;
                        if (ThrowIndicator)
                        {
                            float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;

                            ThrowIndicator.rotation = Quaternion.Euler(0, 0, angle);

                        }
                    }
                }

            }

        }


        //private void LateUpdate()
        //{
        //    Vector2 npos = Camera.main.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().RoundToPixel(transform.position);
        //    transform.position = npos;
        //}
        void OnEnable()
        {
            BeginTelekinesis();
            OnBegin.Invoke();
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




        public void BeginTelekinesis()
        {

            //Debug.Log("LEVITATE");
            particles.Play();
            em.enabled = true;
            body.bodyType = RigidbodyType2D.Dynamic;
            StopAllCoroutines();
            StartCoroutine(Levitate());
        }

        public void EndTelekinesis()
        {

            //flame.SetActive(false);
            em.enabled = false;
            StartCoroutine(Fall());
            //OnEnd.Invoke();
            this.enabled = false;

        }

        public void BeginThrow()
        {
            if (ready && canThrow)
            {
                Debug.Log("THROW");
                thrown = true;
                //controller.enabled = false;

                //body.AddForce(controller.LastInputVel * 6, ForceMode2D.Impulse);
                body.AddForce(lastDir * 7, ForceMode2D.Impulse);

                this.enabled = false;
                //flame.SetActive(false);
                em.enabled = false;
                StartCoroutine(Throw());
                //endSelect();

                if (ThrowIndicator)
                {
                    ThrowIndicator.gameObject.SetActive(false);
                }

            }

        }

        public void ExitInteract()
        {
            if (ready)
            {

                mouse.DetachTelekinesis();
                //spring.enabled = false;
                aim = false;
                //endSelect();
                EndTelekinesis();
                if (ThrowIndicator)
                {
                    ThrowIndicator.gameObject.SetActive(false);
                }
            }

        }
        ///<summary>
        ///Interrupts and cancels telekinesis.
        ///</summary>
        public void Abort()
        {
            StopAllCoroutines();
            ready = true;
            ExitInteract();
            ThrowIndicator.gameObject.SetActive(false);
        }



        public void Interact()
        {
            if (this.enabled)
            {
                Debug.Log(lastPos + "/" + transform.position);
            }
            else
            {
                if (specialmode)
                {
                    mouse.AttachTelekinesis(transform, body);
                    //spring.transform.position = mouse.mousePos;
                    //spring.anchor = Vector2.zero;
                    //spring.connectedAnchor = transform.InverseTransformPoint(mouse.mousePos);
                    //spring.connectedBody = body;
                    //spring.enabled = true;


                }
                this.enabled = true;
            }

        }

        //public void Select(PsychicMouse.SelectEvent e)
        //{
        //    endSelect = e;
        //}

        public bool GetIsReady()
        {
            return isReady;
        }


        IEnumerator Levitate()
        {
            float ti = 0;
            sfx.Play();
            Vector2 t = (Vector2)transform.position + Vector2.up;
            if (canLevitate)
            {

                shadow.SetActive(true);
                while (true)
                {
                    //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 1), Time.deltaTime * 2);

                    //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)shadow.transform.position + (Vector2.up*hoverHeight), Time.deltaTime * 2);



                    teleObject.localPosition = Vector2.Lerp(oTeleObjectPos, oTeleObjectPos + Vector2.up, ti);

                    ti += Time.deltaTime;


                    if (ti >= 1)
                    {
                        break;
                    }


                    //float d = Vector2.Distance(teleObject.position, (Vector2)shadow.transform.position + (Vector2.up*hoverHeight));


                    //Debug.Log(d);
                    //if (teleObject.localPosition.y >= 1)
                    //if (d <= 0.01f)
                    //{
                    //    Debug.Log("FIN");
                    //    break;
                    //}
                    yield return new WaitForEndOfFrame();
                }
            }


            ready = true;
        }

        IEnumerator Fall()
        {
            sfx.Stop();
            float ti = 0;
            //Vector2 pos = teleObject.localPosition;
            if (canLevitate)
            {
                while (true)
                {

                    //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2);
                    //teleObject.position = Vector2.MoveTowards(teleObject.position,shadow.transform.position, Time.deltaTime * 2);


                    teleObject.localPosition = Vector2.Lerp(oTeleObjectPos + Vector2.up, oTeleObjectPos, ti);

                    ti += Time.deltaTime;


                    if (ti >= 1)
                    {
                        break;
                    }


                    //float d = Vector2.Distance(teleObject.position , shadow.transform.position);
                    //if (teleObject.localPosition.y <= 0)
                    //if (teleObject.localPosition.sqrMagnitude == 0)
                    //if(d<=0)
                    //{
                    //    Debug.Log("DONE");
                    //    break;
                    //}

                    yield return new WaitForEndOfFrame();
                }
                shadow.SetActive(false);

            }

            //action();
            //Vector2 cachepos = shadow.transform.position;
            //shadow.transform.localPosition = Vector2.zero;
            //teleObject.localPosition = Vector2.zero;
            //transform.position = cachepos;


            ready = false;
            throwready = false;
            thrown = false;
            OnEnd.Invoke();
            //body.bodyType = RigidbodyType2D.Kinematic;
        }

        IEnumerator Throw()
        {
            sfx.Stop();
            bool arc = false;
            bool bounce = false;
            bool end = false;
            //action();
            inThrow = true;

            float dist = Vector2.Distance(transform.position, teleObject.position);

            //Debug.Log(dist);
            float sec = 0;


            float coef = 0.0011f;

            //Vector2 c = (Vector2)teleObject.localPosition+Vector2.up*0.1f;

            Vector2 pos1 = teleObject.localPosition;
            Vector2 pos2 = (Vector2)teleObject.localPosition + Vector2.up * .25f;
            Vector2 pos3 = oTeleObjectPos;
            Vector2 pos4 = oTeleObjectPos + Vector2.up * 0.55f;

            float x1= 3.5f;
            float x2= 3.5f;
            float x3=4;
            float x4=4;


            float ti = 0;
            //teleObject.localPosition = new Vector2(0, 1f);
            while (true)
            {

                if (!arc)
                {
                    //Debug.DrawLine(teleObject.position,teleObject.TransformPoint(c), Color.blue);

                    //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 1.25f), Time.deltaTime * 1f);
                    //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)shadow.transform.position+(Vector2.up*1.25f), Time.deltaTime * 1f);
                    //float d = Vector2.Distance(teleObject.position, (Vector2)shadow.transform.position + (Vector2.up * 1.25f));


                    teleObject.localPosition = Vector2.Lerp(pos1, pos2, ti);
                    ti += Time.deltaTime * x1;


                    if (ti >= 1)
                    {
                        arc = true;
                        ti = 0;
                    }
                    //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)transform.position + (Vector2.up * 1.25f), Time.deltaTime * 1.5f+(Mathf.Abs(body.angularVelocity)* coef));
                    //float d = Vector2.Distance(teleObject.position, (Vector2)transform.position + (Vector2.up * 1.25f));

                    //sec += Time.deltaTime;

                    //Debug.Log("ARC:" + d);
                    //Debug.Log("ARC:" + teleObject.position + " / " + ((Vector2)transform.position + (Vector2.up * 1.25f))+"="+d+"  Time="+sec);
                    //if (teleObject.localPosition.y >= 1.25f)
                    //if (d<=0)
                    //{
                    //    arc = true;
                    //}

                }
                else
                {
                    if (!bounce)
                    {

                        teleObject.localPosition = Vector2.Lerp(pos2, pos3, ti);

                        ti += Time.deltaTime * x2;

                        if (ti >= 1)
                        {
                            bounce = true;
                            sfx.PlayOneShot(tapSFX);
                            ti = 0;
                            if (breakOnFall)
                            {
                                isBroken = true;
                                StartCoroutine(BreakObject());
                                //Destroy(gameObject);
                                break;
                            }

                        }


                        //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2.5f);
                        //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)shadow.transform.position, Time.deltaTime * 2.5f + (Mathf.Abs(body.angularVelocity) * coef));
                        //float d = Vector2.Distance(teleObject.position, (Vector2)shadow.transform.position);
                        //Debug.Log("BOUNCE1:" + d);

                        //if (teleObject.localPosition.y <= 0)
                        //if (d<=0)
                        //{
                        //    bounce = true;
                        //    sfx.PlayOneShot(tapSFX);
                        //    if (breakOnFall)
                        //    {
                        //        isBroken = true;
                        //        StartCoroutine(BreakObject());
                        //        //Destroy(gameObject);
                        //        break;
                        //    }

                        //}
                    }
                    else
                    {

                        if (!end)
                        {


                            teleObject.localPosition = Vector2.Lerp(pos3, pos4, ti);

                            ti += Time.deltaTime * x3;

                            if (ti >= 1)
                            {
                                end = true;
                                ti = 0;
                            }
                            //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0.2f), Time.deltaTime * 2f);
                            //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)shadow.transform.position + (Vector2.up * 0.2f), Time.deltaTime * 2f);
                            //float d = Vector2.Distance(teleObject.position, (Vector2)shadow.transform.position + (Vector2.up * 0.2f));
                            //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)transform.position + (Vector2.up * 0.2f), Time.deltaTime * 2f + (Mathf.Abs(body.angularVelocity) * coef));
                            //float d = Vector2.Distance(teleObject.position, (Vector2)transform.position + (Vector2.up * 0.2f));
                            //Debug.Log("BOUNCE2:" + d);

                            //if (teleObject.localPosition.y >= 0.2f)
                            //if (d<=0)
                            //{
                            //    end = true;
                            //}
                        }
                        else
                        {

                            teleObject.localPosition = Vector2.Lerp(pos4,oTeleObjectPos , ti);

                            ti += Time.deltaTime * x4;

                            if (ti>=1)
                            {
                                sfx.PlayOneShot(tapSFX);
                                break;
                            }

                            //teleObject.localPosition = Vector2.MoveTowards(teleObject.localPosition, new Vector2(0, 0), Time.deltaTime * 2f);

                            //teleObject.position = Vector2.MoveTowards(teleObject.position, (Vector2)shadow.transform.position, Time.deltaTime * 2f + (Mathf.Abs(body.angularVelocity) * coef));
                            //float d = Vector2.Distance(teleObject.position, (Vector2)shadow.transform.position);

                            //if (teleObject.localPosition.y <= 0)
                            //if (d <= 0)
                            //{
                            //    sfx.PlayOneShot(tapSFX);
                            //    break;
                            //}

                        }


                    }

                }

                aim = false;


                yield return new WaitForEndOfFrame();
            }
            shadow.SetActive(false);
            //controller.enabled = true;
            ready = false;
            throwready = false;
            thrown = false;


            //Vector2 cachepos = shadow.transform.position;
            //shadow.transform.localPosition = Vector2.zero;
            //teleObject.localPosition = Vector2.zero;
            //transform.position = cachepos;

            OnEnd.Invoke();
            //controller.SetInputVel(Vector2.zero);
            body.velocity = Vector2.zero;
            //body.bodyType = RigidbodyType2D.Kinematic;

            inThrow = false;
        }

        IEnumerator BreakObject()
        {
            breakParticles.Play();
            sprite.enabled = false;
            col.enabled = false;
            body.bodyType = RigidbodyType2D.Kinematic;
            body.velocity = Vector2.zero;
            //controller.SetInputVel(Vector2.zero);
            shadow.SetActive(false);
            sfx.PlayOneShot(tapSFX);
            yield return null;
        }

        public void OnMouseButton1Down()
        {
            if (canThrow)
            {
                aim = true;

                if (specialmode)
                    mouse.StopTelemove();
                //body.angularDrag = 1;
            }
            if (ThrowIndicator)
            {
                ThrowIndicator.gameObject.SetActive(true);
            }
        }

        public void OnMouseButton1Up()
        {
            if (!throwready)
            {
                throwready = true;
            }
            else
            {
                Debug.Log("THROW");
                if (specialmode)
                    mouse.DetachTelekinesis();
                BeginThrow();
            }

        }

        public void OnMouseButton2Down()
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseButton2Up()
        {
            throw new System.NotImplementedException();
        }

        public int GetInteractType()
        {
            throw new System.NotImplementedException();
        }
    }
}