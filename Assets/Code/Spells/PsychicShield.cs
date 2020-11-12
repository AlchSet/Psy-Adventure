using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PsychicShield : MonoBehaviour
{ 

    GameObject shield;
    Controller2D control;
    Collider2D collider;
    Vector2 offset;
    bool isOn;
    Transform player;
    public Damageable pD;
    public Sprite flash;
    Sprite oSprite;
    SpriteRenderer r;
    PixelPerfectCamera pixCam;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponentInChildren<SpriteRenderer>();
        control = transform.root.GetComponent<Controller2D>();
        shield = transform.Find("PsyShield 1").gameObject;
        shield.SetActive(false);
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        player = transform.root;
        offset = transform.localPosition;
        transform.SetParent(null);
        oSprite = r.sprite;
        pixCam = Camera.main.GetComponent<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            // transform.position = (Vector2)player.position + offset;
            transform.position=pixCam.RoundToPixel((Vector2)player.position + offset);
            //r.transform.localPosition = pixCam.RoundToPixel(r.transform.localPosition);
        }
    }


    public void OnPress()
    {
        isOn = true;
        collider.enabled = true;
        transform.position = (Vector2)player.position + offset;

        switch (control.faceDir)
        {
            case Controller2D.Direction.North:

                //transform.rotation = Quaternion.Euler(0, 0, 180);

                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);


                break;
            case Controller2D.Direction.South:

                //transform.rotation = Quaternion.Euler(0, 0, 0);
                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 7, ForceMode2D.Impulse);
                break;

            case Controller2D.Direction.East:
                //transform.rotation = Quaternion.Euler(0, 0, 90);

                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 7, ForceMode2D.Impulse);
                break;

            case Controller2D.Direction.West:
                //transform.rotation = Quaternion.Euler(0, 0, -90);
                //g.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 7, ForceMode2D.Impulse);
                break;






        }





        //control.lockDirection = true;
        shield.SetActive(true);
    }

    public void OnRelease()
    {
        isOn = false;
        collider.enabled = false;
        control.lockDirection = false;
        shield.SetActive(false);
    }


    public void PlaceShield()
    {
        //transform.position = (Vector2)player.position + offset;
        transform.position = pixCam.RoundToPixel((Vector2)player.position + offset);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            collision.GetComponent<Damageable>().DealDamage(0, 10,transform.position);

            pD.DealDamage(0, 5, collision.transform.position);

            StartCoroutine(Flash());
            //////Damageable.CollisionInfo info = new Damageable.CollisionInfo();

            //info.totalDmg = 1;
            //info.knockbackForce = 10;
            //info.position = collision.transform.root.position;
            ////info.position=transform.position-
            //damageable.DealDamage(info);

            //damageable.DealDamage(1);
            //Debug.Log("ENEMY HURTS ME");
        }

    }


    IEnumerator Flash()
    {
        r.sprite = flash;

        yield return new WaitForSeconds(0.01f);

        r.sprite = oSprite;
    }
}
