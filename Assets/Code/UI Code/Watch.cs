using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Watch : MonoBehaviour
{
    TextMeshProUGUI HR;
    TextMeshProUGUI MN;
    GameObject colon;
    TextMeshProUGUI DAY;

    string[] day = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

    // Start is called before the first frame update
    void Awake()
    {
        HR = transform.Find("HR").GetComponent<TextMeshProUGUI>();
        MN = transform.Find("MN").GetComponent<TextMeshProUGUI>();
        colon = transform.Find("Colon").gameObject;
        DAY= transform.Find("DAY").GetComponent<TextMeshProUGUI>();
        //StartCoroutine(BlinkColon());
    }



    public void SetHour(int hr,int mn)
    {
        if(hr<10)
        {
            HR.text = "0" + hr;
        }
        else
        {
            HR.text = "" + hr;
        }
        if(mn<10)
        {
            MN.text = "0" + mn;
        }
        else
        {
            MN.text = "" + mn;
        }
        
        
    }


    public void SetDay(int d)
    {
        DAY.text = day[d];
    }

    public void DEBUGSETDAY(int d)
    {
        transform.Find("DAY").GetComponent<TextMeshProUGUI>().text= day[d];
    }


    public void DEBUGSETHOUR(int hr, int mn)
    {
        HR = transform.Find("HR").GetComponent<TextMeshProUGUI>();
        MN = transform.Find("MN").GetComponent<TextMeshProUGUI>();

        if (hr < 10)
        {
            HR.text = "0" + hr;
        }
        else
        {
            HR.text = "" + hr;
        }
        if (mn < 10)
        {
            MN.text = "0" + mn;
        }
        else
        {
            MN.text = "" + mn;
        }
    }

   IEnumerator BlinkColon()
    {
        //yield return new WaitForSeconds(.5f);

        while (true)
        {
            colon.SetActive(!colon.activeSelf);
            yield return new WaitForSeconds(.5f);
        }
    }
}
