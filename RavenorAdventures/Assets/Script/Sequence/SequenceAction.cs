using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceAction : MonoBehaviour
{
    private Action endCallback;

    public void StartAction()
    {
        endCallback = null;

        OnStartAction();
    }

    public void StartAction(Action callback)
    {
        endCallback = callback;

        OnStartAction();
    }

    public void EndAction()
    {
        OnEndAction();

        endCallback?.Invoke();
    }

    public void SkipAction(Action callback)
    {
        OnSkipAction();
    }

    //Start action
    protected abstract void OnStartAction();

    //End Action
    protected abstract void OnEndAction();

    //Skip action
    protected abstract void OnSkipAction();
}
