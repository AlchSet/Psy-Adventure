using MyStateMachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WORK_STATE : State
{

    AiCore ai;

    public Transform workStation;

    public AreaDirectory directory;

    public int workArea;

    public int currentArea;

    public GameObject ShopMenu;

    DirectionQuery[] megaPath;

    bool isReady;

    bool processing;

    int index;

    int pathIndex;

    public List<Path> pathList = new List<Path>();

    bool hasSelectedPath;

    int frames;




    public override void Initialize(BaseStateMachine m)
    {
        this.localFSM = m;
        ai = m.transform.parent.GetComponent<AiCore>();
    }



    public override void OnStateEnter(BaseStateMachine m)
    {
        pathIndex = 0;
        isReady = false;
        if (!ai.currentArea)
        {
            currentArea = 0;
        }
        else
        {
            currentArea = ai.currentArea.ID;
        }

        megaPath = directory.GetDirections(currentArea, workArea, workStation.position);

        localFSM.StartCoroutine(PlotWorkPath());

        //ai.SetDestination(workStation.position);
        ai.OnFinishPath += Test;
        ai.OnTeleport += NextPath;
        ai.OnFinishPath += NextPath;

        //throw new System.NotImplementedException();
    }

    public override void OnStateExit(BaseStateMachine m)
    {
        ai.OnFinishPath -= Test;
        ai.OnTeleport -= NextPath;
        ai.OnFinishPath -= NextPath;

        pathList.Clear();

        hasSelectedPath = false;
        frames = 0;

        if (ShopMenu)
            ShopMenu.SetActive(false);

        //throw new System.NotImplementedException();
    }

    public override void OnStateUpdate(BaseStateMachine m)
    {

        //To Prevent bug when NextPath is called again after finishing path or teleport
        if (hasSelectedPath)
        {
            frames++;
            if (frames > 1)
            {
                frames = 0;
                hasSelectedPath = false;
            }
        }


        if (isReady)
        {
            //Debug.LogWarning("IM GOING TO WORK");
        }
        //throw new System.NotImplementedException();
    }


    public void Test()
    {
        Debug.Log("CALCULATED PATH");
    }


    IEnumerator PlotWorkPath()
    {
        index = 0;
        while (true)
        {
            if (!processing)
            {
                if (megaPath[index].flag == false)
                {
                    break;
                }

                if (index == 0)
                {
                    ai.seeker.StartPath(ai.transform.position, megaPath[index].location, OnPathFound);

                }
                else
                {
                    ai.seeker.StartPath(megaPath[index].fromPos, megaPath[index].location, OnPathFound);

                }
                processing = true;
            }



            yield return new WaitForEndOfFrame();
        }

        isReady = true;
        ai.SetPath(pathList[pathIndex]);

    }


    public void NextPath()
    {
        //Debug.Log("<Color=Yellow>NEXT PATH</Color>");

        if (!hasSelectedPath)
        {
            hasSelectedPath = true;
            pathIndex++;
        }

        
        
        if (pathIndex < pathList.Count)
        {
            ai.SetPath(pathList[pathIndex]);
        }
        else
        {
            Debug.Log("<Color=Yellow>IM AT WORK STATION</Color>");
            if (ShopMenu)
                ShopMenu.SetActive(true);

        }




    }

    public void OnPathFound(Path p)
    {
        if (!p.error)
        {
            Debug.LogWarning("PATH COMPLETE");
            pathList.Add(p);
            index++;
            processing = false;
        }
    }


}
