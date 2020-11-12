using PsyMouse;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PsychicMouse : MonoBehaviour
{
    const int CRS_DEFAULT = 0;
    const int CRS_CHATFAR = 1;
    const int CRS_CHAT = 2;
    const int CRS_TELEKINESIS = 3;
    const int CRS_INSPECT = 4;
    const int CRS_INSPECTFAR = 5;
    const int CRS_DEFAULTPSY = 6;
    const int CRS_PSYBEAM = 7;
    const int CRS_PSYSHIELD = 8;



    public GameObject MentalDefenseWindow;

    public Camera cam;
    PixelPerfectCamera pixcam;

    public GameObject mouse;

    //public Transform ingameMouse;


    public RectTransform myCursor;

    Animator cursorAnim;


    Player player;


    public LayerMask Mask;

    LayerMask normal = 0;
    LayerMask npc = 0;

    //Interactable selected;
    //bool selection;

    public Selection selection = new Selection();


    //public delegate void SelectEvent();


    public Vector2 mousePos;


    public int Mode = 1;

    Collider2D d;
    public Collider2D[] dd;

    public Sprite SP_basic;
    public Sprite SP_Telek;
    public Sprite SP_PsychoB;
    public Sprite SP_PsyShield;
    public Sprite SP_EnterMind;


    public Image ModeUI;


    public GameObject PsychoBullet;

    public PsychicShield PsyShield;


    public AudioSource psybeamSFX;

    SpringJoint2D spring;

    bool teleHold;

    GameManager manager;

    TelekinesisObject telekineticObject;
    bool inTelekinesis;

    public int TelekinesisUses = 10;
    public int PsyshotUses = 30;
    public int PsyshieldUses = 50;
    public int EnterMindUses = 5;

    /// <summary>
    /// Ui for showing number of uses of each power.
    /// </summary>
    public TextMeshProUGUI SpellusesUI;

    public TextMeshProUGUI SpellCostUI;



    bool inShield;

    IEnumerator shielddecay;

    string costS = "-1";

    float test = 100;

    public PsychoSlash Punch;


    // Start is called before the first frame update
    void Start()
    {
        shielddecay = UseShield();
        cursorAnim = myCursor.GetComponent<Animator>();
        Cursor.visible = false;
        player = GetComponent<Player>();


        normal = LayerMask.NameToLayer("Default");
        npc = LayerMask.NameToLayer("Npc");



        spring = transform.Find("Spring").GetComponent<SpringJoint2D>();
        //spring.transform.SetParent(null);
        spring.gameObject.SetActive(false);

        manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

        if (cam)
        {
            pixcam = cam.GetComponent<PixelPerfectCamera>();
        }

        SpellusesUI.text = "";
        SpellCostUI.text = "";


        Punch.onStartWpn = player.DisablePlayer;
        Punch.onEndWpn = player.EnablePlayerDelayed;
        //Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mpos.z = 0;
        //ingameMouse = GameObject.Instantiate(mouse, mpos, Quaternion.identity).transform;
    }



    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        myCursor.position = Input.mousePosition;


        if (!selection.hasSelection)
        {
            //Change Mouse Power
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {//INSPECT-TALK-SHORT RANGE TOGGLE
                Mode = 1;
                ModeUI.sprite = SP_basic;
                SpellusesUI.text = "";
                SpellCostUI.text = "";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {//TELEKINESIS
                Mode = 2;
                ModeUI.sprite = SP_Telek;
                SpellusesUI.text = "" + TelekinesisUses;
                SpellCostUI.text = "";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {//PSY SHOT
                Mode = 3;
                ModeUI.sprite = SP_PsychoB;
                SpellusesUI.text = "" + PsyshotUses;
                SpellCostUI.text = "";

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {//Psyshield
                ModeUI.sprite = SP_PsyShield;
                Mode = 4;
                SpellusesUI.text = "" + PsyshieldUses;
                SpellCostUI.text = "";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ModeUI.sprite = SP_EnterMind;
                Mode = 5;
                SpellusesUI.text = "" + EnterMindUses;
                SpellCostUI.text = "";
            }



        }


        switch (Mode)
        {


            case 1://ISPECT-TALK-TOGGLE-PICKUP

                CheckTelekinesis();


                //SCAN for NPC's and Signs

                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("PUNCH");
                    Punch.Fire();
                }



                if (!selection.hasSelection)
                {


                    //d = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.05f, 1 << LayerMask.NameToLayer("Npc")|1<<LayerMask.NameToLayer("InteractBlock"));

                    dd = Physics2D.OverlapCircleAll(mousePos, 0.05f, 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("InteractBlock"));


                    //if (d != null)
                    if (dd.Length > 0)
                    {


                        foreach (Collider2D t in dd)
                        {
                            if (t.gameObject.layer == LayerMask.NameToLayer("InteractBlock"))
                            {
                                Debug.Log("EXIT");
                                cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);
                                return;

                            }
                        }



                        //Interactable i = d.GetComponent<Interactable>();
                        //Interactable i = dd[0].GetComponent<Interactable>();
                        //Debug.Log(dd[0].name);

                        InteractiveObject i = dd[0].GetComponent<InteractiveObject>();
                        //int type = i.GetInteractType();
                        int type = i.GetInteractiveTypeByDistance(transform.position);
                        Debug.Log("TYPE=" + type);
                        switch (type)
                        {
                            case 0:
                                cursorAnim.SetFloat("CursorMode", CRS_INSPECT);
                                break;

                            case 1:
                                cursorAnim.SetFloat("CursorMode", CRS_INSPECTFAR);
                                break;

                            case 2:
                                cursorAnim.SetFloat("CursorMode", CRS_CHAT);
                                break;


                            case 3:
                                cursorAnim.SetFloat("CursorMode", CRS_CHATFAR);
                                break;



                            default:
                                cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);

                                break;
                        }

                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (type == 0 | type == 2)
                            {
                                if (i.GetIsReady())
                                {
                                    //Debug.Log("DO SHIT with " + i.name);
                                    selection.Select(i);
                                    selection.Process();
                                }

                            }

                        }




                    }
                    else
                    {
                        cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {

                        selection.Process();
                    }

                }

                break;


            case 2://TELEKINESIS


                //INTERACT WITH INTERACTABLES AT DISTANCE
                //==============================
                if (!selection.hasSelection && !inTelekinesis)
                {


                    //d = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.05f, 1 << LayerMask.NameToLayer("Npc")|1<<LayerMask.NameToLayer("InteractBlock"));

                    dd = Physics2D.OverlapCircleAll(mousePos, 0.05f, 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("InteractBlock"));


                    //if (d != null)
                    if (dd.Length > 0)
                    {


                        foreach (Collider2D t in dd)
                        {
                            if (t.gameObject.layer == LayerMask.NameToLayer("InteractBlock"))
                            {
                                Debug.Log("EXIT");
                                cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);
                                return;

                            }
                        }



                        //Interactable i = d.GetComponent<Interactable>();
                        //Interactable i = dd[0].GetComponent<Interactable>();
                        //Debug.Log(dd[0].name);

                        InteractiveObject i = dd[0].GetComponent<InteractiveObject>();
                        //int type = i.GetInteractType();

                        int type = i.GetInteractiveTypeByDistance(transform.position);
                        i.inRange = true;
                        Debug.Log("TYPE=" + type);
                        switch (type)
                        {
                            case 0:
                                cursorAnim.SetFloat("CursorMode", CRS_INSPECT);
                                SpellCostUI.text = costS;
                                break;

                            case 1:
                                cursorAnim.SetFloat("CursorMode", CRS_INSPECTFAR);
                                SpellCostUI.text = costS;
                                break;

                            case 2:
                                cursorAnim.SetFloat("CursorMode", CRS_CHAT);
                                SpellCostUI.text = "";

                                break;


                            case 3:
                                cursorAnim.SetFloat("CursorMode", CRS_CHATFAR);
                                SpellCostUI.text = "";

                                break;



                            default:
                                cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);
                                //SpellCostUI.text = "";

                                break;
                        }



                        if (Input.GetMouseButtonDown(0) && TelekinesisUses > 0)
                        {
                            if (type == 0 | type == 1)
                            {
                                if (i.GetIsReady())
                                {
                                    TelekinesisUses--;
                                    SpellusesUI.text = "" + TelekinesisUses;

                                    //Debug.Log("DO SHIT with " + i.name);
                                    selection.Select(i);
                                    selection.Process();
                                }

                            }

                        }




                    }
                    else
                    {
                        cursorAnim.SetFloat("CursorMode", CRS_DEFAULT);
                        SpellCostUI.text = "";

                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && !inTelekinesis && TelekinesisUses > 0)
                    {
                        //Debug.Log("AHAAAAA");
                        TelekinesisUses--;
                        SpellusesUI.text = "" + TelekinesisUses;
                        selection.Process();
                    }

                }









                //============================
                //SCAN for Objects with Telekinesis tag
                if (!inTelekinesis)
                {
                    dd = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.05f, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("InteractBlock") | 1 << LayerMask.NameToLayer("Bounds"));

                    telekineticObject = null;
                    foreach (Collider2D t in dd)
                    {
                        if (t.gameObject.layer == LayerMask.NameToLayer("InteractBlock"))
                        {
                            cursorAnim.SetFloat("CursorMode", CRS_DEFAULTPSY);
                            return;
                        }

                        if (t.gameObject.CompareTag("Telekinesis"))
                        {
                            //Debug.Log("TELEK");
                            telekineticObject = t.GetComponent<TelekinesisObject>();
                        }

                        if (t.gameObject.CompareTag("Citizen"))
                        {
                            telekineticObject = t.GetComponentInParent<TelekinesisObject>();

                        }


                    }

                    if (telekineticObject != null)
                    {
                        SpellCostUI.text = costS;

                        cursorAnim.SetFloat("CursorMode", CRS_TELEKINESIS);

                        if (Input.GetMouseButtonDown(0) && TelekinesisUses > 0)
                        {
                            Debug.Log("TELEK");
                            telekineticObject.Interact();
                            inTelekinesis = true;
                        }


                    }
                    else
                    {
                        //SpellCostUI.text = "";
                        cursorAnim.SetFloat("CursorMode", CRS_DEFAULTPSY);
                    }

                }
                else
                {
                    SpellCostUI.text = "";
                    if (Input.GetMouseButtonDown(0) && telekineticObject.ready)
                    {
                        //SpellCostUI.text = "";
                        telekineticObject.OnMouseButton1Down();
                    }

                    if (Input.GetMouseButtonUp(0) && TelekinesisUses > 0)
                    {
                        TelekinesisUses--;
                        SpellusesUI.text = "" + TelekinesisUses;
                        telekineticObject.OnMouseButton1Up();
                        if (telekineticObject.thrown)
                        {
                            inTelekinesis = false;
                        }
                    }


                    if (Input.GetMouseButtonDown(1) && telekineticObject.ready)
                    {
                        //Debug.Log("EXIT TEL "+ telekineticObject.GetIsReady());
                        telekineticObject.ExitInteract();
                        inTelekinesis = false;
                    }


                    //IN TELEKINESIS
                    //spring.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(spring.transform.position, mousePos //**+ new Vector2(.15f,-1)**/
                    //        , Time.deltaTime * 15));



                }


                break;

            case 3://PSYBEAM

                CheckTelekinesis();

                cursorAnim.SetFloat("CursorMode", CRS_PSYBEAM);

                Vector2 offs = new Vector2(0.4f, -0.4f);

                if (Input.GetMouseButtonDown(0) && PsyshotUses > 0)
                {
                    PsyshotUses--;
                    SpellusesUI.text = "" + PsyshotUses;
                    Vector2 a = (mousePos + offs) - (Vector2)transform.position;
                    float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;

                    GameObject g = Instantiate(PsychoBullet, transform.position, Quaternion.Euler(0, 0, angle));

                    g.GetComponent<PsychobeamProjectile>().body.AddForce(a.normalized * 8, ForceMode2D.Impulse);

                    psybeamSFX.PlayOneShot(psybeamSFX.clip);


                }




                break;


            case 4://PSYSHIELD

                CheckTelekinesis();


                cursorAnim.SetFloat("CursorMode", CRS_PSYSHIELD);

                Vector2 p = mousePos - (Vector2)transform.position;

                PsyShield.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg);

                //IEnumerator shieldUseDecay=UseShield();

                //Coroutine co = null;

                if (Input.GetMouseButtonDown(0) && PsyshieldUses > 0)
                {
                    PsyshieldUses--;
                    SpellusesUI.text = "" + PsyshieldUses;

                    PsyShield.OnPress();
                    inShield = true;
                    StartCoroutine(shielddecay);
                    //selection.hasSelection = true;

                }


                if (Input.GetMouseButtonUp(0))
                {
                    inShield = false;
                    PsyShield.OnRelease();
                    inShield = false;
                    StopCoroutine(shielddecay);
                    //selection.hasSelection = false;
                }




                break;


            case 5://ENTERMIND

                CheckTelekinesis();

                dd = Physics2D.OverlapCircleAll(mousePos, 0.05f, 1 << LayerMask.NameToLayer("Bounds"));
                Transform target = null;

                //bool b = false;



                if (dd.Length > 0)
                {
                    if (dd[0].CompareTag("Citizen"))
                    {
                        SpellCostUI.text = costS;
                        Debug.Log("<Color=Purple>I SEE A CITIZEN</Color>");
                        target = dd[0].transform;
                    }
                    else
                    {
                        SpellCostUI.text = "";
                    }
                }
                else
                {
                    SpellCostUI.text = "";

                }


                if (Input.GetMouseButtonDown(0) && target && EnterMindUses > 0)
                {
                    EnterMindUses--;
                    SpellusesUI.text = "" + EnterMindUses;
                    Debug.Log("ENTER MIND");
                    EnterMind(target);

                    MentalDefenseWindow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));

                    MentalDefenseWindow.transform.position = pixcam.RoundToPixel(MentalDefenseWindow.transform.position);

                    MentalDefenseWindow.SetActive(true);
                }


                break;

        }





    }


    /// <summary>
    /// Cancels Telekinesis if already in use.
    /// </summary>
    public void CheckTelekinesis()
    {
        if (inTelekinesis)
        {
            Debug.Log("CANCEL TEL");

            telekineticObject.Abort();
            inTelekinesis = false;
        }
    }


    public void ReceiveIncommingMessage(Player.Message m)
    {
        IncommingMessage msg = new IncommingMessage(m);
        //msg.Select(selection.Deselect);
        selection.Select(msg);
        selection.Process();
    }

    /// <summary>
    /// Attaches an object to the spring when using Telekinesis.
    /// </summary>
    public void AttachTelekinesis(Transform t, Rigidbody2D b)
    {
        spring.transform.position = mousePos;
        spring.anchor = Vector2.zero;
        spring.connectedAnchor = t.InverseTransformPoint(mousePos);
        spring.connectedBody = b;
        spring.enabled = true;
        spring.gameObject.SetActive(true);
    }
    /// <summary>
    /// Deactivates the spring and stops Telekinesis.
    /// </summary>
    public void DetachTelekinesis()
    {
        spring.enabled = false;
        spring.gameObject.SetActive(false);
        teleHold = false;
        spring.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
    /// <summary>
    /// Freezes the Telekinetic Object. Called when about to toss the object.
    /// </summary>
    public void StopTelemove()
    {
        teleHold = true;
        spring.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
    }
    /// <summary>
    /// Moves the spring object when using “specialmode” telekinesis.
    /// </summary>
    public void MoveSpring()
    {
        spring.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(spring.transform.position, mousePos //**+ new Vector2(.15f,-1)**/
                , Time.deltaTime * 15));
    }
    /// <summary>
    /// So far just freezes game time.
    /// </summary>
    public void EnterMind(Transform victim)
    {
        manager.gameSpeed = 0;

    }


    IEnumerator UseShield()
    {

        float counter = 0;

        while (true)
        {
            if (PsyshieldUses <= 0)
            {
                inShield = false;
                PsyShield.OnRelease();
                inShield = false;
                break;
            }

            counter += Time.deltaTime;

            if (counter >= 1)
            {
                PsyshieldUses--;
                SpellusesUI.text = "" + PsyshieldUses;
                counter = 0;
            }


            yield return null;
        }
    }


    [System.Serializable]
    public class Selection
    {
        public InteractiveObject selected;

        public bool hasSelection = false;

        public void Process()
        {

            selected.Interact();
        }

        public void Select(InteractiveObject s)
        {
            selected = s;
            UnityEvent e = new UnityEvent();
            e.AddListener(Deselect);

            s.Select(e);
            hasSelection = true;
        }
        public void Deselect()
        {
            hasSelection = false;
        }

    }


    public class IncommingMessage : InteractiveObject
    {
        Player.Message message;
        Player p;
        //SelectEvent endSelect;

        public IncommingMessage(Player.Message m)
        {
            p = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();

            message = m;

        }



        public override void ExitInteract()
        {
            throw new System.NotImplementedException();
        }

        public int GetInteractType()
        {
            throw new System.NotImplementedException();
        }


        public override void Interact()
        {
            throw new System.NotImplementedException();
        }

    }
}



