using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDLE_STATE : State
{
    public override void Initialize(BaseStateMachine m)
    {
        //throw new System.NotImplementedException();
    }

    //Do Nothing

    //Option to wait for timer

    //Maybe make CheckTransitions overridable for the timer to work.



    public override void OnStateEnter(BaseStateMachine m)
    {
        //Debug.Log(this.name + " TRANSITION=" + transitions.Count);
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        
    }
}
