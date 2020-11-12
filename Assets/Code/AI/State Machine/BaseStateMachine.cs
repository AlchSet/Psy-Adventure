using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyStateMachine
{/// <summary>
 /// Base class for finite state machine logic, must derive from this class.
 ///   Utilizes Parameters to influence the state machine.
 ///   Located in the Ai’s hierarchy under with the idea of being able to contain multiple state machines each one for a specific job.
 ///   A BaseStateMachine contains State objects which contain Transition objects which contain Condition object which checks the value of a Parameter.
 ///   Impulses are transitions that can occur on any State, they are contained in a list and are checked before running the check and logic of the current State.
 ///   There is a debugger in the works called MyWindow which gives you a visual of how the state machine looks like and what is its current state.So far it only works in PlayMode and requires the use of Middle Mouse Button to refresh.
 /// </summary>
    public abstract class BaseStateMachine : MonoBehaviour
    {/// <summary>
     /// Contains a list of all states in this state machine, used for the debugger.
     /// </summary>
        public List<State> states = new List<State>();
        /// <summary>
        /// List of user defined parameters to influence the state machine.
        /// </summary>
        public List<Parameter> parameters = new List<Parameter>();
        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        public State currentState;
        /// <summary>
        /// Impulses are Transition objects that conditions can apply in any State.
        /// </summary>
        public List<Transition> ImpulseTransitions = new List<Transition>();


        /// <summary>
        /// Calls currentState OnStateEnter function.
        /// </summary>
        public virtual void Start()
        {
            currentState.OnStateEnter(this);
        }

        /// <summary>
        /// Runs the logic of currentState. Also checks its transitions and updates currentState if so.
        /// </summary>
        public virtual void Update()
        {
            if (currentState != null)
            {
                CheckImpulses();

                currentState.OnStateUpdate(this);
                currentState = currentState.CheckTransitions();
            }

        }


        /// <summary>
        /// Add states to the list to be viewed by the debugger
        /// </summary>
        /// <param name="sts"></param>
        public void AddToStateList(params State[] sts)
        {
            foreach (State s in sts)
            {
                states.Add(s);
            }
        }

        /// <summary>
        /// Create an "Impulse" transition to this state machine.
        /// </summary>
        /// <param name="nextState"></param>
        /// <param name="conditions"></param>
        public void CreateImpulseTransition(State nextState, params Condition[] conditions)
        {
            Transition transition = new Transition();
            transition.nextState = nextState;

            foreach (Condition c in conditions)
            {
                transition.conditions.Add(c);
            }


            ImpulseTransitions.Add(transition);
        }

        /// <summary>
        /// Check if any impulse condition has been met.
        /// </summary>
        public void CheckImpulses()
        {
            Transition tt = null;
            bool check = false;
            if (ImpulseTransitions.Count > 0)
            {
                foreach (Transition t in ImpulseTransitions)
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
                    currentState.OnStateExit(this);
                    tt.nextState.OnStateEnter(this);
                    currentState = tt.nextState;
                }
                else
                {
                    //return this;
                }



            }
            else
            {
                //return this;
            }
        }


    }

    /// <summary>
    /// All state scripts must derive from this class. Provides a basis for how state machine scripts should be structured. Uses Transition classes in a list to denote state movement.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Used to generate id numbers.
        /// </summary>
        public static int IDGIVER = 1000;
        /// <summary>
        /// A unique id. Useful for the debugger
        /// </summary>
        public int ID;
        /// <summary>
        /// A list of transitions which connect to other states in the state machine.
        /// </summary>
        public List<Transition> transitions = new List<Transition>();
        /// <summary>
        /// The state’s name. Used in the debugger.
        /// </summary>
        public string name;
        /// <summary>
        /// Experimental value for the state debugger.
        /// </summary>
        public Vector2 uiPosition;
        /// <summary>
        /// The state can only change to the next one if done is TRUE. By default it is set to TRUE.
        /// </summary>
        public bool done = true;
        /// <summary>
        /// Called once to load all the necessary components. It is recommended to cache m to localFSM.
        /// </summary>
        /// <param name="m">The local FSM</param>
        public abstract void Initialize(BaseStateMachine m);
        /// <summary>
        /// Called upon entering a state.
        /// </summary>
        /// <param name="m">The local FSM</param>
        public abstract void OnStateEnter(BaseStateMachine m);
        /// <summary>
        /// Called upon every frame.
        /// </summary>
        /// <param name="m">The local FSM</param>
        public abstract void OnStateUpdate(BaseStateMachine m);
        /// <summary>
        /// Called when moving to another state. Used for clean up e.g. speed variables.
        /// </summary>
        /// <param name="m">The local FSM</param>
        public abstract void OnStateExit(BaseStateMachine m);
        /// <summary>
        /// Reference to the state machine if initialized.
        /// </summary>
        public BaseStateMachine localFSM;
        /// <summary>
        /// Creates a new state and assigns it self a unique id number to ID from IDGIVER.
        /// </summary>
        public State()
        {
            ID = IDGIVER;
            State.IDGIVER++;

            //Debug.Log(ID);
        }
        /// <summary>
        /// Loops through all Transition objects in transitions and checks if their valid for change. If so return its nextState, else return its self.
        /// </summary>
        /// <returns>The next State</returns>
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

                if (check && done)
                {
                    OnStateExit(localFSM);
                    tt.nextState.OnStateEnter(localFSM);
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

        /// <summary>
        /// Create transition with conditions
        /// </summary>
        /// <param name="nextState">The destined state</param>
        /// <param name="condition">Conditions for this transition</param>
        public void CreateTransition(State nextState, params Condition[] conditions)
        {

            Transition transition = new Transition();
            transition.nextState = nextState;

            foreach (Condition c in conditions)
            {
                transition.conditions.Add(c);
            }


            transitions.Add(transition);


        }


    }

    /// <summary>
    /// For a state machine to change from one state to another it needs data about where to go.
    ///A State holds a list of transition with each transition holding data such as the next state and a list of conditions which is checked by the state’s CheckTransitions() command, if all conditions are met then the state machine will transition to the next state.If no conditions exist then change to next state unless it’s not done.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// The state which this transition will lead to.
        /// </summary>
        public State nextState;
        /// <summary>
        /// A list of conditions which is used to check if this transition is qualified.
        /// </summary>
        public List<Condition> conditions = new List<Condition>();

        /// <summary>
        /// Loops through all Condition objects in conditions and checks if their valid for change. If so return TRUE, else return FALSE. If no conditions exist in the list return TRUE.
        /// </summary>
        /// <returns>If True Condition Valid</returns>
        public bool CheckTransition()
        {
            if (conditions.ToArray().Length > 0)
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

    /// <summary>
    /// Used in both BaseStateMachine and Condition classes to influence the state of the state machine.
    ///Parameters can be of type Integer, Boolean, Float or Trigger and by changing its values we can signals changes through the conditions you’ve setted up in each transition of the current state.
    /// </summary>
    [System.Serializable]
    public class Parameter
    {/// <summary>
     /// INTEGER, BOOLEAN, FLOAT, TRIGGER
     /// </summary>
        public enum ParamTypes { INTEGER, BOOLEAN, FLOAT, TRIGGER }
        /// <summary>
        /// Denotes the parameter’s type.
        /// </summary>
        public ParamTypes type;
        /// <summary>
        /// The parameter’s name.
        /// </summary>
        public string name;
        /// <summary>
        /// The parameter’s value if BOOLEAN.
        /// </summary>
        public bool boolType;
        /// <summary>
        /// The parameter’s value if INTEGER.
        /// </summary>
        public int intType;
        /// <summary>
        /// The parameter’s value if FLOAT.
        /// </summary>
        public float floatType;
        /// <summary>
        /// The parameter’s value if TRIGGER, should be set to FALSE immediately after the check.
        /// </summary>
        public bool TriggerType;
        /// <summary>
        /// Initiates parameter class.
        /// </summary>
        public Parameter()
        {

        }
        /// <summary>
        /// Initiate parameter class values.
        /// </summary>
        /// <param name="t">Parameter type</param>
        /// <param name="name">Parameter name</param>
        public Parameter(ParamTypes t, string name)
        {
            type = t;
            this.name = name;
        }


    }
    /// <summary>
    /// Each Transition object may or may not have a list of conditions which signals change if TRUE.
    ///A condition have a reference to the states machines parameter and cross references its condition value with the parameters value and signal change if conditions are satisfied.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// EQUAL, LESS, GREATER, NOTEQUAL, TRUE, FALSE
        /// </summary>
        public enum ConditionTypes { EQUAL, LESS, GREATER, NOTEQUAL, TRUE, FALSE }
        /// <summary>
        /// Denotes the conditions checking type.
        /// </summary>
        public ConditionTypes type;
        /// <summary>
        /// The associated parameter which lies in the state machine.
        /// </summary>
        public Parameter p;
        /// <summary>
        /// The checked value if BOOLEAN. (TRUE, FALSE)
        /// </summary>
        public bool boolValue;
        /// <summary>
        /// The checked value if INTEGER.(EQUAL, NOTEQUAL, LESS, GREATER)
        /// </summary>
        public int intValue;
        /// <summary>
        /// The checked value if FLOAT.(LESS, GREATER)
        /// </summary>
        public float floatValue;
        /// <summary>
        /// The checked value if TRIGGER(N/A)
        /// </summary>
        public bool triggerValue;

        /// <summary>
        /// Initiates Condition class.
        /// </summary>
        public Condition()
        {

        }
        /// <summary>
        /// Initiate Condition class with parameters.
        /// </summary>
        /// <param name="t">Condition type</param>
        /// <param name="p">The associated parameter</param>
        public Condition(ConditionTypes t, Parameter p)
        {
            type = t;
            this.p = p;
        }
        /// <summary>
        /// Initiate Condition class with parameter, value is dependant on the parameter type.
        /// </summary>
        /// <param name="type">Type of Comparison check</param>
        /// <param name="p">Reference to the parameter</param>
        /// <param name="value">Value for the condition, type depends on parameter</param>
        public Condition(ConditionTypes type, Parameter p, object value)
        {
            this.type = type;
            this.p = p;
            switch (p.type)
            {
                case Parameter.ParamTypes.BOOLEAN:

                    boolValue = (bool)value;

                    break;

                case Parameter.ParamTypes.INTEGER:

                    intValue = (int)value;


                    break;


                case Parameter.ParamTypes.FLOAT:

                    floatValue = (float)value;

                    break;


                case Parameter.ParamTypes.TRIGGER:


                    break;


            }
        }
        /// <summary>
        /// Checks the condition by ParamTypes then by ConditionTypes. Returns true if the associated value is met.
        /// </summary>
        /// <returns>TRUE if valid</returns>
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
                        return p.intType == intValue ? true : false;
                    }
                    else if (type == ConditionTypes.LESS)
                    {
                        return p.intType < intValue ? true : false;
                    }
                    else if (type == ConditionTypes.GREATER)
                    {
                        return p.intType > intValue ? true : false;
                    }
                    else if (type == ConditionTypes.NOTEQUAL)
                    {
                        return p.intType != intValue ? true : false;
                    }
                    else
                    {
                        return false;
                    }






                case Parameter.ParamTypes.FLOAT:


                    if (type == ConditionTypes.LESS)
                    {
                        return p.floatType < floatValue ? true : false;
                    }
                    else if (type == ConditionTypes.GREATER)
                    {
                        return p.floatType > floatValue ? true : false;
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
