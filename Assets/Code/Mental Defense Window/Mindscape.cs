using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mindscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScrollUp()
    {
        Debug.Log("SCROLL UP");

        Vector2 t = transform.localPosition;
        t.y =t.y- 11.875f;
        transform.localPosition = t;
    }

    public void ScrollDown()
    {
        Debug.Log("SCROLL DOWN");

        Vector2 t = transform.localPosition;
        t.y = t.y + 11.875f;
        transform.localPosition = t;
    }

    public void ScrollLeft()
    {
        Debug.Log("SCROLL LEFT");

        Vector2 t = transform.localPosition;
        t.x = t.x + 13.94f;
        transform.localPosition = t;
    }

    public void ScrollRight()
    {
        Debug.Log("SCROLL RIGHT");

        Vector2 t = transform.localPosition;
        t.x = t.x - 13.94f;
        transform.localPosition = t;
    }


    


}
