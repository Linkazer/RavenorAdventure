using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LvlEnd_CharacterDeath : RVN_LevelEnd
{
    [SerializeField] private bool looseOnAchieve = false;

    [SerializeField] private int wantedCharacterDeath = -1;

    [SerializeField] private List<CPN_Character> wantedEnnemyDeads = new List<CPN_Character>();

    private int currentCharacterDeath = 0;

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
        if (wantedEnnemyDeads.Contains(diedCharacter))
        {
            wantedEnnemyDeads.Remove(diedCharacter);

            currentCharacterDeath++;

            if(currentCharacterDeath == wantedCharacterDeath || wantedEnnemyDeads.Count <= 0)
            {
                if (!looseOnAchieve)
                {
                    WinLevel();
                }
                else
                {
                    LoseLevel();
                }
            }
        }
    }
}
