using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    Transform player;

    public float range = 5;

    public bool detect;
    
    public SensorActions[] designatedTargets;
    public Collider2D[] seenObjects = new Collider2D[20];

    public Transform target;

    //public HashSet<Collider2D> obtainedTargets = new HashSet<Collider2D>();

    //public Animator ai;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.root;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < seenObjects.Length; i++)
        {
            seenObjects[i] = null;
        }


        Physics2D.OverlapCircleNonAlloc(transform.position, range, seenObjects, 1 << LayerMask.NameToLayer("Bounds"));



        foreach(SensorActions s in designatedTargets)
        {
            for (int i = 0; i < seenObjects.Length; i++)
            {
                if(seenObjects[i]==null)
                {
                    continue;
                }

                if(seenObjects[i].CompareTag(s.tag))
                {
                    target = seenObjects[i].transform;
                    //obtainedTargets.Add(seenObjects[i]);
                    s.action.Invoke();
                }


                //seenObjects[i] = null;
            }
        }





        float d = Vector2.Distance(transform.position, player.position);

        if(d<=5)
        {
            detect = true;
            
        }
        else
        {
            detect = false;
        }

        //Physics2D.overlapC

        //ai.SetBool("DetectPlayer",detect);
    }

    [System.Serializable]
    public class SensorActions
    {
        public string tag;
        public UnityEvent action;
    }

}



