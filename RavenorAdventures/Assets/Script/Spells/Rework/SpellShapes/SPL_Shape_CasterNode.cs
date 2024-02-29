using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPL_Shape_CasterNode : SPL_SpellActionShape
{
    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        return new List<Node>() { casterNode };
    }
}
