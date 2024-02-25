using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPL_Shape_ByDistance : SPL_SpellActionShape
{
    [SerializeField] private int zoneRange;

    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        return Pathfinding.GetAllNodeInDistance(targetNode, zoneRange, false);
    }
}

