using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOGGLEOBJECT_STATE : State
{
    AiCore ai;
    public int[] selectedObject;

    public override void Initialize(BaseStateMachine m)
    {
        ai = m.GetComponentInParent<AiCore>();
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
       foreach(int i in selectedObject)
        {
            ai.toggleObjects[i].ToggleGenericObject();
        }
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        //throw new System.NotImplementedException();
    }


    public void AddSelectedObjects(int[]args)
    {
        selectedObject = new int[args.Length];

        foreach(int i in args)
        {
            selectedObject[i] = args[i];
        }
    }
}
