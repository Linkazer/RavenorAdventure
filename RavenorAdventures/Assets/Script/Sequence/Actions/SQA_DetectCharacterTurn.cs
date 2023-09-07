using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_DetectCharacterTurn : SequenceAction
{
    [SerializeField] private CPN_Character characterToCheck;
    [SerializeField, Tooltip("Si mit � VRAI, l'action sera pas d�clench� si c'est d�j� le tour du personnage.")] private bool checkBeginTurn;

    protected override void OnStartAction()
    {
        if (!checkBeginTurn && RVN_BattleManager.CurrentCharacter == characterToCheck)
        {
            EndAction();
        }
        else
        {
            characterToCheck.ActOnBeginSelfTurn += DetectTurn;
        }
    }

    protected override void OnEndAction()
    {
        characterToCheck.ActOnBeginSelfTurn -= DetectTurn;
    }

    protected override void OnSkipAction()
    {
        
    }

    private void DetectTurn(RVN_ComponentHandler handler)
    {
        EndAction();
    }
}
