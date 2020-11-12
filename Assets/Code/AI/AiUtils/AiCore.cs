using MyStateMachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AiCore : MonoBehaviour
{
    public delegate void TeleportEvent();

    public TeleportEvent OnTeleport;

    public float Energy=100;

    [HideInInspector]
    //Formula is number of seconds in a entire day/100
    public float drain=0.069444444f;
    public enum StatusEffect { OK, STUN, FEAR, FRENZY }

    public StatusEffect status;

    StatusEffect lastStatus;

    public UnityEvent OnWakingUp;

    public UnityEvent OnSleeping;


    public UnityEvent OnEnterArea;

    public UnityEvent OnStunned;
    public UnityEvent OnExitStun;


    public AreaData currentArea;

 

    public FearData fearData;

    [System.Serializable]
    public struct FearData
    {
        public Vector2 fearPoint;
        public float fearSpeed;
        public float cacheSpeed;
        public float duration;
        public float elapsed;


        public FearData(Vector2 f, float sp, float osp, float d)
        {
            fearPoint = f;
            fearSpeed = sp;
            cacheSpeed = osp;
            duration = d;
            elapsed = 0;
        }


        public void Tick()
        {
            elapsed += Time.deltaTime;
        }

        public bool CheckDuration()
        {
            return elapsed >= duration ? true : false;
        }

    }


    public bool isSleeping
    {
        get
        {
            return m_isSleeping;
        }

        set
        {
            m_isSleeping = value;
            if (m_isSleeping)
            {
                Debug.Log("IM SLEEPING");
                OnSleeping.Invoke();
            }
            else
            {
                Debug.Log("IM AWAKE");
                OnWakingUp.Invoke();
            }
        }

    }
    [SerializeField]
    private bool m_isSleeping;

    public Controller2D controller;

    public Damageable dmg;

    //public Vector2 originPos;

    public Seeker seeker;
    public Path path;

    public float speed = 2;
    public float nextWaypointDistance = .2f;
    public int currentWaypoint = 0;

    public bool reachedEndOfPath;
    bool finishPath;
    public bool stopMove;
    bool selectp;


    public delegate void Action();
    public Action OnFinishPath;

    public UnityEvent OnReset;

    //Temporary for dependency issues.
    public Animator anim;


    bool tst;

    Vector2 destination;


    public List<GenericToggleObject> toggleObjects = new List<GenericToggleObject>();

    public List<BaseStateMachine> listOfJobs = new List<BaseStateMachine>();

    public Transform target;

    public Spell tempSpell;


    public SpriteRenderer sprite;


    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<Controller2D>();
        //OnFinishPath += DefaultMethod;


        dmg = GetComponentInChildren<Damageable>();

        //originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        float _drain = drain;

        switch(status)
        {
            case StatusEffect.OK:
                FollowPath();
                break;


            case StatusEffect.STUN:


                break;


            case StatusEffect.FEAR:

                _drain += 5;
                RunInFear();

                break;


            case StatusEffect.FRENZY:


                break;
        }


        Energy -= _drain * Time.deltaTime;

        


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
            tst = false;
            reachedEndOfPath = false;
            stopMove = false;
            //reachedEndOfPath = false;

            //Debug.Log("RESET WAYPOINT");
        }
    }


    public void SetPath(Path p)
    {
        path = p;

        currentWaypoint = 0;
        tst = false;
        reachedEndOfPath = false;
        stopMove = false;
    }

    public void SetDestination(Vector2 newPos)
    {
        //Debug.Log("SET PATH");
        destination = newPos;
        seeker.StartPath(transform.position, destination, OnPathComplete);


        //currentWaypoint = 0;
    }

    /// <summary>
    /// Runs routing logic
    /// </summary>
    public void FollowPath()
    {
        if (path == null)
        {
            //Debug.Log("NO PATH");
            controller.InputVel = Vector2.zero;
            return;
        }

        float distanceToWaypoint = 100;


        distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);


        if (distanceToWaypoint < nextWaypointDistance)
        {
            if (currentWaypoint == path.vectorPath.ToArray().Length - 1)
            {
                Debug.Log("END PATH");
                reachedEndOfPath = true;

                if (!selectp && !tst)
                {
                    //Debug.Log("SEL PATH:"+distanceToWaypoint+","+ currentWaypoint+","+path.ToString());
                    StartCoroutine(SelectPath());
                    tst = true;
                    stopMove = true;
                }
                //OnFinishPath();
                return;
            }

            currentWaypoint = Mathf.Clamp(currentWaypoint + 1, 0, path.vectorPath.ToArray().Length - 1);

            //Debug.Log("CurrentWypoint=" + currentWaypoint);


            //Debug.Log("NEXT POINT");
        }
        else
        {

            //Debug.Log("WALKING PATH");
        }




        //var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        float speedFactor = 1f;

        //Debug.Log(speedFactor);

        if (!stopMove && !reachedEndOfPath)
        {
            Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            //Vector2 velocity = dir * speed * speedFactor;



            //controller.InputVel = velocity;

            controller.SetInputVel(dir);

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


    public void RunInFear()
    {
        controller.speed = fearData.fearSpeed;
        Vector2 dir =  (Vector2)transform.position- fearData.fearPoint;
        dir.Normalize();
        controller.SetInputVel(dir);
        fearData.Tick();
        if(fearData.CheckDuration())
        {
            controller.speed = fearData.cacheSpeed;
            SetStatus((int)StatusEffect.OK);
        }
    }
    public void SetStatus(int s)
    {
        status = (StatusEffect)s;

        switch (status)
        {
            case StatusEffect.OK:

                if (lastStatus == StatusEffect.STUN|| lastStatus == StatusEffect.FEAR)
                {
                    lastStatus = status;
                    OnExitStun.Invoke();
                }

                break;


            case StatusEffect.STUN:

                lastStatus = StatusEffect.STUN;
                OnStunned.Invoke();

                break;


            case StatusEffect.FEAR:

                lastStatus = StatusEffect.STUN;
                OnStunned.Invoke();
                QuitPath();

                break;

            case StatusEffect.FRENZY:

                break;

        }


        //if(status==StatusEffect.STUN)
        //{
        //    lastStatus = StatusEffect.STUN;
        //    OnStunned.Invoke();
        //}
        //else
        //{
        //    if (lastStatus == StatusEffect.STUN)
        //    {
        //        lastStatus = status;
        //        OnExitStun.Invoke();
        //    }
        //}





    }


    public void InflictFear(Vector2 pos)
    {
        fearData = new FearData(pos, controller.speed * 2, controller.speed,7);
        SetStatus((int)StatusEffect.FEAR);
    }


    public void QuitPath()
    {
        path = null;

        currentWaypoint = 0;
        tst = false;
        reachedEndOfPath = false;
        stopMove = false;
        OnFinishPath = null;
    }

    IEnumerator SelectPath()
    {



        selectp = true;


        //COMMENTED CODE COULD BE USED FOR DELAYING PER POINT

        //yield return new WaitForSeconds(0.56f);

        OnFinishPath();
        //yield return new WaitForSeconds(0.6f);
        reachedEndOfPath = false;
        selectp = false;



        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Teleport")
        {
            Debug.Log("Teleport");
            //TeleportAI(collision.GetComponent<Teleport>());
            StartCoroutine(StartTeleport(collision.GetComponent<Teleport>()));
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Area"))
        {
            currentArea = collision.GetComponent<AreaData>();
            OnEnterArea.Invoke();
        }

    }

    IEnumerator StartTeleport(Teleport tele)
    {

        yield return new WaitForEndOfFrame();

        TeleportAI(tele);
        path = null;

        if(OnTeleport!=null)
        {
            OnTeleport();
        }
        //SetDestination(destination);
    }


    public void TeleportAI(Teleport tele)
    {
        transform.position = tele.GetDestination();

    }


}

[System.Serializable]
public class GenericToggleObject
{
    public GameObject GENERICOBJECT;
    public bool GENERICSTATE;


    public void ToggleGenericObject()
    {
        GENERICOBJECT.SetActive(!GENERICOBJECT.activeSelf);
    }

}
