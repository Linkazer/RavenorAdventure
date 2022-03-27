using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_CombatInputController : RVN_Singleton<RVN_CombatInputController>
{
    [SerializeField] private RVN_ComponentHandler currentCharacter;

    private int selectedAction = 0;

    [SerializeField] private bool isActionPlaying = false;

    public void SetCurrentCharacter(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter.Handler;
    }

    public void SelectAction(int actionSelected)
    {
        selectedAction = actionSelected;
    }

    public void DoAction(Vector2 actionPosition)
    {
        if (currentCharacter != null && !isActionPlaying)
        {
            if (selectedAction == 0)
            {
                if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                {
                    DoAction(() => movement.AskToMoveTo(actionPosition, OnEndAction));
                }
            }
        }
    }

    private void DoAction(Action actionToDo)
    {
        isActionPlaying = true;
        actionToDo?.Invoke();
    }

    private void OnEndAction()
    {
        isActionPlaying = false;
    }
}
