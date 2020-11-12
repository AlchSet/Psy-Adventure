using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedCitizenA : BaseStateMachine
{
    Parameter detect;
    public Parameter life;
    bool detectSensor;

    public AiCore victim;
    public Sensor sensor;


    public TRANSFORM_STATE tranformToDemon;

    public TRANSFORM_STATE returnBackToNormal;

    public TRANSFORM_STATE deathReturnNormalForm;


    // Start is called before the first frame update
    void Start()
    {


        parameters.Clear();

        detect = new Parameter(Parameter.ParamTypes.BOOLEAN, "DetectPlayer");
        life = new Parameter(Parameter.ParamTypes.INTEGER, "Life");
        parameters.Add(detect);
        parameters.Add(life);





        //States


        IDLE_STATE waitForPlayer = new IDLE_STATE();
        waitForPlayer.name = "WaitForPlayer";


        tranformToDemon = new TRANSFORM_STATE();
        tranformToDemon.name = "BECOME DEMON";


        ATTACK_STATE attackPlayer = new ATTACK_STATE();
        attackPlayer.name = "AttackPlayer";


        FLEE_STATE fleeFromPlayer = new FLEE_STATE();
        fleeFromPlayer.name = "FLEE";


        returnBackToNormal = new TRANSFORM_STATE();
        returnBackToNormal.name = "BECOME HUMAN";



        deathReturnNormalForm=new TRANSFORM_STATE();
        deathReturnNormalForm.name = "REVERT FORM";

        IDLE_STATE deadDemon = new IDLE_STATE();
        deadDemon.name = "DEAD";

        //Transitions

        Transition waitToTransform_T = new Transition();
        waitToTransform_T.nextState = tranformToDemon;


        Transition transformToAttack_T = new Transition();
        transformToAttack_T.nextState = attackPlayer;

        //Return back transition

        Transition AttackToNormalForm_T = new Transition();
        AttackToNormalForm_T.nextState = returnBackToNormal;


        Transition AttackToFlee_T = new Transition();
        AttackToFlee_T.nextState = fleeFromPlayer;



        Transition normalFormToWait_T = new Transition();
        normalFormToWait_T.nextState = waitForPlayer;

        Transition attackToRevertForm_T = new Transition();
        attackToRevertForm_T.nextState = deathReturnNormalForm;

        Transition fleeToReverForm_T = new Transition();
        fleeToReverForm_T = new Transition();
        fleeToReverForm_T.nextState = deathReturnNormalForm;

        Transition RevertFormToDeath_T = new Transition();
        RevertFormToDeath_T.nextState = deadDemon;



        //Condition

        Condition checkDetectPlayer_CON = new Condition(Condition.ConditionTypes.TRUE, detect);


        //PLUG CONDITIONS TO TRANSITIONS

        waitToTransform_T.conditions.Add(checkDetectPlayer_CON);

        waitForPlayer.transitions.Add(waitToTransform_T);


        tranformToDemon.transitions.Add(transformToAttack_T);

        //waitToAttack_T.conditions.Add(checkDetectPlayer_CON);

        //waitForPlayer.transitions.Add(waitToAttack_T);



        //-------------------

        Condition checkLostPlayer_CON = new Condition(Condition.ConditionTypes.FALSE, detect);

        AttackToNormalForm_T.conditions.Add(checkLostPlayer_CON);


        //normalFormToWait.conditions.Add(checkLostPlayer_CON);

        attackPlayer.transitions.Add(AttackToNormalForm_T);


        returnBackToNormal.transitions.Add(normalFormToWait_T);



        Condition checkLife_CON = new Condition(Condition.ConditionTypes.EQUAL, life);
        checkLife_CON.intValue = 0;


        attackToRevertForm_T.conditions.Add(checkLife_CON);

        attackPlayer.transitions.Add(attackToRevertForm_T);


        deathReturnNormalForm.transitions.Add(RevertFormToDeath_T);


        Condition LowLifeFlee_CON = new Condition(Condition.ConditionTypes.LESS, life);
        LowLifeFlee_CON.intValue = 2;


        AttackToFlee_T.conditions.Add(LowLifeFlee_CON);
        attackPlayer.transitions.Add(AttackToFlee_T);



        fleeToReverForm_T.conditions.Add(checkLife_CON);
        fleeFromPlayer.transitions.Add(fleeToReverForm_T);


        //Piece them together

        currentState = waitForPlayer;

        states.Add(waitForPlayer);
        states.Add(attackPlayer);
        states.Add(tranformToDemon);
        states.Add(returnBackToNormal);
        states.Add(fleeFromPlayer);
        states.Add(deathReturnNormalForm);
        states.Add(deadDemon);





        //parameters.Add()
    }


    public override void Update()
    {

        if (!detectSensor)
        {
            Debug.Log("<Color=blue>DONT SEE PLAYER</Color>");
            detect.boolType = false;
        }
        detectSensor = false;

        base.Update();
    }

    public void DetectPlayer()
    {
        Debug.Log("DetectPlayer");
        detect.boolType = true;
        detectSensor = true;
        victim.target = sensor.target.root;
    }


    public void InitializeNewBody()
    {
        foreach (State s in states)
        {
            s.Initialize(this);
        }
    }
}
