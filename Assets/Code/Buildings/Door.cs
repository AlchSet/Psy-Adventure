using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    AudioSource sfx;
    Collider2D collider;

    void Start()
    {
        sfx = GetComponent<AudioSource>();
        //player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
        collider = GetComponent<Collider2D>();
        isReady = true;
    }



    public override void ExitInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        Debug.Log("OPEN");
        sfx.PlayOneShot(sfx.clip);
        collider.enabled = false;
        deselectEvent.Invoke();
    }
}






//public class Door : MonoBehaviour, Interactable
//{

//    bool inRange;

//    Player player;

//    bool isReady = true;

//    PsychicMouse.SelectEvent endS;

//    AudioSource sfx;

//    Collider2D collider;

//    // Start is called before the first frame update
//    void Start()
//    {
//        sfx = GetComponent<AudioSource>();
//        player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<Player>();
//        collider = GetComponent<Collider2D>();
//    }



//    public void ExitInteract()
//    {
//        throw new System.NotImplementedException();
//    }

//    public int GetInteractType()
//    {
//        float d = Vector2.Distance(transform.position, player.transform.position);

//        if (d <= 5)
//        {
//            inRange = true;
//        }
//        else
//        {
//            inRange = false;
//        }


//        return inRange ? 0 : 1;
//    }

//    public bool GetIsReady()
//    {
//        return inRange && isReady;
//    }

//    public void Interact()
//    {
//        sfx.PlayOneShot(sfx.clip);
//        collider.enabled = false;
//        endS();
//    }

//    public void OnMouseButton1Down()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void OnMouseButton1Up()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void OnMouseButton2Down()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void OnMouseButton2Up()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Select(PsychicMouse.SelectEvent e)
//    {
//         endS=e;
//    }


//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
