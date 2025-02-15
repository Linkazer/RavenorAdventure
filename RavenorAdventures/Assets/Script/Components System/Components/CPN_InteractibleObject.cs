using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_InteractibleObject : RVN_Component
{
    [SerializeField] private List<NodeDataHanlder> interactibleNodes = new List<NodeDataHanlder>();

    [SerializeField] private SequenceCutscene interactionCutscene;

    /// <summary>
    /// Essaye de faire intéragir l'objet voulut avec l'objet interactible. Utiliser lors des Combats
    /// </summary>
    /// <param name="interactor">L'objet qui veut interagir.</param>
    public void TryInteract(RVN_ComponentHandler interactor)
    {
        foreach (NodeDataHanlder n in interactibleNodes)
        {
            if (Pathfinding.GetDistance(n.CurrentNode, interactor.CurrentNode) <= 15)
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
        if (interactionCutscene != null)
        {
            interactionCutscene.StartAction(EndInteraction);
        }
        else
        {
            EndInteraction();
        }
    }

    private void EndInteraction()
    {

    }

    public override void SetComponent(RVN_ComponentHandler handler)
    {
        
    }

    public override void Activate()
    {
        if (interactibleNodes.Count == 0)
        {
            FillNodes();
        }
    }

    public override void Disactivate()
    {
        
    }

    [ContextMenu("Search for NodeData")]
    private void FillNodes()
    {
        foreach (Transform tr in transform)
        {
            if (tr.gameObject.GetComponent<NodeDataHanlder>() != null)
            {
                interactibleNodes.Add(tr.gameObject.GetComponent<NodeDataHanlder>());
            }
        }
    }
}
