using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_Wait : SequenceAction
{
    [SerializeField] private float waitTime;

    private TimerManager.Timer waitTimer;

    protected override void OnStartAction()
    {
        waitTimer = TimerManager.CreateGameTimer(waitTime, EndAction);
    }

    protected override void OnEndAction()
    {
        waitTimer?.Stop();
        waitTimer = null;
    }

    protected override void OnSkipAction()
    {
        waitTimer?.Execute();
        waitTimer = null;
    }
}
