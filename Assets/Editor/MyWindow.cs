using UnityEditor;
using UnityEngine;

using System.Collections.Generic;
using MyStateMachine;

public class MyWindow : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    MyStateMachine.BaseStateMachine fsm;

    bool loaded;

    List<Node> nodes = new List<Node>();

    Dictionary<int, Node> mahdic = new Dictionary<int, Node>();

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MyWindow));
    }

    void OnGUI()
    {
        Handles.DrawAAPolyLine(new Vector3(50, 100), new Vector3(100, 100), new Vector3(100, 150), new Vector3(50, 100));

        //Texture2D t = Texture2D.whiteTexture;

        //GUI.color = Color.blue;
        //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), Texture2D.whiteTexture, ScaleMode.StretchToFill);

        //GUI.Label(new Rect(200, 200, 100, 100), "A label");
        //GUI.TextField(new Rect(20, 20, 70, 30), "");


        //Debug.Log(Selection.activeObject.GetType());
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.normal.background = Texture2D.whiteTexture;
        //style.normal.background.
        if (Selection.activeGameObject.GetComponent<MyStateMachine.BaseStateMachine>())
        {
            fsm = Selection.activeGameObject.GetComponent<MyStateMachine.BaseStateMachine>();


            if (!loaded)
            {
                int i = 0;
                nodes.Clear();
                mahdic.Clear();
                foreach (MyStateMachine.State s in fsm.states)
                {
                    Node n = new Node(new Vector2(100 * i, 100 * i), 50, 50);
                    n.color = Color.white;
                    n.name = s.name;
                    n.state = s;

                    //Debug.Log("TEST:"+s.uiPosition);

                    //Load state in cached position
                    if (s.uiPosition==Vector2.zero)
                    {
                        Debug.Log("A");
                        n.posAndSize = new Vector4(100 * i, 100 * i, 50, 50);
                    }
                    else
                    {
                        Debug.Log("B:"+n.state.uiPosition);
                        n.posAndSize = new Vector4(s.uiPosition.x, s.uiPosition.y, 50, 50);
                        n.rect.position = new Vector2(s.uiPosition.x, s.uiPosition.y);
                    }
                    
                    n.ID = s.ID;
                    nodes.Add(n);
                    //Debug.Log(n.ID);
                    mahdic.Add(n.ID, n);
                    //GUI.backgroundColor = Color.white;
                    //GUI.Box(new Rect(100, 100*i, 50, 50), s.name,style);
                    //GUI.color = Color.white;
                    i++;
                }

                foreach (MyStateMachine.State s in fsm.states)
                {
                    //Debug.Log("SSSSS");
                    Node n = mahdic[s.ID];

                    foreach (MyStateMachine.Transition t in s.transitions)
                    {

                        Connection c = new Connection();
                        c.n = mahdic[t.nextState.ID];

                        n.connections.Add(c);
                    }
                }

                //foreach (Node n in nodes)
                //{
                //    foreach (MyStateMachine.StateMachine.State s in fsm.states)
                //    {
                //        if(s.ID==n.ID)
                //        {

                //            foreach(MyStateMachine.StateMachine.Transition t in s.transitions)
                //            {


                //            }

                //            break;
                //        }

                //    }
                //}


                loaded = true;
            }

            Handles.color = Color.green;
            //Handles.ArrowHandleCap(0, new Vector2(200,200), Quaternion.LookRotation(Vector3.down), HandleUtility.GetHandleSize(new Vector2(200, 200)), EventType.Repaint);

            foreach (Node n in nodes)
            {
                //if(fsm.currentState.ID==n.ID)
                //{
                //    GUI.backgroundColor = Color.yellow;
                //}
                //else
                //{
                //    GUI.backgroundColor = Color.white;
                //}

                GUI.backgroundColor = n.color;
                GUI.Box(n.rect, n.name, style);
                //Handles.DrawAAConvexPolygon(new Vector3(0, 100), new Vector3(1, 100), new Vector3(0, 150));

                foreach (Connection c in n.connections)
                {
                    Debug.Log("CONNE");
                    Handles.color = Color.white;
                    //Handles.DrawLine(new Vector2(0,0), new Vector3(100,100));
                    Handles.DrawLine(new Vector2(n.rect.x + (n.rect.width / 2), n.rect.y + (n.rect.height / 2)), new Vector2(c.n.rect.x + (c.n.rect.width / 2), c.n.rect.y + (c.n.rect.height / 2)));


                    Vector2 d = new Vector2(n.rect.x, n.rect.y) - new Vector2(c.n.rect.x, c.n.rect.y);


                    //EditorGUIUtility.RotateAroundPivot(Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg, Vector2.Lerp(new Vector2(n.rect.x, n.rect.y), new Vector2(c.n.rect.x, c.n.rect.y), 0.5f));
                    GUI.Box(new Rect(Vector2.Lerp(new Vector2(n.rect.x, n.rect.y), new Vector2(c.n.rect.x, c.n.rect.y), 0.5f), Vector2.one), "", style);
                    //EditorGUIUtility.RotateAroundPivot(0,Vector2.zero);

                    //Handles.ArrowHandleCap(0, new Vector2(n.rect.x, n.rect.y), Quaternion.LookRotation(d.normalized), 1, EventType.Repaint);
                }


                //GUI.color = Color.white;
            }



            Debug.Log("OBJECT HAS STATE MACHINE");





        }


        ProcessNodeEvents(Event.current);





        //Selection.activeGameObject.GetC

        //Handles.DrawBezier(new Vector3(100, 100), new Vector3(100, 200), new Vector3(100, 150), new Vector3(50, 100), Color.blue, null, 2F);



        //for(int i=0;i<10;i++)
        //{
        //    Handles.DrawLine(new Vector2(20 * i, 0), new Vector2(20 * i, position.height));
        //}

        ////EditorGUILayout.RectField("TEST", new Rect(0, 0, 50, 50));

        //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //myString = EditorGUILayout.TextField("Text Field", myString);

        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        //EditorGUILayout.EndToggleGroup();


        //GUI.color = new Color(1, 0, 0, 1);
        //GUI.Box(new Rect(100, 100, 50, 50), "Test");
    }

    private void Update()
    {

        foreach (Node n in nodes)
        {

            if (fsm.currentState.ID == n.ID)
            {
                n.color = Color.yellow;
            }
            else
            {
                n.color = Color.white;
            }
        }
        Repaint();
    }

    class Node
    {
        public Vector4 posAndSize;
        public string name;
        public Rect rect;
        public int ID;
        public Color color;
        public List<Connection> connections = new List<Connection>();
        public bool isDragged;

        public State state;

        public Node(Vector2 pos, float width, float height)
        {
            rect = new Rect(pos.x, pos.y, width, height);
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;

            if(state!=null)
            {
                
                state.uiPosition = rect.position;


                //Debug.Log("POS NODE:"+state.uiPosition);

            }

        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                        }



                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;

                    break;

                case EventType.MouseDrag:

                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;

            }

            return false;
        }

    }

    class Connection
    {
        public Node n;

    }


    void ProcessNodeEvents(Event e)
    {
        //MIDDLE CLICK TO SHOW UI WHILE PLAYMODE

        //Debug.Log(e.button);
        //Debug.Log(e.ToString());
        if (e.button == 2)
        {
            //Debug.Log
            //("REEEEEEEEEEEEEEEE");
            loaded = false;
        }

        if (nodes.ToArray().Length > 0)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }



        }




        //Debug.Log(e.ToString());
        //if (e.button == 1)
        //{
        //    //Debug.Log
        //        //("REEEEEEEEEEEEEEEE");
        //    loaded = false;
        //}

    }
}