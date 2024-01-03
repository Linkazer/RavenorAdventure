using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharacterAliveByNumber : SequenceAction
{
    [SerializeField] private int teamToCheck = 1;
    [SerializeField] private int numberCharacterAliveWanted = 1;

    protected override void OnStartAction()
    {
        RVN_BattleManager.ActOnCharacterDie += OnCharacterDeath;
    }

    protected override void OnEndAction()
    {
        RVN_BattleManager.ActOnCharacterDie -= OnCharacterDeath;
    }

    protected override void OnSkipAction()
    {
        RVN_BattleManager.ActOnCharacterDie -= OnCharacterDeath;
    }

    private void OnCharacterDeath(RVN_ComponentHandler deadCharacter)
    {
        Debug.Log(RVN_BattleManager.GetTeamByIndex(teamToCheck).Count);
        if(RVN_BattleManager.GetTeamByIndex(teamToCheck).Count <= numberCharacterAliveWanted + 1)
        {
            EndAction();
        }
    }
}
