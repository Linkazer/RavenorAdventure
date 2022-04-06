using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_CombatInputController : RVN_Singleton<RVN_CombatInputController>
{
    [SerializeField] private RVN_ComponentHandler currentCharacter;

    private CPN_Character nextCharacter;

    private CPN_CharacterAction selectedAction = null;

    [SerializeField] private bool isActionPlaying = false;

    private void LateUpdate()
    {
        if (nextCharacter != null) //CODE REVIEW : A revoir avec le MouseHandler (Eviter de faire une action si on sélectionne un personnage)
        {
            ChangeCharacter(nextCharacter);

            nextCharacter = null;
        }
    }

    public void SetCurrentCharacter(CPN_Character nCharacter)
    {
        nextCharacter = nCharacter;
    }

    private void ChangeCharacter(CPN_Character nCharacter)
    {
        if (currentCharacter != null)
        {
            selectedAction.UndisplayAction(RVN_InputController.MousePosition);
        }

        currentCharacter = nextCharacter.Handler;

        selectedAction = null;

        SelectAction(0);
    }

    public void SelectAction(int actionSelected)
    {
        if(selectedAction != null)
        {
            RVN_GridDisplayer.UnsetGridFeedback();
        }

        switch(actionSelected)
        {
            case 0:
                if(currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                {
                    selectedAction = movement;
                    selectedAction.DisplayAction(RVN_InputController.MousePosition);
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

                RVN_GridDisplayer.UnsetGridFeedback();
            }
        }
    }

    private void OnEndAction()
    {
        selectedAction.DisplayAction(RVN_InputController.MousePosition);

        isActionPlaying = false;
    }
}
