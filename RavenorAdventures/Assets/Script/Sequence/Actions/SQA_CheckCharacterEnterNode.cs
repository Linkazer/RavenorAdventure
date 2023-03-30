using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharacterEnterNode : SequenceAction
{
    [SerializeField] private NodeDataHanlder nodeDataToCheck;
    [SerializeField] private NodeDataHanlder node;

    protected override void OnStartAction()
    {
        node.actOnDataEnter += DetectEntry;
    }

    protected override void OnEndAction()
    {
        node.actOnDataEnter -= DetectEntry;
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
