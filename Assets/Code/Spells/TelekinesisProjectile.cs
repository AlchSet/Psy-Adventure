using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisProjectile : MonoBehaviour
{
    Rigidbody2D body;
    ParticleSystem particles;
    Collider2D collider;
    public float elapsed;
    bool stop;

    public float angle;

    public Player.PlayerAction action;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!stop)
        {
            elapsed += Time.deltaTime;

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            if(input.sqrMagnitude>0)
            {
                float a = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

                angle = Mathf.LerpAngle(angle, a, Time.deltaTime*3);
            }
            

            //0 is right
            //90 is up
            //180 is left
            //270 is down


            body.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad))*5;

            //if (input.magnitude != 0)
            //{
            //    float m = body.velocity.magnitude;

            //    Vector2 nvel = input.normalized * m;

            //    body.velocity = nvel;


            //}


            if (elapsed >= 1.5f)
            {
                particles.Stop();
                body.velocity = Vector2.zero;
                body.bodyType = RigidbodyType2D.Static;
                collider.enabled = false;
                stop = true;
                if (action != null)
                    action();
                StartCoroutine(CleanUp());
            }
        }


    }

    public void Cast(float angle)
    {
        this.angle = angle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Telekinesis")
        {
            particles.Stop();
            body.velocity = Vector2.zero;
            body.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
            collision.GetComponent<TelekineticObject>().enabled = true;
            collision.GetComponent<TelekineticObject>().action = action;
            stop = true;
            StartCoroutine(CleanUp());
        }
    }


    IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
