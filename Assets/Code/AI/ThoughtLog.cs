using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtLog : MonoBehaviour
{
    [TextArea(1,2)]
    public List<string> thoughts = new List<string>();

    [TextArea(1, 2)]
    public string WakeUpMsg;
     [TextArea(1, 2)]
    public string SleepMsg;

    public AiCore ai;

    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

        ai.OnSleeping.AddListener(AddFallAsleepThought);
        ai.OnWakingUp.AddListener(AddWakeUpThought);
        ai.OnEnterArea.AddListener(EnterAreaThought);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWakeUpThought()
    {
        thoughts.Add("[" + manager.GetFormatedTime() + "] " +WakeUpMsg);
    }


    public void AddFallAsleepThought()
    {
        thoughts.Add("[" + manager.GetFormatedTime() + "] " + SleepMsg);
    }

    public void EnterAreaThought()
    {
        thoughts.Add("["+manager.GetFormatedTime()+"] Entered " + ai.currentArea.areaName);
    }

}
