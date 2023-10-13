using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_StartSequence : SequenceAction
{
    [SerializeField] private Sequence toStart;

    protected override void OnStartAction()
    {
        toStart.StartAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
       
    }

}
