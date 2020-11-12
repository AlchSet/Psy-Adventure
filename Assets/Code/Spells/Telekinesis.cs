using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    public GameObject bullet;

    public Controller2D controller;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Fire(Player.PlayerAction action)
    {
        GameObject g = Instantiate(bullet, transform.position, Quaternion.identity);
        g.GetComponent<TelekinesisProjectile>().action = action;
        switch (controller.faceDir)
        {
            case Controller2D.Direction.North:

                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                g.GetComponent<TelekinesisProjectile>().Cast(90);

                break;
            case Controller2D.Direction.South:
                g.GetComponent<TelekinesisProjectile>().Cast(270);
                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 5, ForceMode2D.Impulse);
                break;

            case Controller2D.Direction.East:
                g.GetComponent<TelekinesisProjectile>().Cast(0);
                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                break;

            case Controller2D.Direction.West:
                g.GetComponent<TelekinesisProjectile>().Cast(180);
                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                break;






        }


    }


}
