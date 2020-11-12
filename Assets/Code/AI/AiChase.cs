using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : StateMachineBehaviour
{
    AiCore core;

    public float ATKDistance = 2;

    public Transform p;

    bool triggered;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        triggered = false;
        if (core == null)
        {
            core = animator.GetComponent<AiCore>();

        }

        if(p==null)
        {
            p = GameObject.FindGameObjectWithTag("Player").transform.root;
        }

        core.anim.SetBool("Walk", true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(!triggered)
        {

            float d = Vector2.Distance(animator.transform.position, p.position);

            if (d <= ATKDistance)
            {
                animator.SetTrigger("Continue");
                triggered = true;
            }

        }
    }



}
