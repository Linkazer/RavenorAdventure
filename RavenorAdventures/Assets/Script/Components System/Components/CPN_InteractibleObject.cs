using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_InteractibleObject : RVN_Component
{
    [SerializeField] private List<Vector3> usedPositions;

    [SerializeField] protected UnityEvent<RVN_ComponentHandler> OnInteract;

    private List<Node> usedNodes = new List<Node>();

    private void Start()
    {
        usedNodes = new List<Node>();
        usedNodes.Add(Grid.GetNodeFromWorldPoint(transform.position));
        foreach(Vector3 v in usedPositions)
        {
            usedNodes.Add(Grid.GetNodeFromWorldPoint(transform.position + v));
        }
    }

    public void TryInteract()
    {
        TryInteract(RVN_BattleManager.CurrentCharacter);
    }

    public void TryInteract(RVN_ComponentHandler interactor)
    {

        foreach(Node n in usedNodes)
        {
            if(Pathfinding.GetDistance(n, interactor.CurrentNode) <= 15)
            {
                Interact(interactor);
            }
        }
    }

    public void Interact(RVN_ComponentHandler interactor)
    {
        OnInteract?.Invoke(interactor);
    }
}
