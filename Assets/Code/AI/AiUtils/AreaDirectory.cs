using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A gameobject with this script must be the root
/// </summary>
public class AreaDirectory : MonoBehaviour
{
    /// <summary>
    /// Predefined array locations for each area node
    /// </summary>
    public const int OUTSIDE_AREA = 0;
    public const int COFEESHOP_AREA = 1;
    public const int HOME_AREA = 2;
    public const int HOME_ROOM1_AREA = 3;

    /// <summary>
    /// The roots Area Data
    /// </summary>
    public AreaData coreArea;


    /// <summary>
    /// A list of all nodes in the tree
    /// </summary>
    public List<DirectoryNode> nodeData = new List<DirectoryNode>();

    /// <summary>
    /// Results for finding path between 2 nodes
    /// </summary>
    public DirectionQuery[] directions = new DirectionQuery[15];


    /// <summary>
    /// List of parent nodes for current node
    /// </summary>
    public DirectoryNode[] plotPathA = new DirectoryNode[10];


    /// <summary>
    /// List of parent nodes for destined node
    /// </summary>
    public DirectoryNode[] plotPathB = new DirectoryNode[10];



    private void Start()
    {
        //TESTING


        //GetDirections(0, 1, Vector2.zero, Vector2.zero); //OutDoor to Shop test
        //GetDirections(2, 1, Vector2.zero);    //House to Shop test
        //GetDirections(1, 3, Vector2.zero);    //From shop to House>Room

        //GetDirections(OUTSIDE_AREA, COFEESHOP_AREA, Vector2.zero); //OutDoor to Shop test

        //GetDirections(HOME_AREA, COFEESHOP_AREA, Vector2.zero);    //House to Shop test

        GetDirections(COFEESHOP_AREA, HOME_ROOM1_AREA, Vector2.zero);    //From shop to House>Room

    }

    /// <summary>
    /// Get the path from currentArea to destined area
    /// </summary>
    /// <param name="currentArea">Id to the current node</param>
    /// <param name="destinedArea">Id to the destined node</param>
    /// <param name="destinedPos">The position we want in the destined node</param>
    /// <returns>A list of positions to follow</returns>
    public DirectionQuery[] GetDirections(int currentArea, int destinedArea, Vector2 destinedPos)
    {
        //Reset the cached final path
        for (int i = 0; i < directions.Length; i++)
        {
            directions[i].flag = false;
        }


        //Reset the cahces for each side
        for (int i = 0; i < plotPathA.Length; i++)
        {
            plotPathA[i] = null;
        }
        for (int i = 0; i < plotPathB.Length; i++)
        {
            plotPathB[i] = null;
        }


        DirectoryNode nodeA = null;
        DirectoryNode nodeB = null;

        //Find the current and destined nodes in the list
        foreach (DirectoryNode n in nodeData)
        {
            if (n.area.ID == currentArea)
            {
                nodeA = n;

            }
            if (n.area.ID == destinedArea)
            {
                nodeB = n;

            }
        }


        DirectoryNode currentNode = nodeA;



        int ii = 0;

        //Iterate through all parents of nodeA
        while (true)
        {
            if (currentNode.parent.destination == null)
            {
                //ROOT NODE

                plotPathA[ii] = nodeData[OUTSIDE_AREA];

                break;
            }
            else
            {


                plotPathA[ii] = currentNode;

                currentNode = currentNode.parent.destination;
                ii++;
            }

        }

        currentNode = nodeB;

        ii = 0;
        //Iterate through all parents of nodeB
        while (true)
        {
            if (currentNode.parent.destination == null)
            {
                //Debug.LogError("ROOT NODE");

                plotPathB[ii] = nodeData[OUTSIDE_AREA];

                break;
            }
            else
            {

                //s += currentNode.parent.destination + ">";

                plotPathB[ii] = currentNode;

                currentNode = currentNode.parent.destination;
                ii++;
            }

        }



        int pA = 0;
        int pB = 0;


        //Check for common ancenstor and cache their locations in the array
        for (int x = 0; x < plotPathA.Length; x++)
        {
            if (plotPathA[x] == null)
            {
                continue;
            }
            for (int y = 0; y < plotPathB.Length; y++)
            {
                if (plotPathB[y] == null)
                {
                    continue;
                }

                if (plotPathA[x] == plotPathB[y])
                {
                    pA = x;
                    pB = y;

                    //Break out of here and go to MakePath
                    goto MakePath;

                }
            }
        }

        //Where the code escapes to
        MakePath:


        //Begin plotting the results, add points from first list until common parent
        int pathPlotter = 0;


        for (int x = 0; x < pA; x++)
        {
            directions[pathPlotter].areaName = plotPathA[x].area.areaName;
            directions[pathPlotter].location = plotPathA[x].area.OUT.transform.position;
            //directions[pathPlotter].OUTorIN = false;
            if (x != 0)
            {
                //Add the last's node teleports destination
                directions[pathPlotter].fromPos = plotPathA[x - 1].area.OUT.GetDestinationNoEvents();
            }
            directions[pathPlotter].flag = true;
            pathPlotter++;
        }

        //Debug.LogWarning("pA=" + pathPlotter);
        //Ommit the common parent and add points in the reverse order
        for (int y = pB - 1; y > -1; y--)
        {
            //Debug.Log("V-" + plotPathB[y].area.areaName);
            directions[pathPlotter].areaName = plotPathB[y].area.areaName;
            directions[pathPlotter].location = plotPathB[y].area.IN.transform.position;
            if (y == pB - 1)
            {
                if (pA != 0)
                    //Add the teleport's destination from the last entry of the first tree
                    directions[pathPlotter].fromPos = plotPathA[pA - 1].area.OUT.GetDestinationNoEvents();
            }
            else
            {
                //Add the last nodes telelport's destination
                directions[pathPlotter].fromPos = plotPathB[y + 1].area.IN.GetDestinationNoEvents();

            }
            directions[pathPlotter].flag = true;
            pathPlotter++;

        }
        //Attach the destined position at the end - index is 0 because its the last room before destination
        directions[pathPlotter].areaName = "destinedPos";
        directions[pathPlotter].location = destinedPos;
        directions[pathPlotter].fromPos = plotPathB[0].area.IN.GetDestinationNoEvents();
        directions[pathPlotter].flag = true;

        return directions;

    }
}

