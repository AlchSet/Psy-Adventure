using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : InteractiveObject
{

    [TextArea]
    public string[] message;
    Player player;
    //PsychicMouse.SelectEvent endS;
    //bool inRange;
    //public bool isReady=true;
    public float OnEndDelay = 0.1f;


    //public void ExitInteract()
    //{
    //    player.FinishDialogue();
    //}


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
        isReady = true;

    }


    //public int GetInteractType()
    //{
    //    float d = Vector2.Distance(transform.position, player.transform.position);

    //    if (d <= 5)
    //    {
    //        inRange = true;
    //    }
    //    else
    //    {
    //        inRange = false;
    //    }






    //    return inRange?0:1;
    //}

    //public bool GetIsReady()
    //{
    //    float d = Vector2.Distance(transform.position, player.transform.position);

    //    if (d <= 5)
    //    {
    //        inRange = true;
    //    }
    //    else
    //    {
    //        inRange = false;
    //    }

    //    return inRange && isReady;
    //}

    //public void Interact()
    //{
    //    if (!player.isTalking)
    //    {
    //        player.StartDialogue(new Player.Message(message[0], null, null, 0, 0));
    //        isReady = false;
    //    }
    //    else
    //    {
    //        player.FinishDialogue();
    //        endS();
    //        StartCoroutine(ReturnToReady());
    //    }

    //}





    /// <summary>
    /// Chat type. Returns 3 if too far, 2 if in range.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public override int GetInteractiveTypeByDistance(Vector2 p)
    {
        int i = base.GetInteractiveTypeByDistance(p);

        return i == 0 ? 2 : 3;

    }


    IEnumerator ReturnToReady()
    {
        yield return new WaitForSeconds(OnEndDelay);
        isReady = true;
    }

    public override void Interact()
    {
        if (!player.isTalking)
        {
            player.StartDialogue(new Player.Message(message[0], null, null, 0, 0));
            isReady = false;
        }
        else
        {
            player.FinishDialogue();
            deselectEvent.Invoke();
            StartCoroutine(ReturnToReady());
        }
    }

    public override void ExitInteract()
    {
        player.FinishDialogue();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
