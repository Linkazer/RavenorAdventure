using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharacterEnterNode : SequenceAction
{
    [SerializeField] private NodeDataHanlder nodeDataToCheck;
    [SerializeField] private List<NodeDataHanlder> nodes;

    protected override void OnStartAction()
    {
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
        if (nodeData == nodeDataToCheck)
        {
            EndAction();
        }
    }
}
