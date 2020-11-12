using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyStateMachine
{
    [System.Obsolete("StateMachine is deprecated, please use BaseStateMachine instead.", true)]

    public class StateMachine : MonoBehaviour
    {

        public List<State> states = new List<State>();
        public List<Parameter> parameters = new List<Parameter>();

        public State currentState;
        // Start is called before the first frame update
        void Start()
        {


            //GoToState state1 = new GoToState();
            //state1.name = "State 1";


            //GoToState state2 = new GoToState();
            //state2.name = "STATE 2";


            //GoToState state3 = new GoToState();
            //state3.name = "STate 3";

            //GoToState state4 = new GoToState();
            //state4.name = "STate 4";


            //GoToState state5 = new GoToState();
            //state5.name = "STate 5";

            //GoToState state6 = new GoToState();
            //state6.name = "STATE 6";


            //Transition t1 = new Transition();

            //t1.nextState = state2;

            //Condition c1 = new Condition();
            //c1.p = parameters[0];
            //c1.type = Condition.ConditionTypes.TRUE;
            //t1.conditions.Add(c1);

            //Transition t2 = new Transition();
            //t2.nextState = state3;

            //Condition c2 = new Condition();
            //c2.p = parameters[2];
            //t2.conditions.Add(c2);


            //Transition t3 = new Transition();
            //t3.nextState = state4;

            //Condition c3 = new Condition();
            //c3.type = Condition.ConditionTypes.EQUAL;
            //c3.i = 4;
            //c3.p = parameters[1];
            //t3.conditions.Add(c3);


            //Transition t4 = new Transition();
            //t4.nextState = state5;

            //Condition c4 = new Condition();
            //c4.type = Condition.ConditionTypes.GREATER;
            //c4.f = 2.0f;
            //c4.p = parameters[3];
            //t4.conditions.Add(c4);


            //Transition t5= new Transition();
            //t5.nextState = state6;



            //state1.transitions.Add(t1);
            //state1.transitions.Add(t2);
            //state1.transitions.Add(t3);
            //state1.transitions.Add(t4);

            //state5.transitions.Add(t5);

            //currentState = state1;

            //currentState.OnStateEnter(this);



            //states.Add(state1);
            //states.Add(state2);
            //states.Add(state3);

            //states.Add(state4);
            //states.Add(state5);
            //states.Add(state6);
        }

        // Update is called once per frame
        void Update()
        {
            currentState.OnStateUpdate(this);
            currentState = currentState.CheckTransitions();

        }


        //void CheckNextState()
        //{
        //    if(currentState.CheckTransitions())
        //    {

        //        currentState.OnStateExit();

        //    }
        //}




        //[System.Serializable]
        public abstract class State
        {
            public static int IDGIVER = 1000;
            public int ID;
            public List<Transition> transitions = new List<Transition>();
            public string name;
            public abstract void OnStateEnter(StateMachine m);
            public abstract void OnStateUpdate(StateMachine m);
            public abstract void OnStateExit(StateMachine m);
            StateMachine m;



            public State()
            {
                ID = IDGIVER;
                State.IDGIVER++;

                //Debug.Log(ID);
            }

            public State CheckTransitions()
            {
                Transition tt = null;
                bool check = false;
                if (transitions.ToArray().Length > 0)
                {
                    foreach (Transition t in transitions)
                    {
                        if (t.CheckTransition())
                        {
                            tt = t;
                            check = true;
                            break;
                        }
                    }

                    if (check)
                    {
                        OnStateExit(m);
                        tt.nextState.OnStateEnter(m);
                        return tt.nextState;
                    }
                    else
                    {
                        return this;
                    }



                }
                else
                {
                    return this;
                }

            }


        }


        public class Transition
        {
            public State nextState;
            public List<Condition> conditions = new List<Condition>();



            public bool CheckTransition()
            {
                if(conditions.ToArray().Length>0)
                {
                    bool checker = true;

                    foreach (Condition c in conditions)
                    {
                        if (c.CheckCondition() == false)
                        {
                            checker = false;
                        }
                    }


                    return checker;
                }
                else
                {
                    return true;
                }

             
            }

        }


        [System.Serializable]
        public class Parameter
        {
            public enum ParamTypes { INTEGER, BOOLEAN, FLOAT, TRIGGER }

            public ParamTypes type;
            public string name;

            public bool boolType;
            public int intType;
            public float floatType;
            public bool TriggerType;

        }

        public class Condition
        {
            public enum ConditionTypes { EQUAL, LESS, GREATER, NOTEQUAL, TRUE, FALSE }

            public ConditionTypes type;
            public Parameter p;

            public bool b;
            public int i;
            public float f;
            public bool t;
            


            public bool CheckCondition()
            {

                switch (p.type)
                {
                    case Parameter.ParamTypes.BOOLEAN:

                        if (type == ConditionTypes.TRUE)
                        {
                            return p.boolType ? true : false;
                        }
                        else if (type == ConditionTypes.FALSE)
                        {
                            return p.boolType ? false : true;
                        }
                        else
                        {
                            return false;
                        }




                    case Parameter.ParamTypes.INTEGER:

                        if (type == ConditionTypes.EQUAL)
                        {
                            return p.intType == i ? true : false;
                        }
                        else if (type == ConditionTypes.LESS)
                        {
                            return p.intType < i ? true : false;
                        }
                        else if (type == ConditionTypes.GREATER)
                        {
                            return p.intType > i ? true : false;
                        }
                        else if (type == ConditionTypes.NOTEQUAL)
                        {
                            return p.intType != i ? true : false;
                        }
                        else
                        {
                            return false;
                        }






                    case Parameter.ParamTypes.FLOAT:


                        if (type == ConditionTypes.LESS)
                        {
                            return p.floatType < f ? true : false;
                        }
                        else if (type == ConditionTypes.GREATER)
                        {
                            return p.floatType > f ? true : false;
                        }
                        else
                        {

                            return false;
                        }




                    case Parameter.ParamTypes.TRIGGER:

                        if (p.TriggerType)
                        {
                            p.TriggerType = false;
                            return true;
                        }
                        else
                        {
                            return false;
                        }



                    default:

                        return false;


                }


            }


        }



    }
}