using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RaycastController2D : MonoBehaviour
{
    public Vector2 inputVel;

    public float speed = 3;

    public bool useUnscaledTime;

    public PixelPerfectCamera pixelCam;


    Vector2 DEBUG_RAY_X;
    Vector2 DEBUG_RAY_Y;


    float DRX;
    float DRY;



    public float skin = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 moveDelta)
    {
        float delta = Time.deltaTime;
        if (useUnscaledTime)
            delta = Time.unscaledDeltaTime;

        moveDelta = moveDelta * speed;

        //Debug.DrawRay((Vector2)transform.position + moveDelta.normalized * 0.16f, moveDelta,Color.red);


        RaycastHit2D b=Physics2D.Raycast((Vector2)transform.position + Vector2.right * (0.16f * Mathf.Sign(moveDelta.x)), Vector2.right * moveDelta.x, speed, 1 << LayerMask.NameToLayer("MDW"));
        RaycastHit2D c = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (0.16f * Mathf.Sign(moveDelta.y)), Vector2.up * moveDelta.y, speed, 1 << LayerMask.NameToLayer("MDW"));

        Debug.DrawRay((Vector2)transform.position + Vector2.right * (0.16f*Mathf.Sign(moveDelta.x)), Vector2.right * (3*Mathf.Sign(moveDelta.x)), Color.red);
        Debug.DrawRay((Vector2)transform.position + Vector2.up * (0.16f*Mathf.Sign(moveDelta.y)), Vector2.up * (3 * Mathf.Sign(moveDelta.y)), Color.red);

        if(b.collider)
        {
            if(!b.collider.isTrigger)
            {
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (0.16f * Mathf.Sign(moveDelta.x)), Vector2.right * (3 * Mathf.Sign(moveDelta.x)), Color.yellow);
                DEBUG_RAY_X = b.point;
                DRX = b.distance;

                if(b.distance<=0.126f+skin)
                {
                    moveDelta.x = 0;
                }

            }






        }

        if (c.collider)
        {

            if(!c.collider.isTrigger)
            {
                Debug.DrawRay((Vector2)transform.position + Vector2.up * (0.16f * Mathf.Sign(moveDelta.y)), Vector2.up * (3 * Mathf.Sign(moveDelta.y)), Color.yellow);

                DEBUG_RAY_Y = c.point;
                DRY = c.distance;

                if (c.distance <= 0.126f+skin)
                {
                    moveDelta.y = 0;
                }

            }



        }

        transform.Translate(moveDelta*delta);

        if (pixelCam)
            transform.position = pixelCam.RoundToPixel(transform.position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(DEBUG_RAY_Y, 0.25f);
        Gizmos.DrawWireSphere(DEBUG_RAY_X, 0.25f);
    }

    //private void OnGUI()
    //{
    //    GUIStyle s = new GUIStyle();
    //    s.fontSize = 30;
    //    s.fontStyle = FontStyle.Bold;

    //    s.normal.textColor = Color.white;
    //    GUI.color = Color.white;
    //    GUI.Label(new Rect(50,50,100,50),"X RAY DIST: "+DRX,s);
    //    GUI.Label(new Rect(50,100,100,50),"Y RAY DIST: "+DRY,s);
        
    //}

}
