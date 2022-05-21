using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains all the data needed by a Node.
/// </summary>
public class NodeDataHanlder : MonoBehaviour
{
    /// <summary>
    /// Does the component block the vision.
    /// </summary>
    [SerializeField] private bool blockVision = false;
    /// <summary>
    /// Can the component be walk on.
    /// </summary>
    [SerializeField] private bool walkable = true;
    /// <summary>
    /// Reference to the ComponentHandler.
    /// </summary>
    [SerializeField] private RVN_ComponentHandler componentsHandler;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<Node> OnEnterNode;          // Played when the component enter a node.
    [SerializeField] private UnityEvent<Node> OnExitNode;           // Player when the component exit a node.
    [SerializeField] private UnityEvent<NodeDataHanlder> OnDataEnter;// Played when an other component enter the node on which the component is.
    [SerializeField] private UnityEvent<NodeDataHanlder> OnDataExit;// Played when an other component exit the node on which the component is.

    /// <summary>
    /// The node on which the component is.
    /// </summary>
    private Node currentNode = null;

    public Node CurrentNode => currentNode;

    public RVN_ComponentHandler Handler => componentsHandler;

    public bool Walkable => walkable;

    public bool BlockVision => blockVision;

    private void Start()
    {
        SetNodeDataFromPosition();
    }

    /// <summary>
    /// Assign a new node to the component.
    /// </summary>
    /// <param name="nNode">The node to assign.</param>
    public void SetNodeData(Node nNode)
    {
        UnsetNodeData(true);

        currentNode = nNode;

        if (currentNode != null)
        {
            currentNode.AddDataOnNode(this);

            OnEnterNode?.Invoke(currentNode);
        }
    }

    /// <summary>
    /// Assign a new node to the component depending on the position of the GameObject.
    /// </summary>
    public void SetNodeDataFromPosition()
    {
        SetNodeData(Grid.GetNodeFromWorldPoint(transform.position));
    }

    /// <summary>
    /// Unassign the current node.
    /// </summary>
    public void UnsetNodeData(bool applyNodeEffect)
    {
        if (currentNode != null)
        {
            if (applyNodeEffect)
            {
                OnExitNode?.Invoke(currentNode);
            }

            currentNode.RemoveDataOnNode(this);

            currentNode = null;
        }
    }

    /// <summary>
    /// Called when an other component enter the current node.
    /// </summary>
    /// <param name="dataEnter">The NodeDataHandler that enter the node.</param>
    public void OnDataEnterCurrentNode(NodeDataHanlder dataEnter)
    {
        OnDataEnter?.Invoke(dataEnter);
    }
    /// <summary>
    /// Called when an other component exit the current node.
    /// </summary>
    /// <param name="dataExit">The NodeDataHandler that exit the node.</param>
    public void OnDataExitCurrentNode(NodeDataHanlder dataExit)
    {
        OnDataExit?.Invoke(dataExit);
    }
}
