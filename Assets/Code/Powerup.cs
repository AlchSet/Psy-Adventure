using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    Vector2 oPos;


    // Start is called before the first frame update
    void Start()
    {
        oPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = oPos +Vector2.up* (Mathf.Sin(Time.time*20)*0.1f);
    }
}
