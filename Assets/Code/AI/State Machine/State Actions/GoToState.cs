using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyStateMachine
{
    public class GoToState : State
    {
        public override void Initialize(BaseStateMachine m)
        {
            //throw new System.NotImplementedException();
        }




        //public override void OnStateEnter(StateMachine m)
        //{

        //    Debug.Log(name + " ENTER");
        //}

        public override void OnStateEnter(BaseStateMachine m)
        {
            Debug.Log("ENTER " + name);
        }

        //public override void OnStateExit(StateMachine m)
        //{
        //    Debug.Log(name + " EXIT");
        //}

        public override void OnStateExit(BaseStateMachine m)
        {
            Debug.Log("EXIT " + name);
        }

        //public override void OnStateUpdate(StateMachine m)
        //{
        //    Debug.Log(name + " UPDATE");
        //}

        public override void OnStateUpdate(BaseStateMachine m)
        {
            Debug.Log("UPDATE " + name);
        }
    }
}