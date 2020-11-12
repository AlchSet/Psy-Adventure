using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoBeam : MonoBehaviour
{

    public GameObject bullet;

    public Controller2D controller;

    AudioSource sfx;

    bool limit;

    float elapsed;
    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(limit)
        {
            elapsed += Time.deltaTime;
            if(elapsed>=0.25f)
            {
                limit = false;
                elapsed = 0;
            }
        }
    }


    // Update is called once per frame
    public void Fire()
    {
        if (!limit)
        {
            sfx.PlayOneShot(sfx.clip);
            GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);

            if(controller.InputVel==Vector2.zero)
            {
                switch (controller.faceDir)
                {
                    case Controller2D.Direction.North:

                        g.transform.rotation = Quaternion.Euler(0, 0, 90);

                        g.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);
                        g.GetComponent<PsychobeamProjectile>().dir = Vector2.up;

                        break;
                    case Controller2D.Direction.South:
                        g.transform.rotation = Quaternion.Euler(0, 0, -90);
                        g.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 8, ForceMode2D.Impulse);
                        g.GetComponent<PsychobeamProjectile>().dir = Vector2.down;

                        break;

                    case Controller2D.Direction.East:
                        g.transform.rotation = Quaternion.Euler(0, 0, 0);

                        g.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8, ForceMode2D.Impulse);
                        g.GetComponent<PsychobeamProjectile>().dir = Vector2.right;

                        break;

                    case Controller2D.Direction.West:
                        g.transform.rotation = Quaternion.Euler(0, 0, 180);
                        g.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 8, ForceMode2D.Impulse);
                        g.GetComponent<PsychobeamProjectile>().dir = Vector2.left;

                        break;






                }
            }
            else
            {

                float a = Mathf.Atan2(controller.InputVel.y, controller.InputVel.x) * Mathf.Rad2Deg;

                g.GetComponent<PsychobeamProjectile>().dir = controller.InputVel;


                g.transform.rotation = Quaternion.Euler(0, 0, a);

                g.GetComponent<Rigidbody2D>().AddForce(controller.InputVel * 8, ForceMode2D.Impulse);
            }


            ////g.GetComponent<TelekinesisProjectile>().action = action;
            //switch (controller.faceDir)
            //{
            //    case Controller2D.Direction.North:

            //        g.transform.rotation = Quaternion.Euler(0, 0, -90);

            //        g.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);


            //        break;
            //    case Controller2D.Direction.South:
            //        g.transform.rotation = Quaternion.Euler(0, 0, 90);
            //        g.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 8, ForceMode2D.Impulse);
            //        break;

            //    case Controller2D.Direction.East:
            //        g.transform.rotation = Quaternion.Euler(0, 0, 180);

            //        g.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8, ForceMode2D.Impulse);
            //        break;

            //    case Controller2D.Direction.West:
            //        g.transform.rotation = Quaternion.Euler(0, 0, 0);
            //        g.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 8, ForceMode2D.Impulse);
            //        break;






            //}
            limit = true;
        }
       
    }
}
