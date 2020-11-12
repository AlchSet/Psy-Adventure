using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteList : MonoBehaviour
{
    public Route[] routes;




    public Route GetRoute(int index)
    {
        return routes[index];
    }


    public Vector2 GetPointInRoute(int index, int point)
    {
        Debug.Log("RouteNAME=" + routes[index].name);
        return routes[index].GetGlobalPoint(point);
    }


}
