using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSoul : MonoBehaviour
{
    public enum SoulStates { WANDER,CHASE,POSSESS,FLEE}

    public SoulStates state = SoulStates.WANDER;

    Transform target;

    Rigidbody2D body;

    Collider2D col;

    ParticleSystem particles;
    AiCore victim;
    Sensor sensor;
    PossessedCitizenA job;

    Spell fireball;

    SpriteRenderer formSprite;

    GameObject hitbox;
    Damageable life;
    // Start is called before the first frame update
    void Start()
    {
        sensor = transform.Find("Vision").GetComponent<Sensor>();
        job = GetComponent<PossessedCitizenA>();
        job.sensor = sensor;
        fireball = transform.Find("Fireball").GetComponent<Spell>();

        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;

        col = GetComponent<Collider2D>();
        col.enabled = false;

        particles = transform.Find("Particle System").GetComponent<ParticleSystem>();

        formSprite = transform.Find("FormSprite").GetComponent<SpriteRenderer>();
        hitbox = transform.Find("Hitbox").gameObject;
        life = hitbox.GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case SoulStates.WANDER:


                Transform t = SearchForHost();

                if (t != null)
                {
                    target = t;
                    col.enabled = true;
                    state = SoulStates.CHASE;
                }
                break;


            case SoulStates.CHASE:

                MoveToTarget();
               

                break;


            case SoulStates.POSSESS:


                break;

            case SoulStates.FLEE:


                

                break;

        }
    }


    Transform SearchForHost()
    {
        Collider2D[] cols=Physics2D.OverlapCircleAll(transform.position, 5, 1 << LayerMask.NameToLayer("Bounds"));

        Transform t = null;

        foreach(Collider2D c in cols)
        {
            if(c.CompareTag("Citizen"))
            {
                Debug.Log("<color=yellow> FOUND A HUMAN </color>");

                t = c.transform.root;
                break;

            }
        }

        return t;

    }

    void MoveToTarget()
    {
        body.MovePosition(Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 1.5f));
    }

    void PossessTarget()
    {
        victim = target.GetComponent<AiCore>();
        job.victim = victim;
        job.life.intType = life.life;
        victim.QuitPath();
        victim.tempSpell = fireball;
        foreach(MyStateMachine.BaseStateMachine j in victim.listOfJobs)
        {
            j.enabled = false;
        }

        transform.SetParent(target);

        job.tranformToDemon.fromSprite = victim.sprite;
        job.tranformToDemon.toSprite = formSprite;
        job.tranformToDemon.hitbox = hitbox;
        job.tranformToDemon.toState = true;


        job.returnBackToNormal.fromSprite = formSprite;
        job.returnBackToNormal.toSprite = victim.sprite;
        job.returnBackToNormal.hitbox = hitbox;
        job.returnBackToNormal.toState = false;

        job.deathReturnNormalForm.fromSprite = formSprite;
        job.deathReturnNormalForm.toSprite = victim.sprite;
        job.deathReturnNormalForm.hitbox = hitbox;
        job.deathReturnNormalForm.toState = false;




        job.InitializeNewBody();
        particles.Stop();
        body.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        state = SoulStates.POSSESS;
    }

    public void Exorcised()
    {
        state = SoulStates.FLEE;
        col.enabled = true;
        particles.Play();
        transform.SetParent(null);
    }


    public void UpdateLife()
    {
        job.life.intType = life.life;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Citizen")&&state!=SoulStates.FLEE)
        {
            Debug.Log("<color=red> POSSES DAT BITCH</color>");
            PossessTarget();
        }

        if (collision.CompareTag("Player") && state == SoulStates.FLEE)
        {
            Debug.Log("PLAYER ABSORB SOUL");
            col.enabled = false;
            particles.Stop();
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position,10);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, sensor.range);
        }
        
    }

}
