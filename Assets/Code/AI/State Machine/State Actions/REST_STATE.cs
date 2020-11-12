using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REST_STATE : State
{
    AiCore ai;

    float oldEnergyDrain;

    public float multiplier = 5;

    public bool searchBench;

    Collider2D[] availableBenches = new Collider2D[5];

    public override void Initialize(BaseStateMachine m)
    {
        
        localFSM = m;
        ai = m.GetComponentInParent<AiCore>();
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        ai.QuitPath();
        if (searchBench)
        {
            Physics2D.OverlapCircleNonAlloc(localFSM.transform.position, 7, availableBenches);
            Collider2D b = null;

            foreach (Collider2D bench in availableBenches)
            {
                if (bench != null&&bench.CompareTag("REST"))
                    b = bench;
            }



            if(b!=null)
            {
                ai.OnFinishPath += Rejuvenate;
                ai.SetDestination(b.transform.position);
            }
            else
            {
                //FUCK IT
                Rejuvenate();
            }
            

        }
        else
        {
            Rejuvenate();
        }




    }

    public override void OnStateExit(BaseStateMachine m)
    {
        ai.drain = oldEnergyDrain;
        ai.OnFinishPath -= Rejuvenate;
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        //Debug.LogWarning("SLEEP " + ai.drain);
    }


    public void Rejuvenate()
    {
        Debug.LogWarning("YAREYAREYARE");
        oldEnergyDrain = ai.drain;
        ai.drain = -oldEnergyDrain * multiplier;
    }
}
