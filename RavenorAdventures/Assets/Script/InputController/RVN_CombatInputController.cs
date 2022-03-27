using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_CombatInputController : RVN_Singleton<RVN_CombatInputController>
{
    [SerializeField] private RVN_ComponentHandler currentCharacter;

    private CPN_CharacterAction selectedAction = null;

    [SerializeField] private bool isActionPlaying = false;

    public void SetCurrentCharacter(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter.Handler;

        selectedAction = null;

        SelectAction(0);
    }

    public void SelectAction(int actionSelected)
    {
        switch(actionSelected)
        {
            case 0:
                if(currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                {
                    selectedAction = movement;
                }
                break;
            case 1:
                if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
                {
                    selectedAction = caster;
                }
                break;
        }
    }

    public void SelectSpell(int spellIndex)
    {
        if(currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.SelectSpell(spellIndex);
        }
    }

    public void DoAction(Vector2 actionPosition)
    {
        if (currentCharacter != null && !isActionPlaying)
        {
            if (selectedAction!= null && selectedAction.IsActionUsable(actionPosition))
            {
                isActionPlaying = true;

                selectedAction.TryDoAction(actionPosition, OnEndAction);
            }
        }
    }

    private void OnEndAction()
    {
        isActionPlaying = false;
    }
}
