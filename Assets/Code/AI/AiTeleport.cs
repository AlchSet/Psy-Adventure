using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTeleport : StateMachineBehaviour
{
    public Vector2 Destination;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

        animator.transform.root.position = Destination;

    }
}
