using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdle : StateMachineBehaviour
{

    AiCore core;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(core==null)
        {
            core = animator.GetComponent<AiCore>();

        }


        core.anim.SetBool("Walk", false);
        //Debug.Log(core.anim.ToString());

    }


}
