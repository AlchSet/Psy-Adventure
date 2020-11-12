using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleSwitch : MonoBehaviour
{
    public Transform player;
    public Transform destination;
    public Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.root;
    }

    // Update is called once per frame



    public void TeleportPlayer()
    {
        player.position = (Vector2)destination.position + offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, (Vector2)destination.position + offset);

    }
}
