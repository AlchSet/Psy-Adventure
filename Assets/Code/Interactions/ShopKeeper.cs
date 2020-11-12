using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : InteractiveObject
{
    public GameObject shopMenu;
    public GameManager manager;


    public override void ExitInteract()
    {
        throw new System.NotImplementedException();
    }



    public override void Interact()
    {
        manager.gameSpeed = 0;
        Debug.Log("OPEN SHOP");
        shopMenu.SetActive(true);
        deselectEvent.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        isReady = true;
        manager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
