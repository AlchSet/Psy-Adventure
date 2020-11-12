using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeShopJob2 : BaseStateMachine
{
    public Transform workStation;
    public Transform homeStation;

    public GameObject ShopMenu;

    public AreaDirectory areaDirectory;

    AiCore ai;
    Parameter InterruptParam;
    Parameter JudgeParam;

    Parameter HourParam;

    Parameter EnergyParam;
    Parameter lowEnergyParam;

    GameManager manager;

    bool lowEnergy;
    // Start is called before the first frame update
    public override void Start()
    {
        ai = GetComponentInParent<AiCore>();
        ai.OnStunned.AddListener(InterruptEvent);
        ai.OnExitStun.AddListener(EndInterruptEvent);

        manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

        Parameter workParam = new Parameter(Parameter.ParamTypes.BOOLEAN, "GOWORK");
        InterruptParam = new Parameter(Parameter.ParamTypes.TRIGGER, "STUN");
        JudgeParam = new Parameter(Parameter.ParamTypes.TRIGGER, "JUDGE");

        HourParam = new Parameter(Parameter.ParamTypes.INTEGER, "TIME");

        EnergyParam = new Parameter(Parameter.ParamTypes.FLOAT, "ENERGY");
        EnergyParam.floatType = ai.Energy;

        lowEnergyParam = new Parameter(Parameter.ParamTypes.TRIGGER, "LOWENERGY");

        parameters.Add(workParam);
        parameters.Add(InterruptParam);
        parameters.Add(JudgeParam);
        parameters.Add(HourParam);
        parameters.Add(EnergyParam);
        parameters.Add(lowEnergyParam);

        REST_STATE sleep = new REST_STATE();
        sleep.name = "SLEEP";
        sleep.multiplier = 6;

        WORK_STATE work = new WORK_STATE();
        work.name = "WORK";
        work.workStation = workStation;
        work.directory = areaDirectory;
        work.workArea = AreaDirectory.COFEESHOP_AREA;
        work.ShopMenu = ShopMenu;

        WORK_STATE home = new WORK_STATE();
        home.name = "HOME";
        home.workStation = homeStation;
        home.directory = areaDirectory;
        home.workArea = AreaDirectory.HOME_ROOM1_AREA;
               
        IDLE_STATE Interrupted = new IDLE_STATE();
        Interrupted.name = "INTERRUPTED";


        IDLE_STATE Judge = new IDLE_STATE();
        Judge.name = "JUDGE";


        REST_STATE Rest = new REST_STATE();
        Rest.name = "REST";
        Rest.searchBench = true;

        sleep.CreateTransition(work, new Condition(Condition.ConditionTypes.GREATER, HourParam, 6), new Condition(Condition.ConditionTypes.LESS, HourParam, 16));


        work.CreateTransition(home, new Condition(Condition.ConditionTypes.GREATER, HourParam, 15));

        home.CreateTransition(sleep, new Condition(Condition.ConditionTypes.LESS, HourParam, 1));

        Rest.CreateTransition(Judge, new Condition(Condition.ConditionTypes.GREATER, EnergyParam, 50f));

        //sleep.CreateTransition(work, new Condition(Condition.ConditionTypes.TRUE, workParam, true));

        Interrupted.CreateTransition(Judge, new Condition(Condition.ConditionTypes.TRUE, JudgeParam, true));

        Judge.CreateTransition(work, new Condition(Condition.ConditionTypes.GREATER, HourParam, 6), new Condition(Condition.ConditionTypes.LESS, HourParam, 17));
        Judge.CreateTransition(home, new Condition(Condition.ConditionTypes.GREATER, HourParam, 15));
        
        CreateImpulseTransition(Interrupted, new Condition(Condition.ConditionTypes.TRUE, InterruptParam, true));
        CreateImpulseTransition(Rest, new Condition(Condition.ConditionTypes.TRUE, lowEnergyParam, true));
        
        sleep.Initialize(this);
        work.Initialize(this);
        home.Initialize(this);
        Interrupted.Initialize(this);
        Judge.Initialize(this);
        Rest.Initialize(this);
        
        AddToStateList(sleep, work, home, Interrupted, Judge, Rest);

        currentState = sleep;

        base.Start();

    }


    public override void Update()
    {
        HourParam.intType = (int)manager.hour;
        EnergyParam.floatType = ai.Energy;

        if (ai.Energy<25&&!lowEnergy)
        {
            lowEnergyParam.TriggerType = true;
            lowEnergy = true;
        }

        if(ai.Energy>50)
        {
            lowEnergy = false;
        }

        //EnergyParam.floatType = ai.Energy;
        base.Update();
    }

    public void InterruptEvent()
    {
        InterruptParam.TriggerType = true;
        Debug.Log("<Color=Red>INTERRUPT EVENT</Color>");
    }

    public void EndInterruptEvent()
    {
        JudgeParam.TriggerType = true;
        Debug.Log("<Color=Red>INTERRUPT EVENT</Color>");
    }



}
