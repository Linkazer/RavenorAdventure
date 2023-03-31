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
        public List<SequenceAction> secondaryActions = new List<SequenceAction>();
    }

    [SerializeField] private List<SequenceStep> steps = new List<SequenceStep>();

    private int currentStep = 0;

    [ContextMenu("Fill Sequence")]
    public void FillSequence()
    {
        steps = new List<SequenceStep>();

        foreach (Transform child in transform)
        {
            SequenceStep newStep = new SequenceStep();

            newStep.mainAction = child.GetComponent<SequenceAction>();

            if(newStep.mainAction != null && newStep.mainAction.enabled)
            {
                foreach (Transform secondaryChild in child)
                {
                    SequenceAction toAdd = secondaryChild.GetComponent<SequenceAction>();
                    if (toAdd != null && toAdd.enabled)
                    {
                        newStep.secondaryActions.Add(toAdd);
                    }
                }

                steps.Add(newStep);
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
    }

    private void NextStep()
    {
        currentStep++;

        if (currentStep >= steps.Count)
        {
            EndAction();
        }
        else
        {
            foreach (SequenceAction act in steps[currentStep].secondaryActions)
            {
                act.StartAction();
            }

            steps[currentStep].mainAction.StartAction(NextStep);
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
