using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRANSFORM_STATE : State
{
    public SpriteRenderer fromSprite;
    public SpriteRenderer toSprite;

    float elapsed;


    public GameObject hitbox;
    public bool toState;

    

    public override void Initialize(BaseStateMachine m)
    {
        done = false;
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(BaseStateMachine m)
    {
        hitbox.SetActive(toState);
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        elapsed = 0;
        done = false;
        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {
        toSprite.enabled =! toSprite.enabled;
        fromSprite.enabled =! fromSprite.enabled;

        elapsed += Time.deltaTime;

        if(elapsed>1.5f)
        {
            elapsed = 0;
            done = true;

            toSprite.enabled = true;
            fromSprite.enabled = false;
        }


        //throw new System.NotImplementedException();
    }

   
}
