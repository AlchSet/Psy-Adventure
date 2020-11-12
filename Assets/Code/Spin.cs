using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public float spinSpeed = 100;

    public bool useUnscaledTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!useUnscaledTime)
        {
            transform.Rotate(Vector3.forward * (spinSpeed * Time.deltaTime));
        }
        else
        {
            transform.Rotate(Vector3.forward * (spinSpeed * Time.unscaledDeltaTime));
        }
        
    }
}
