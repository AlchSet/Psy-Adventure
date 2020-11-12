using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATTACK_STATE : State
{
    AiCore ai;

    bool hasFired;

    float elapsed;
    public override void Initialize(BaseStateMachine m)
    {
        localFSM = m;
        ai = m.GetComponentInParent<AiCore>();
        Debug.Log(ai.ToString());
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        if(!hasFired)
        {
            ai.tempSpell.Fire(ai.target.position- m.transform.position);
            hasFired = true;
        }
        else
        {
            elapsed += Time.deltaTime;

            if(elapsed>2)
            {
                elapsed = 0;
                hasFired = false;
            }
        }

        //throw new System.NotImplementedException();
    }

   
}
