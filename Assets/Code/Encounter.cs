using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : MonoBehaviour
{

    public GameObject bounds;
    AudioSource sfx;

    public UnityEvent OnEncounter;
    // Start is called before the first frame update
    void Start()
    {
        bounds = transform.Find("Bounds").gameObject;
        bounds.SetActive(false);
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            bounds.SetActive(true);
            sfx.PlayOneShot(sfx.clip);
            OnEncounter.Invoke();
        }

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{

        //
        //}
    }
}
