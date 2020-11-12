using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionQuery
{
    public string areaName;
    public Vector2 location;
    public Vector2 fromPos;
    public bool flag;
    public bool OUTorIN;

    public DirectionQuery(){ }

    public DirectionQuery(Vector2 loc, bool f)
    {
        location = loc;
        flag = f;
    }
}
