using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETSLEEP_STATE : State
{
    AiCore ai;

    public bool value;

    public override void Initialize(BaseStateMachine m)
    {
        ai = m.GetComponentInParent<AiCore>();
        localFSM = m;
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        ai.isSleeping = value;
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
    }
}
