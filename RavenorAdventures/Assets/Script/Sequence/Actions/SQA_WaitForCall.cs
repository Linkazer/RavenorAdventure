using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_WaitForCall : SequenceAction
{
    private bool isActivated;

    protected override void OnStartAction()
    {
        isActivated = true;
    }

    protected override void OnEndAction()
    {
        isActivated = false;
    }

    protected override void OnSkipAction()
    {

    }

    public void CallAction()
    {
        if (isActivated)
        {
            EndAction();
        }
    }
}
