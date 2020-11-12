using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TempCamFollow : MonoBehaviour
{

    public Transform target;
    PixelPerfectCamera p;
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       

        Vector3 npos = target.position;
        npos = p.RoundToPixel(npos);
        npos.z = transform.position.z;

        Vector3 tpos = Vector3.MoveTowards(transform.position, npos, Time.deltaTime*3.2f);

        tpos = p.RoundToPixel(tpos);


        transform.position = tpos;

    }
}
