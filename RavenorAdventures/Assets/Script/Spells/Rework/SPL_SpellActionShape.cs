using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SPL_SpellActionShape
{
    public abstract List<Node> GetZone(Node casterNode, Node targetNode);
}
