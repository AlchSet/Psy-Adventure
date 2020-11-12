using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : MonoBehaviour
{

    public enum Direction { North, South, East, West }

    public Direction facingDirection = Direction.South;

    ParticleSystem particles;
    SpriteRenderer sprite;
    Animator anim;

    public Gradient skullColorEffects;

    public AnimationCurve alphaCurve;

    public bool Play;
    Color oColor;

    Vector2 skullPos;

    Vector2 oPos;

    public float shakeFreequency = 100;
    public float shakeAmplitude = 0.0625f;

    public float range = 3;

    AudioSource sfx;

    public Collider2D[] victims = new Collider2D[10];

    // Start is called before the first frame update
    void Awake()
    {
        sfx = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = sprite.GetComponent<Animator>();
        oColor = sprite.color;
        sprite.color = Color.clear;
        oPos = sprite.transform.localPosition;



    }

    private void Start()
    {
        //particles.Play();
        //StartCoroutine(Testu());
    }

    public void DetectVictims()
    {

        for (int i = 0; i < victims.Length; i++)
        {
            victims[i] = null;
        }
        Physics2D.OverlapCircleNonAlloc(transform.position, range, victims, 1 << LayerMask.NameToLayer("Bounds"));

        foreach (Collider2D c in victims)
        {
            if (c != null && c.CompareTag("Citizen"))
            {

                Vector2 dirA = c.transform.position - transform.position;
                dirA.Normalize();
                Vector2 dirB = GetDirectionVector(facingDirection);
                if(Vector2.Angle(dirA, dirB)<95)
                {
                    c.GetComponentInParent<AiCore>().InflictFear(transform.position);
                }
                //Debug.Log("ANGLE=" + );

                
            }
        }


    }
    // Update is called once per frame
    void Update()
    {
        if (Play)
        {
            StartCoroutine(FearAnimation());
            Play = false;
        }

    }

    IEnumerator FearAnimation()
    {
        sfx.PlayOneShot(sfx.clip);
        float duration = 0.6f;
        float elapsed = 0;
        float i = 0;
        particles.Play();
        anim.SetTrigger("Play");
        skullPos = sprite.transform.position;
        DetectVictims();
        while (true)
        {
            elapsed += Time.deltaTime;
            i = elapsed / duration;

            //sprite.color = skullColorEffects.Evaluate(i);
            Color c = oColor;
            c.a = alphaCurve.Evaluate(i);
            sprite.color = c;

            //Shake
            Vector2 npos = skullPos + (Vector2.up * Mathf.Sin(Time.time * shakeFreequency) * shakeAmplitude) + (Vector2.right * Mathf.Cos(Time.time * shakeFreequency) * shakeAmplitude);

            sprite.transform.position = npos;

            //Debug.LogError(i);
            if (i >= 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        sprite.transform.localPosition = oPos;
        yield return null;
    }

    public Vector2 GetDirectionVector(Direction dir)
    {
        Vector2 var = Vector2.zero;
        switch (dir)
        {
            case Direction.North:
                var = Vector2.up;
                break;

            case Direction.South:
                var = Vector2.down;
                break;

            case Direction.East:
                var = Vector2.right;
                break;

            case Direction.West:
                var = Vector2.left;
                break;
        }
        return var;
    }


    IEnumerator Testu()
    {

        StartCoroutine(FearAnimation());

        //particles.Play();

        yield return new WaitForSeconds(2);
        //particles.Play();
        StartCoroutine(FearAnimation());
        yield return new WaitForSeconds(2);
        //particles.Play();
        StartCoroutine(FearAnimation());
        //yield return new WaitForSeconds(2);


        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
