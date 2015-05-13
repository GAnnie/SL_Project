using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetsPathTreeRoot : AssetsPathNode
{
    protected string _nodeName;
    public string nodeName { get { return _nodeName; } }

    public AssetsPathTreeRoot()
    {
        _nodeName = "Assets";
    }
}
