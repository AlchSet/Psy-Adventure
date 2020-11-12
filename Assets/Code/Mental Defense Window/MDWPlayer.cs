using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Controller2D))]
public class MDWPlayer : MonoBehaviour
{
    public Vector2 inputVel;
    Controller2D controller;
    RaycastController2D rController;


    public UnityEvent OnWinMDW;

    Collider2D d;


    public UnityEvent OnHitHazard;
    public UnityEvent OnDeath;


    public int life = 3;


    public MDWMain.Action OnTeleport;

    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<Controller2D>();
        rController = GetComponent<RaycastController2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        inputVel.x = Input.GetAxisRaw("Horizontal");
        inputVel.y = Input.GetAxisRaw("Vertical");

        rController.Move(inputVel);


        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.28f, 1 << LayerMask.NameToLayer("MDW"));

        d = col;

        if(col!=null)
        {
            if (col.CompareTag("Finish"))
            {
                OnWinMDW.Invoke();
                Debug.Log("<Color=Green> MDW WIN </Color>");
            }
            else if(col.CompareTag("Hazard"))
            {
                OnHitHazard.Invoke();
                life--;
                Debug.Log("<Color=Red> MDW OUCH! </Color>");
            }
            else if(col.CompareTag("MDWT_U"))
            {
                OnTeleport(1);
                Debug.Log("TELEPORT");
            }
            else if(col.CompareTag("MDWT_D"))
            {
                OnTeleport(2);
                Debug.Log("TELEPORT");
            }
            else if(col.CompareTag("MDWT_L"))
            {
                OnTeleport(3);
                Debug.Log("TELEPORT");
            }
            else if(col.CompareTag("MDWT_R"))
            {
                OnTeleport(4);
                Debug.Log("TELEPORT");
            } else if(col.CompareTag("MDWT_EXIT"))
            {
                OnTeleport(5);
                Debug.Log("EXIT");
            }
        }
       

        if(life<=0)
        {
            OnDeath.Invoke();
        }


        //controller.SetInputVel(inputVel);


    }

    //private void OnGUI()
    //{
    //    GUIStyle s = new GUIStyle();
    //    s.fontSize = 30;
    //    s.fontStyle = FontStyle.Bold;

    //    s.normal.textColor = Color.white;
    //    GUI.color = Color.white;
    //    GUI.Label(new Rect(50, 50, 100, 50), "COLLISION: " + d.name);
    //    GUI.Label(new Rect(50, 100, 100, 50), "Y RAY DIST: " + DRY, s);

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Finish"))
    //    {
    //        OnWinMDW.Invoke();
    //        Debug.Log("<Color=Green> MDW WIN </Color>");
    //    }

    //    if (collision.CompareTag("Hazard"))
    //    {
    //        Debug.Log("<Color=Red> MDW OUCH! </Color>");
    //    }
    //}

}
