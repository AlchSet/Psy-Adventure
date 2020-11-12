using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttack : StateMachineBehaviour
{

    AiCore core;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (core == null)
        {
            core = animator.GetComponent<AiCore>();
        }

        core.anim.SetTrigger("Attack");

    }


}
