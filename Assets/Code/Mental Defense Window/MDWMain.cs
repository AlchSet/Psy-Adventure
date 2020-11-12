using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MDWMain : MonoBehaviour
{
    public List<GameObject> defenseObjects;
    public List<GameObject> mindscapeObjects;



    public MDWPlayer player;

    public Vector2 playerEntryPos;

    public ThoughtLog thoughts;

    public RectTransform thoughtLogUIContent;

    public List<TextMeshProUGUI> thoughtLogText;

    Vector2 oPos;

    GameManager gm;

    public Mindscape mindscape;

    public delegate void Action(int i);

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();

        player.OnWinMDW.AddListener(EnterMind);

        foreach (Transform t in thoughtLogUIContent)
        {
            thoughtLogText.Add(t.GetComponent<TextMeshProUGUI>());
        }

        oPos = player.transform.localPosition;

        player.OnHitHazard.AddListener(ResetPlayerPosition);
        player.OnDeath.AddListener(CloseMDW);
        player.OnTeleport += CheckTeleport;
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void EnterMind()
    {
        foreach (GameObject g in defenseObjects)
        {
            g.SetActive(false);
        }

        foreach (GameObject g in mindscapeObjects)
        {
            g.SetActive(true);
        }
        player.transform.position = transform.TransformPoint(playerEntryPos);



    }


    public void LoadThoughts()
    {
        foreach(TextMeshProUGUI t in thoughtLogText)
        {
            t.text = "";
        }



        int i = 0;
        foreach(string t in thoughts.thoughts)
        {
            thoughtLogText[i].text = t;
            i++;
        }

        if(i==0)
        {
            thoughtLogText[i].text = "THE MIND IS COMPLETELY EMPTY";
        }

    }


    public void ResetPlayerPosition()
    {
        player.transform.position = transform.TransformPoint(oPos);
    }


    public void CheckTeleport(int i)
    {
        Vector2 d = Vector2.zero;

        switch (i)
        { 
            case 1:
                mindscape.ScrollUp();
                d=player.transform.localPosition;
                d.y = -4.75f;
                player.transform.localPosition = d;
                break;

            case 2:
                mindscape.ScrollDown();
                d = player.transform.localPosition;
                d.y = 5.5f;
                player.transform.localPosition = d;
                break;

            case 3:
                mindscape.ScrollLeft();
                d = player.transform.localPosition;
                d.x = 6.1875f;
                player.transform.localPosition = d;
                break;


            case 4:
                mindscape.ScrollRight();
                d = player.transform.localPosition;
                d.x = -6.1875f;
                player.transform.localPosition = d;
                break;


            case 5:

                CloseMDW();
                break;
        }
    }

    public void CloseMDW()
    {
        gameObject.SetActive(false);
        gm.ResetTimeSpeed();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.TransformPoint(playerEntryPos), 0.5f);
    }


}
