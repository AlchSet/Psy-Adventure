using MyStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class CoffeeShopJob : BaseStateMachine
{
    public const int HASWORKED_PARAM = 0;
    public const int HOUR_PARAM = 1;
    public const int DAY_PARAM = 2;
    GameManager manager;

    int day;

    public bool loaded;
    // Start is called before the first frame update
    void Start()
    {

        states.Clear();
        Debug.Log("DERPADERPA");
        manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

        //Define States
        IDLE_STATE sleep = new IDLE_STATE();
        sleep.name = "SLEEP";


        SETSLEEP_STATE awaken = new SETSLEEP_STATE();
        awaken.name = "WAKEY WAKEY";
        awaken.value = false;

        WALKROUTE_STATE goWork = new WALKROUTE_STATE();
        goWork.name = "Goto Work";

        WALKROUTE_STATE goHome = new WALKROUTE_STATE();
        goHome.name = "GO HOME";

        SETSLEEP_STATE FallAsleep = new SETSLEEP_STATE();
        FallAsleep.name = "SLEEPY";
        FallAsleep.value = true;

        TOGGLEOBJECT_STATE openShop = new TOGGLEOBJECT_STATE();
        openShop.name = "OPEN SHOP";

        TOGGLEOBJECT_STATE closeShop = new TOGGLEOBJECT_STATE();
        closeShop.name = "CLOSE SHOP";


        //SELECT THE SHOP OBJECT
        openShop.AddSelectedObjects(new int[] { 0 });
        closeShop.AddSelectedObjects(new int[] { 0 });




        //SELECT ROUTE HOME
        goWork.selectedRoute = 0;
        goHome.selectedRoute = 1;

        //Define Transitions
        Transition sleepToAwaken_T = new Transition();

        Transition AwakenToWork_T = new Transition();

        Transition workToOpenShop_T = new Transition();


        Transition openShopToCloseShop_T = new Transition();


        Transition closeShopToHome_T = new Transition();

        Transition HomeToFallAsleep_T = new Transition();

        Transition FallAsleepToSleep_T = new Transition();


        //Check if Hour>5 (START WORK AT 6AM)
        Condition checkWorkTime_CON = new Condition();
        checkWorkTime_CON.p = parameters[HOUR_PARAM];
        checkWorkTime_CON.type = Condition.ConditionTypes.GREATER;
        checkWorkTime_CON.intValue = 5;

        //Check if I already worked today
        Condition checkIfWorkedToday_CON = new Condition();
        checkIfWorkedToday_CON.p = parameters[HASWORKED_PARAM];
        checkIfWorkedToday_CON.type = Condition.ConditionTypes.FALSE;

        //Check if Sunday
        Condition checkifNotSunday_CON = new Condition();
        checkifNotSunday_CON.p = parameters[DAY_PARAM];
        checkifNotSunday_CON.type = Condition.ConditionTypes.NOTEQUAL;
        checkifNotSunday_CON.intValue = 6;

        //Check if Hour>15 (END WORK AT 4PM)
        Condition checkEndWorkTime_CON = new Condition();
        checkEndWorkTime_CON.p = parameters[HOUR_PARAM];
        checkEndWorkTime_CON.type = Condition.ConditionTypes.GREATER;
        checkEndWorkTime_CON.intValue = 15;




        //Prepare Transition
        sleepToAwaken_T.conditions.Add(checkWorkTime_CON);
        sleepToAwaken_T.conditions.Add(checkIfWorkedToday_CON);
        sleepToAwaken_T.conditions.Add(checkifNotSunday_CON);


        sleepToAwaken_T.nextState = awaken;

        AwakenToWork_T.nextState = goWork;



        workToOpenShop_T.nextState = openShop; 

        openShopToCloseShop_T.nextState = closeShop; 

        openShopToCloseShop_T.conditions.Add(checkEndWorkTime_CON);



        //closeShopToHome_T.conditions.Add(checkEndWorkTime_CON);
        closeShopToHome_T.nextState = goHome;

        HomeToFallAsleep_T.nextState = FallAsleep;

        FallAsleepToSleep_T.nextState = sleep;
        //Add Transition to State

        sleep.transitions.Add(sleepToAwaken_T);


        awaken.transitions.Add(AwakenToWork_T);

        goWork.transitions.Add(workToOpenShop_T);

        openShop.transitions.Add(openShopToCloseShop_T);

        closeShop.transitions.Add(closeShopToHome_T);



        //goWork.transitions.Add(openShopToHome_T);

        goHome.transitions.Add(HomeToFallAsleep_T);

        FallAsleep.transitions.Add(FallAsleepToSleep_T);




        //BEGIN

        sleep.Initialize(this);
        goWork.Initialize(this);
        openShop.Initialize(this);
        closeShop.Initialize(this);
        goHome.Initialize(this);
        awaken.Initialize(this);
        FallAsleep.Initialize(this);


        currentState = sleep;

        currentState.OnStateEnter(this);


        states.Add(sleep);
        states.Add(goWork);
        states.Add(openShop);
        states.Add(closeShop);
        states.Add(goHome);
        states.Add(awaken);
        states.Add(FallAsleep);

        Debug.Log(states.Count);

    }

    public override void Update()
    {
        if (Application.IsPlaying(gameObject))
        {
            parameters[HOUR_PARAM].intType = (int)manager.hour;
            parameters[DAY_PARAM].intType = (int)manager.currentDate;
            if (manager.hour > 15)
            {
                parameters[HASWORKED_PARAM].boolType = true;
            }


            if ((int)manager.hour == 0)
            {
                Debug.Log("NEW DAY");
                parameters[HASWORKED_PARAM].boolType = false;
            }


            base.Update();
        }
        else
        {

            if (!loaded)
            {
                states.Clear();
                Debug.Log("DERPADERPA");
                manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

                //Define States
                IDLE_STATE sleep = new IDLE_STATE();
                sleep.name = "SLEEP";

                WALKROUTE_STATE goWork = new WALKROUTE_STATE();
                goWork.name = "Goto Work";

                WALKROUTE_STATE goHome = new WALKROUTE_STATE();
                goHome.name = "GO HOME";

                //SELECT ROUTE HOME
                goWork.selectedRoute = 0;
                goHome.selectedRoute = 1;

                //Define Transitions
                Transition sleepToWork_T = new Transition();

                Transition WorkToHome_T = new Transition();

                Transition HomeToSleep_T = new Transition();

                //Check if Hour>5 (START WORK AT 6AM)
                Condition checkWorkTime_CON = new Condition();
                checkWorkTime_CON.p = parameters[HOUR_PARAM];
                checkWorkTime_CON.type = Condition.ConditionTypes.GREATER;
                checkWorkTime_CON.intValue = 5;

                //Check if I already worked today
                Condition checkIfWorkedToday_CON = new Condition();
                checkIfWorkedToday_CON.p = parameters[HASWORKED_PARAM];
                checkIfWorkedToday_CON.type = Condition.ConditionTypes.FALSE;

                //Check if Sunday
                Condition checkifNotSunday_CON = new Condition();
                checkifNotSunday_CON.p = parameters[DAY_PARAM];
                checkifNotSunday_CON.type = Condition.ConditionTypes.NOTEQUAL;
                checkifNotSunday_CON.intValue = 6;

                //Check if Hour>15 (END WORK AT 4PM)
                Condition checkEndWorkTime_CON = new Condition();
                checkEndWorkTime_CON.p = parameters[HOUR_PARAM];
                checkEndWorkTime_CON.type = Condition.ConditionTypes.GREATER;
                checkEndWorkTime_CON.intValue = 15;




                //Prepare Transition
                sleepToWork_T.conditions.Add(checkWorkTime_CON);
                sleepToWork_T.conditions.Add(checkIfWorkedToday_CON);
                sleepToWork_T.conditions.Add(checkifNotSunday_CON);


                sleepToWork_T.nextState = goWork;


                WorkToHome_T.conditions.Add(checkEndWorkTime_CON);
                WorkToHome_T.nextState = goHome;

                HomeToSleep_T.nextState = sleep;

                //Add Transition to State

                sleep.transitions.Add(sleepToWork_T);

                goWork.transitions.Add(WorkToHome_T);

                goHome.transitions.Add(HomeToSleep_T);

                //BEGIN

                sleep.Initialize(this);
                goWork.Initialize(this);
                goHome.Initialize(this);

                currentState = sleep;

                currentState.OnStateEnter(this);


                states.Add(sleep);
                states.Add(goWork);
                states.Add(goHome);


                Debug.Log(states.Count);
                loaded = true;
            }

        }
    }



}
