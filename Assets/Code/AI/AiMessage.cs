using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMessage : StateMachineBehaviour
{

    Player player;
    PsychicMouse m;
    public MessageAndSignal message;
    Npc npc;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(npc==null)
        {
            npc = animator.GetComponent<Npc>();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
            m = player.transform.GetComponent<PsychicMouse>();
        }

        m.ReceiveIncommingMessage(new Player.Message(message.message[0], null, null, 1, 1));


        //player.StartDialogue(new Player.Message(message.message[0], null, null, 1, 1));

        //player.SendMSGtoPlayer(message.message[0]);
        //player.StartDialogue(npc);


    }



}
