using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RVN_CombatInputController : RVN_Singleton<RVN_CombatInputController>
{
    [SerializeField] private RVN_ComponentHandler currentCharacter;

    private CPN_Character nextCharacter;

    private CPN_CharacterAction selectedAction = null;

    [SerializeField] private bool isActionPlaying = false;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent<CPN_CharacterAction> OnSelectAction;
    [SerializeField] private UnityEvent<CPN_CharacterAction> OnRefreshActionDisplay;
    [SerializeField] private UnityEvent<CPN_CharacterAction> OnUnselectAction;

    private void Update()
    {
        if(!isActionPlaying && selectedAction != null)
        {
            OnRefreshActionDisplay?.Invoke(selectedAction);
        }
    }

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

        UnselectAction();
    }

    private void ChangeCharacter(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter.Handler;

        SelectAction(0);
    }

    public void SelectAction(int actionSelected)
    {
        if (currentCharacter != null)
        {
            if (selectedAction != null)
            {
                UnselectAction();
            }

            switch (actionSelected)
            {
                case 0:
                    if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
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

            OnSelectAction?.Invoke(selectedAction);
        }
    }

    private void UnselectAction()
    {
        OnUnselectAction?.Invoke(selectedAction);

        selectedAction = null;
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
        OnSelectAction?.Invoke(selectedAction);

        isActionPlaying = false;
    }

    //CODE REVIEW : Voir pour mettre ça dans un gestionnaire de Feedback ?
    public void DisplayAction(CPN_CharacterAction toDisplay)
    {
        if (toDisplay != null)
        {
            toDisplay.DisplayAction(RVN_InputController.MousePosition);
        }
    }

    public void UndisplayAction(CPN_CharacterAction toUndisplay)
    {
        if (toUndisplay != null)
        {
            toUndisplay.UndisplayAction(RVN_InputController.MousePosition);
        }
    }
}
