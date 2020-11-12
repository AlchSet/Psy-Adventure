using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WALKROUTE_STATE : State
{
    public enum PatrolMode { OneWay, Looping, PingPong, Random }
    public PatrolMode mode = PatrolMode.OneWay;

    public float speed = 2;

    RouteList routeList;
    public int selectedRoute;
    public int waypointIndex = 0;
    bool hasEnded;

    
    AiCore ai;

    public override void Initialize(BaseStateMachine m)
    {
        this.localFSM = m;
        ai = m.transform.parent.GetComponent<AiCore>();
        routeList = ai.transform.GetComponent<RouteList>();
        //ai.OnFinishPath += NextPoint;

        Debug.Log("INIT " + name + " State.....................DONE");
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        hasEnded = false;
        waypointIndex = 0;
        ai.controller.speed = speed;
        ai.SetDestination(routeList.routes[selectedRoute].GetGlobalPoint(waypointIndex));
        ai.stopMove = false;

        ai.OnFinishPath += NextPoint;
        done = false;
        //Debug.Log("MY STATE IS " + name + " AND MY ROUTE IS " + selectedRoute);
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        ai.OnFinishPath -= NextPoint;
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        Debug.Log("MY STATE IS " + name + " AND MY ROUTE IS " + selectedRoute);
        //throw new System.NotImplementedException();
    }


    public void NextPoint()
    {
        Debug.Log("NEXT POINT");


        


        switch(mode)
        {
            case PatrolMode.Looping:
                waypointIndex = (waypointIndex + 1) % routeList.routes[selectedRoute].Points.Length;
                //ai.SetDestination(routeList.routes[selectedRoute].GetGlobalPoint(waypointIndex));
                ai.SetDestination(routeList.GetPointInRoute(selectedRoute, waypointIndex));
                break;

            case PatrolMode.OneWay:

                if(waypointIndex<routeList.routes[selectedRoute].Points.Length-1)
                {
                    waypointIndex = Mathf.Clamp(waypointIndex + 1, 0, routeList.routes[selectedRoute].Points.Length);
                    ai.SetDestination(routeList.GetPointInRoute(selectedRoute, waypointIndex));
                }
                else
                {
                    if(!hasEnded)
                    {
                       
                        //FIND A WAY TO MOVE ON TO NEXT STATE
                        hasEnded = true;
                        ai.stopMove = true;
                        done = true;
                    }
                }





                break;


        }



    }


}
