using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LvlEnd_EnnemyTeamDeath : RVN_LevelEnd
{
    [SerializeField] private List<CPN_Character> wantedEnnemyDeads = new List<CPN_Character>();


    protected override void OnSetLevelEnd()
    {
        RVN_BattleManager.ActOnCharacterDie += CheckCharacterDeath;
    }

    protected override void OnUnsetLevelEnd()
    {
        RVN_BattleManager.ActOnCharacterDie -= CheckCharacterDeath;
    }

    private void CheckCharacterDeath(CPN_Character diedCharacter)
    {
        if(wantedEnnemyDeads.Contains(diedCharacter))
        {
            wantedEnnemyDeads.Remove(diedCharacter);

            if(wantedEnnemyDeads.Count <= 0)
            {
                WinLevel();
            }
        }
    }
}
