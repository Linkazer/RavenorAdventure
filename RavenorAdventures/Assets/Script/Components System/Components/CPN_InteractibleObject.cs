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

    public override void OnEnterBattle()
    {

    }

    public override void OnExitBattle()
    {

    }

    /// <summary>
    /// Essaye de faire intéragir le personnage actuel avec l'objet.
    /// </summary>
    public void TryInteract()
    {
        TryInteract(RVN_BattleManager.CurrentCharacter);
    }

    /// <summary>
    /// Essaye de faire intéragir l'objet voulut avec l'objet interactible.
    /// </summary>
    /// <param name="interactor">L'objet qui veut interagir.</param>
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

    /// <summary>
    /// Effectue l'interaction avec l'objet voulant interagir.
    /// </summary>
    /// <param name="interactor">L'objet voulant interagir.</param>
    public void Interact(RVN_ComponentHandler interactor)
    {
        OnInteract?.Invoke(interactor);
    }
}
