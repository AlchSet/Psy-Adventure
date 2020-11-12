using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    Controller2D controller;

    public Animator anim;

    public Telekinesis telekinesis;
    public PsychoBeam psychobeam;
    public PsychoSlash psychoSlash;
    public PsychicShield psyshield;

    public int spellMode = 0;

    public delegate void PlayerAction();

    public delegate void DialogueAction(Npc npc);

    Vector2 talk;

    Transform feet;

    public GameObject MessagePanel;

    public TextMeshProUGUI text;

    public bool isTalking;
    public bool isReady;
    bool isTeleporting;


    public TextMeshProUGUI spellNo;


    public Vector2 debugInput;

    public MessagePanelManager messagePanel;

    Npc npc;

    public Message currentMSG;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        feet = transform.Find("Bounds");

        spellNo.text = "" + spellMode;

        psychoSlash.onStartWpn = DisablePlayer;
        psychoSlash.onEndWpn = EnablePlayerDelayed;

    }

    // Update is called once per frame
    void Update()
    {
        //controller.SetInputVel(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));


        //psyshield.PlaceShield();
        if (!isTalking)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            debugInput = input;


            //Deadzone input for analog


            if (Mathf.Abs(input.x) < 0.4f)
            {
                input.x = 0;
            }
            if (Mathf.Abs(input.y) < 0.4f)
            {
                input.y = 0;
            }



            ////if(input.magnitude>0.5f)
            ////{

            ////}
            ////else
            ////{
            ////    input = Vector2.zero;
            ////}



            if (input.x != 0)
            {
                input.x = Mathf.Sign(input.x);
            }
            if (input.y != 0)
            {
                input.y = Mathf.Sign(input.y);
            }

            controller.SetInputVel(input);

            anim.SetFloat("X", controller.FacingDirection.x);
            anim.SetFloat("Y", controller.FacingDirection.y);

            if (Input.GetButtonDown("Jump"))
            {
                spellMode = (spellMode + 1) % 4;
                spellNo.text = "" + spellMode;
            }


            if (Input.GetKeyDown(KeyCode.C))
            {
                switch (spellMode)
                {
                    case 0:
                        //Telekinesis
                        telekinesis.Fire(EnablePlayer);
                        controller.SetInputVel(Vector2.zero);
                        this.enabled = false;

                        break;


                    case 1:
                        //Psychobeam
                        psychobeam.Fire();
                        break;

                    case 2:

                        psychoSlash.Fire();

                        break;

                    case 3:

                        psyshield.OnPress();
                        //psychoSlash.Fire();

                        break;

                }





            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                switch (spellMode)
                {
                    case 3:
                        psyshield.OnRelease();
                        break;
                }
            }

            //--OLD TALK SYSTEM
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    Vector2 talkPos = (Vector2)feet.position + controller.FacingDirection * 0.5f;
            //    talk = talkPos;
            //    Collider2D cols = Physics2D.OverlapCircle(talkPos, .25f, 1 << LayerMask.NameToLayer("Npc"));

            //    if (cols != null)
            //    {

            //        if (cols.tag == "Think")
            //        {
            //            Debug.Log("TALK to NPC");
            //            //MessagePanel.SetActive(true);
            //            isTalking = true;
            //            //Npc npc = cols.transform.parent.GetComponent<Npc>();
            //            //text.text = cols.GetComponent<Sign>().message[0];
            //            messagePanel.msgpane1.ShowMessage(cols.GetComponent<Sign>().message[0]);
            //        }
            //        else
            //        {

            //            Debug.Log("TALK to NPC");
            //            //MessagePanel.SetActive(true);
            //            isTalking = true;
            //            npc = cols.transform.parent.GetComponent<Npc>();
            //            //text.text = npc.dialogue.state.GetMessage()[0];


            //            //messagePanel.msgpane1.ShowMessage(npc.dialogue.state.GetMessage()[0]);

            //            npc.BeginDialogue(StartDialogue);

            //        }

            //    }
            //    //Debug.Log(cols);
            //}

        }
        else
        {
            //----OLD TALK SYSTEM
            //if (Input.GetKeyDown(KeyCode.E) && isReady)
            //{
            //    MessagePanel.SetActive(false);
            //    npc.EndDialogue(EndDialogue);
            //    //MessagePanel.SetActive(false);
            //    //isTalking = false;
            //    //isReady = false;
            //    //controller.ResumeMovement();
            //}
        }



    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Teleport")
        {
            Debug.Log("Teleport");

            StartCoroutine(TeleportPlayer(collision.GetComponent<Teleport>()));
        }
    }


    IEnumerator TeleportPlayer(Teleport tele)
    {
        switch (tele.roof)
        {
            case Teleport.RoofStates.INDOOR:
                gameObject.layer = LayerMask.NameToLayer("PlayerIndoors");
                break;

            case Teleport.RoofStates.OUTDOOR:

                gameObject.layer = LayerMask.NameToLayer("Player");
                break;
        }

        transform.position = tele.GetDestination();
        transform.position = controller.pixCam.RoundToPixel(transform.position);
        
        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(talk, 0.25f);
    }

    public void EnablePlayer()
    {
        Debug.Log("ENABLE P");

        this.enabled = true;
        controller.ResumeMovement();
    }



    public void EnablePlayerDelayed()
    {
        //this.enabled = true;
        StartCoroutine(DelayedResume());
    }

    public void DisablePlayer()
    {
        controller.HaltInputAndMovement();
        Debug.Log("DISABLE P");
        this.enabled = false;
    }


    public void UpdateDirection(float X, float Y)
    {
        controller.FacingDirection.x = X;
        controller.FacingDirection.y = Y;
        anim.SetFloat("X", controller.FacingDirection.x);
        anim.SetFloat("Y", controller.FacingDirection.y);
    }

    public void SendMSGtoPlayer(string msg)
    {
        isTalking = true;
        isReady = true;
        messagePanel.msgpane1.ShowMessage(msg);
        controller.HaltInputAndMovement();
    }


    public void StartDialogue()
    {
        Debug.Log("TALK to NPC");
        //MessagePanel.SetActive(true);
        isTalking = true;
        isReady = true;
        //npc = cols.transform.parent.GetComponent<Npc>();
        //text.text = npc.dialogue.state.GetMessage()[0];
        messagePanel.msgpane1.ShowMessage(npc.dialogue.state.GetMessage()[0]);
    }



    public void StartDialogue(Npc n)
    {
        npc = n;
        Debug.Log("TALK to NPC");
        //MessagePanel.SetActive(true);
        isTalking = true;
        isReady = true;
        //npc = cols.transform.parent.GetComponent<Npc>();
        //text.text = npc.dialogue.state.GetMessage()[0];
        messagePanel.msgpane1.ShowMessage(npc.dialogue.state.GetMessage()[0]);
    }









    public void EndDialogue()
    {
        MessagePanel.SetActive(false);
        isTalking = false;
        isReady = false;
        controller.ResumeMovement();

    }

    IEnumerator DelayedResume()
    {
        this.enabled = true;
        yield return new WaitForSeconds(0.119f);
        if (this.enabled == true)
        {
            controller.ResumeMovement();
        }

    }



    //NEW TALK SYSTEM

    public void StartDialogue(Message m)
    {
        if (!isTalking&&!isReady)
        {
            isTalking = true;
            currentMSG = m;
            controller.SetInputVel(Vector2.zero);
            if (currentMSG.OnTalk != null)
                currentMSG.OnTalk.Invoke();
            StartCoroutine(MessageStartDelay(currentMSG.onStartDelay));
        }
    



    }

    public bool FinishDialogue()
    {
        if (isReady && isTalking)
        {
            isTalking = false;
            MessagePanel.SetActive(false);
            if (currentMSG.OnEndTalk != null)
                currentMSG.OnEndTalk.Invoke();
            StartCoroutine(MessageEndDelay(currentMSG.onEndDelay));
            return true;
        }
        else
        {
            return false;
        }
       
    }

    IEnumerator MessageStartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isReady = true;
        messagePanel.msgpane1.ShowMessage(currentMSG.message);
    }
    IEnumerator MessageEndDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //isReady = true;
        //messagePanel.msgpane1.ShowMessage(currentMSG.message);
        isTalking = false;
        isReady = false;
        controller.ResumeMovement();
    }

    public class Message
    {
        public UnityEvent OnTalk;
        public UnityEvent OnEndTalk;
        public string message;
        public float onStartDelay;
        public float onEndDelay;


        public Message(string message, UnityEvent onTalk, UnityEvent onEndTalk, float startDelay, float endDelay)
        {
            this.message = message;
            this.OnTalk = onTalk;
            this.OnEndTalk = onEndTalk;
            onStartDelay = startDelay;
            onEndDelay = endDelay;
        }

    }

}
