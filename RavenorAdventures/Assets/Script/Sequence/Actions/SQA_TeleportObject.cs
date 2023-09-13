using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_TeleportObject : SequenceAction
{
    [SerializeField] private Transform toMove;
    [SerializeField] private Transform targetPosition;

    protected override void OnStartAction()
    {
        toMove.position = targetPosition.position;
        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        toMove.position = targetPosition.position;
    }
}
