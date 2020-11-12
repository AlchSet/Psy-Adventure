using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public Seeker seeker;
    public Controller2D controller;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = .2f;

    public int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public delegate void Action();

    bool finishPath;

    public Action OnFinishPath;

    public bool stopMove;

    public Animator anim;

    bool selectp;



    public Damageable dmg;

    public Vector2 originPos;

    public UnityEvent OnReset;

    
    


    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<Controller2D>();

        OnFinishPath += DefaultMethod;


        dmg = GetComponentInChildren<Damageable>();

        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
        {
            Debug.Log("NO PATH");
            controller.InputVel = Vector2.zero;
            return;
        }

        //reachedEndOfPath = false;

        float distanceToWaypoint = 100;


        distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);


        if (distanceToWaypoint < nextWaypointDistance)
        {
            if (currentWaypoint == path.vectorPath.ToArray().Length - 1)
            {
                Debug.Log("END PATH");
                reachedEndOfPath = true;

                if (!selectp)
                {
                    StartCoroutine(SelectPath());
                }
                //OnFinishPath();
                return;
            }

            currentWaypoint = Mathf.Clamp(currentWaypoint + 1, 0, path.vectorPath.ToArray().Length - 1);



            //Debug.Log("NEXT POINT");
        }


        

        //var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        float speedFactor = 1f;

        //Debug.Log(speedFactor);

        if (!stopMove && !reachedEndOfPath)
        {
            Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            Vector2 velocity = dir * speed * speedFactor;

            //controller.InputVel = velocity;

            controller.SetInputVel(velocity);

            if (anim)
            {
                anim.SetFloat("X", controller.InputVel.x);
                anim.SetFloat("Y", controller.InputVel.y);

                anim.SetBool("isMoving", controller.isMoving);



            }
        }
        else
        {
            controller.SetInputVel(Vector2.zero);
            //controller.inputVel = Vector2.zero;

        }


    }

    void DefaultMethod()
    {
        Debug.Log("FINISH PATH");
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);

        if (!p.error)
        {

            Debug.Log("New path");
            path = p;

            currentWaypoint = 0;
            //reachedEndOfPath = false;

            //Debug.Log("RESET WAYPOINT");
        }
    }


    public void SetDestination(Vector2 newPos)
    {
        //Debug.Log("SET PATH");

        seeker.StartPath(transform.position, newPos, OnPathComplete);


        //currentWaypoint = 0;
    }



    public void Reset()
    {
        transform.position = originPos;
        dmg.life = dmg.maxLife;

        OnReset.Invoke();
    }


    IEnumerator SelectPath()
    {
        selectp = true;

        yield return new WaitForSeconds(0.1f);

        OnFinishPath();
        yield return new WaitForSeconds(0.1f);
        reachedEndOfPath = false;
        selectp = false;

    }
}
