using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : SequenceAction
{
    [Serializable]
    public class SequenceStep
    {
        public SequenceAction mainAction;
        public SequenceAction[] secondaryActions;
    }

    [SerializeField] private SequenceStep[] steps;

    private int currentStep = 0;

    private void NextStep()
    {
        currentStep++;

        if (currentStep >= steps.Length)
        {
            EndAction();
        }
        else
        {
            steps[currentStep].mainAction.StartAction(NextStep);

            foreach (SequenceAction act in steps[currentStep].secondaryActions)
            {
                act.StartAction(null);
            }
        }
    }

    protected override void OnStartAction()
    {
        currentStep = -1;

        NextStep();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        throw new NotImplementedException();
    }
}
