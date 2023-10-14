using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_StartSequence : SequenceAction
{
    [SerializeField] private int loopAmount = -1;
    [SerializeField] private Sequence toStart;

    protected override void OnStartAction()
    {
        EndAction();

        if (loopAmount != 0)
        {
            loopAmount--;
            toStart.StartAction();
        }
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
       
    }

}
