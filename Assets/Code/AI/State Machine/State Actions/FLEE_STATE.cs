using MyStateMachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLEE_STATE : State
{

    AiCore ai;

    Vector2 escapePoint;

    public override void Initialize(BaseStateMachine m)
    {
        this.localFSM = m;
        ai = m.transform.parent.GetComponent<AiCore>();
        //done = false;
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        Vector2 dir = localFSM.transform.position - ai.target.position;

        RaycastHit2D hit = Physics2D.Raycast(localFSM.transform.position, dir.normalized, 20, 1 << LayerMask.NameToLayer("Concrete"));

        if(hit.collider)
        {

            NNInfo info = AstarPath.active.GetNearest(hit.point,NNConstraint.Default);

            escapePoint = info.position;

           

        }
        else
        {
            NNInfo info = AstarPath.active.GetNearest((Vector2)localFSM.transform.position+dir.normalized*20,NNConstraint.Default);
            escapePoint = info.position;
        }


        //Debug.Log("<Color=yellow>MY SPEED IS:</Color>" + ai.controller.speed);

        ai.OnFinishPath += FinishFleePoint;



        ai.SetDestination(escapePoint);
        ai.speed = 3;
        


        //throw new System.NotImplementedException();
    }

    public override void OnStateExit(BaseStateMachine m)
    {

        ai.OnFinishPath -= FinishFleePoint;
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
      
        Debug.DrawRay(escapePoint, Vector2.up);
        Debug.DrawRay(escapePoint, Vector2.down);
        Debug.DrawRay(escapePoint, Vector2.left);
        Debug.DrawRay(escapePoint, Vector2.right);
        //throw new System.NotImplementedException();
    }

    public void FinishFleePoint()
    {
        Debug.Log("Reached Flee Point");
    }

    
}
