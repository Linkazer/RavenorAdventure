using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SQA_UnityEvent : SequenceAction
{
    [SerializeField] private UnityEvent eventToPlay;

    protected override void OnStartAction()
    {
        eventToPlay?.Invoke();
        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        eventToPlay?.Invoke();
    }
}
