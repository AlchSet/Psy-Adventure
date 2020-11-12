using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DirectoryNode : MonoBehaviour
{

    public AreaData area;

    public DirectoryConnection parent;
    public List<DirectoryConnection> children = new List<DirectoryConnection>();

    //public DirectoryNode parent=new DirectoryNode();

}

/// <summary>
/// Propably Unecessary
/// </summary>
[System.Serializable]
public class DirectoryConnection
{
    public string name;

    public DirectoryNode destination;


}