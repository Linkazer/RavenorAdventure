using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_PauseBattle : SequenceAction
{
    [SerializeField] private MonoBehaviour locker;
    [SerializeField] private bool setPause;

    protected override void OnStartAction()
    {
        if (setPause)
        {
            RVN_BattleManager.Instance.PauseBattle(locker);
        }
        else
        {
            RVN_BattleManager.Instance.RestartBattle(locker);
        }
    }

    protected override void OnEndAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSkipAction()
    {
        throw new System.NotImplementedException();
    }
}
