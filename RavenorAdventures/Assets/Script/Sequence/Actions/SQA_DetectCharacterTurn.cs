using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_DetectCharacterTurn : SequenceAction
{
    [SerializeField] private CPN_Character characterToCheck;

    protected override void OnStartAction()
    {
        if (RVN_BattleManager.CurrentCharacter == characterToCheck)
        {
            EndAction();
        }
        else
        {
            characterToCheck.ActOnBeginTurn += DetectTurn;
        }
    }

    protected override void OnEndAction()
    {
        characterToCheck.ActOnBeginTurn -= DetectTurn;
    }

    protected override void OnSkipAction()
    {
        
    }

    private void DetectTurn(RVN_ComponentHandler handler)
    {
        EndAction();
    }
}
