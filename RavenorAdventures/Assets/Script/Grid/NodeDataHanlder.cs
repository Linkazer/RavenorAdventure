using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeDataHanlder : MonoBehaviour
{
    [SerializeField] private bool blockVision = false;
    [SerializeField] private bool walkable = true;

    [SerializeField] private RVN_ComponentHandler componentsHandler;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<Node> OnEnterNode;
    [SerializeField] private UnityEvent<Node> OnExitNode;
    [SerializeField] private UnityEvent<NodeDataHanlder> OnDataEnter;
    [SerializeField] private UnityEvent<NodeDataHanlder> OnDataExit;

    private Node currentNode = null;

    public RVN_ComponentHandler Handler => componentsHandler;

    public bool Walkable => walkable;

    public bool BlockVision => blockVision;

    private void Start()
    {
        SetNodeDataFromPosition();
    }

    public void SetNodeData(Node nNode)
    {
        UnsetNodeData();

        currentNode = nNode;

        if (currentNode != null)
        {
            currentNode.AddDataOnNode(this);

            OnEnterNode?.Invoke(currentNode);
        }
    }

    public void SetNodeDataFromPosition()
    {
        SetNodeData(Grid.GetNodeFromWorldPoint(transform.position));
    }

    public void UnsetNodeData()
    {
        if (currentNode != null)
        {
            OnExitNode?.Invoke(currentNode);

            currentNode.RemoveDataOnNode(this);

            currentNode = null;
        }
    }

    public void OnDataEnterCurrentNode(NodeDataHanlder dataEnter)
    {
        OnDataEnter?.Invoke(dataEnter);
    }

    public void OnDataExitCurrentNode(NodeDataHanlder dataExit)
    {
        OnDataExit?.Invoke(dataExit);
    }
}
