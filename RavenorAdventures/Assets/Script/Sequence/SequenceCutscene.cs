using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutscene : Sequence
{
    protected override void OnStartAction()
    {
        RVN_BattleManager.Instance.PauseBattle(this);

        base.OnStartAction();
    }

    protected override void OnEndAction()
    {
        RVN_BattleManager.Instance.RestartBattle(this);

        base.OnEndAction();
    }
}
