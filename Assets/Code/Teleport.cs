using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{

    public enum RoofStates { OUTDOOR, INDOOR };

    public RoofStates roof;

    public Transform destination;
    public Vector2 offset;


    public UnityEvent OnTeleport;


    /// <summary>
    /// Returns the destination of the teleport. Invokes OnTeleport.
    /// </summary>
    /// <returns>Teleport Desination including offset</returns>
    public Vector2 GetDestination()
    {
        OnTeleport.Invoke();
        return (Vector2)destination.position + offset;
    }

    /// <summary>
    /// Returns the destination of the teleport without invoking events.
    /// </summary>
    /// <returns>Teleport Desination including offset</returns>
    public Vector2 GetDestinationNoEvents()
    {
        return (Vector2)destination.position + offset;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, (Vector2)destination.position + offset);
    }
}
