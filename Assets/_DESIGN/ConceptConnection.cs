using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class ConceptConnection : MonoBehaviour
{

    LineRenderer line;

    public Transform to;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = true;

        if(to!=null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, to.position);
        }
        


    }

    private void OnValidate()
    {
        if (to != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, to.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (to != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, to.position);
        }
    }
}
