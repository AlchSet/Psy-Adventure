using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Npc : InteractiveObject
{
    public DialogueTreeContrainer dialogue;

    public UnityEvent OnTalk;

    public UnityEvent OnEndTalk;

    public Player.DialogueAction dialogueAction;

    float dialogueDelay;

    public AnimationCurve curve;

    public float OnStartDelay;
    public float OnEndDelay;
    Player player;
    //PsychicMouse.SelectEvent endSelect;

    //public bool isReady = true;
    //public bool inRange = false;

    // Start is called before the first frame update
    void Awake()
    {
        dialogue = transform.Find("DialogueTree").GetComponent<DialogueTreeContrainer>();
        player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}



    public void BeginDialogue(Player.PlayerAction action)
    {
        OnTalk.Invoke();
        StartCoroutine(Dialoging(action));
        dialogueDelay = 0;
    }


    public void EndDialogue(Player.PlayerAction action)
    {
        OnEndTalk.Invoke();
        StartCoroutine(Dialoging(action));
        dialogueDelay = 0;
    }

    IEnumerator Dialoging(Player.PlayerAction action)
    {
        yield return new WaitForSeconds(dialogueDelay);
        action.Invoke();
    }



    public void DelayDialogye(float f)
    {
        dialogueDelay = f;
    }

   

    IEnumerator ReturnToReady()
    {
        yield return new WaitForSeconds(OnEndDelay);
        isReady = true;
    }

   

    /// <summary>
    /// Chat type. Returns 3 if too far, 2 if in range.
    /// </summary>
    /// <param name="p">Insert player distance</param>
    /// <returns>Returns interaction type</returns>
    public override int GetInteractiveTypeByDistance(Vector2 p)
    {
        int i = base.GetInteractiveTypeByDistance(p);

        return i == 0 ? 2 : 3;
    }

    /// <summary>
    /// Process Dialogue until its end, invoke deselectEvent when done.
    /// </summary>
    public override void Interact()
    {
        if (!player.isTalking)
        {
            isReady = false;
            player.StartDialogue(new Player.Message(dialogue.state.GetMessage()[0], OnTalk, OnEndTalk, OnStartDelay, OnEndDelay));

        }
        else
        {
            bool b = player.FinishDialogue();
            if (b)
            {
                deselectEvent.Invoke();
                StartCoroutine(ReturnToReady());
            }
        }
    }

    public override void ExitInteract()
    {
        player.FinishDialogue();
    }

    //public bool isInRange()
    //{
    //    throw new System.NotImplementedException();
    //}
}
