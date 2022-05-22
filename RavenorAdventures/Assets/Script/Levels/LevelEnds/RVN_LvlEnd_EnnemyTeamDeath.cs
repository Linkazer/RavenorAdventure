using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LvlEnd_EnnemyTeamDeath : RVN_LevelEnd
{
    protected override void OnSetLevelEnd()
    {
        RVN_BattleManager.OnEnnemyTeamDie += WinLevel;
    }

    protected override void OnUnsetLevelEnd()
    {
        RVN_BattleManager.OnEnnemyTeamDie -= WinLevel;
    }
}
