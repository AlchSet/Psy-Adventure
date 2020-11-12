using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueState : StateMachineBehaviour
{
    [TextArea]
    public string[] message;

    DialogueTreeContrainer container;

    public bool next;
    Animator anim;

    public int Code;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        anim = animator;
        animator.transform.GetComponent<DialogueTreeContrainer>().state = this;

        //if(next)
        //{
        //    animator.SetTrigger("Continue");
        //}
    }


    public string[] GetMessage()
    {
        string[] m = message;
        if (next)
        {
            anim.SetTrigger("Continue");
        }
        return m;

    }


    public MessageAndSignal GetMessageAndSignal()
    {

        MessageAndSignal m = new MessageAndSignal();
        m.message = message;
        m.code = Code;
        if (next)
        {
            anim.SetTrigger("Continue");
        }
        return m;
    }

}


[System.Serializable]
public class MessageAndSignal
{
    public string[] message;
    public int code;
}
