using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractiveObject : MonoBehaviour
{
    public bool isReady;
    public bool inRange;

    public UnityEvent deselectEvent;


    public abstract void Interact();

    public abstract void ExitInteract();


    public virtual void Select(UnityEvent e)
    {
        deselectEvent = e;
    }

    public virtual bool GetIsReady()
    {
        return inRange && isReady;
    }

    /// <summary>
    /// Checks the distance between the interaction and p and sets the results to inRange, 
    /// returns a 0 or 1 to be used by for icons in PsychicMouse.
    /// </summary>
    /// <param name="p">Compare distance with</param>
    /// <returns>Interaction type 0,1,2,3</returns>
    public virtual int GetInteractiveTypeByDistance(Vector2 p)
    {
        float d = Vector2.Distance(transform.position, p);

        if (d <= 3)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }


        return inRange ? 0 : 1;
    }
}
