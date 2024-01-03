using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharacterEnterNode : SequenceAction
{
    [SerializeField, System.Obsolete] private NodeDataHanlder nodeDataToCheck;
    [SerializeField] private List<NodeDataHanlder> movingNodesToCheck;
    [SerializeField] private List<NodeDataHanlder> nodes;

    protected override void OnStartAction()
    {
        if(movingNodesToCheck.Count == 0)
        {
            movingNodesToCheck.Add(nodeDataToCheck);
        }

        foreach (NodeDataHanlder node in nodes)
        {
            node.actOnDataEnter += DetectEntry;
        }
    }

    protected override void OnEndAction()
    {
        foreach (NodeDataHanlder node in nodes)
        {
            node.actOnDataEnter -= DetectEntry;
        }
    }

    protected override void OnSkipAction()
    {

    }

    private void DetectEntry(NodeDataHanlder nodeData)
    {
        foreach (NodeDataHanlder node in movingNodesToCheck)
        {
            if (nodeData == node)
            {
                EndAction();
            }
        }
    }
}
