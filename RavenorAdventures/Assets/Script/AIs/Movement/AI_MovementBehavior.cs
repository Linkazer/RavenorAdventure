using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AI_MovementBehavior
{
    protected enum PathType
    {
        PathBlockByMovingObstacle,
        PathClear,
    }

    public abstract List<Node> GetBestMovementNodes(CPN_Character currentCharacter);
}
