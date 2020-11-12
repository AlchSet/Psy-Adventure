using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTransparent : MonoBehaviour
{
    public SpriteRenderer[] spriteR;
    Color[] oColor;
    Color[] transColor;

    // Start is called before the first frame update
    void Start()
    {
        oColor = new Color[spriteR.Length];
        transColor = new Color[spriteR.Length];

        int i = 0;
        foreach(SpriteRenderer r in spriteR)
        {
            oColor[i] = r.color;
            transColor[i] = oColor[i];
            transColor[i].a = 0.5f;
            i++;
        }


        //oColor = spriteR.color;
        //transColor = oColor;
        //transColor.a = 0.5f;
    }
    public void StartOcclude()
    {
        int i = 0;

        foreach(SpriteRenderer r in spriteR)
        {
            r.color = transColor[i];
            i++;
        }
    }

    public void StopOcclude()
    {
        int i = 0;

        foreach (SpriteRenderer r in spriteR)
        {
            r.color = oColor[i];
            i++;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player"))
        {        Debug.Log("GVHASHJKLJSDAJKDSK");
            //spriteR.color = transColor;
            StartOcclude();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //spriteR.color = oColor;
            StopOcclude();
        }
    }
}
