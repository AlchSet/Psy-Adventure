using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public const float TOTALHOURS = 24;

    public float currentTime;



    [Range(0f, 20.0f)]
    public float gameSpeed = 1;

    [Range(0, 6)]
    public int currentDate = 0;


    public float dayDuration;


    public float hour;
    public float minute;
    public float hourSlice;

    public Watch watch;

    public Gradient daylight;

    public Light sun;





    [Range(0, 20)]
    public float a;
    [Range(0, 20)]
    public float b;
    [Range(0, 20)]
    public float c;

    public int money = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnValidate()
    {
        //Calculate hour and minutes
        hourSlice = dayDuration / TOTALHOURS;
        hour = currentTime / hourSlice;
        minute = (hour - (int)hour) * 60;
        //Update Time UI
        watch.DEBUGSETHOUR((int)hour, (int)minute);
        watch.DEBUGSETDAY(currentDate);
    }
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;



        hourSlice = dayDuration / TOTALHOURS;


        currentTime += Time.deltaTime;

        if (currentTime >= dayDuration)
        {
            currentDate = (currentDate + 1) % 7;
            watch.SetDay(currentDate);
            currentTime = currentTime % dayDuration;
        }




        hour = currentTime / hourSlice;

        minute = (hour - (int)hour) * 60;


        if (sun)
        {
            float i = currentTime / dayDuration;

            sun.color = daylight.Evaluate(i);


        }


        //minute = Mathf.Ceil(minute / 10)%6;
        //minute = minute * 10;
        if (watch)
        {
            watch.SetHour((int)hour, (int)minute);
        }


        float r = Mathf.InverseLerp(a, b, c);
        //Debug.Log(r);
    }


    public void ResetTimeSpeed()
    {
        gameSpeed = 1;
    }

    public void SubtractMoney(int c)
    {
        money -= c;
    }

    public string GetFormatedTime()
    {
        string time = "";

        if (hour < 10)
        {
            time += "0" + (int)hour;
        }
        else
        {
            time += (int)hour;
        }
        time += ":";
        if (minute < 10)
        {
            time += "0" + (int)minute;
        }
        else
        {
            time += (int)minute;
        }
        return time;
    }
}
