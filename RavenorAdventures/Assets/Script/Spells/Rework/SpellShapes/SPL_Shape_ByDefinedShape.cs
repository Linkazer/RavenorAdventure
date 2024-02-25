using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPL_Shape_ByDefinedShape : SPL_SpellActionShape
{
    [SerializeField] private List<Vector2Int> zoneDefined;
    [SerializeField] private bool zoneFaceCaster;

    public override List<Node> GetZone(Node casterNode, Node targetNode)
    {
        List<Node> toReturn = new List<Node>();

        Vector2 direction = Vector2.one;
        if (zoneFaceCaster && casterNode != null)
        {
            direction = new Vector2(targetNode.gridX, targetNode.gridY) - new Vector2(casterNode.gridX, casterNode.gridY);
        }

        foreach (Vector2Int v in zoneDefined)
        {
            Node toAdd = null;

            if (direction.y == 0 && direction.x == 0)
            {
                toAdd = Grid.GetNode(targetNode.gridX + v.x, targetNode.gridY + v.y);
            }
            else if (direction.y > 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
            {
                toAdd = Grid.GetNode(targetNode.gridX + v.x, targetNode.gridY + v.y);
            }
            else if (direction.x < 0 && (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) || direction.x == -direction.y))
            {
                toAdd = Grid.GetNode(targetNode.gridX - v.y, targetNode.gridY + v.x);
            }
            else if (direction.y < 0 && (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) || direction.x == direction.y))
            {
                toAdd = Grid.GetNode(targetNode.gridX - v.x, targetNode.gridY - v.y);
            }
            else
            {
                toAdd = Grid.GetNode(targetNode.gridX + v.y, targetNode.gridY - v.x);
            }

            if (toAdd != null)
            {
                toReturn.Add(toAdd);
            }
        }

        return toReturn;
    }
}
