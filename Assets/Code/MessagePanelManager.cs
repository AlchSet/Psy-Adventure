using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessagePanelManager : MonoBehaviour
{
    public GameObject Panel1;

    public MessagePane msgpane1;


    // Start is called before the first frame update
    void Start()
    {
        if (Panel1)
        {
            msgpane1 = new MessagePane(Panel1, Panel1.transform.Find("MSGText").GetComponent<TextMeshProUGUI>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    [System.Serializable]
    public class MessagePane
    {
        public GameObject root;

        public TextMeshProUGUI text;
        public MessagePane(GameObject g, TextMeshProUGUI t)
        {
            root = g;
            text = t;
        }

        public void ShowMessage(string s)
        {
            Debug.Log("SHOW MSG");
            root.SetActive(true);
            text.text = s;
        }

        public void HideMessage()
        {
            root.SetActive(false);
        }

    }
}
