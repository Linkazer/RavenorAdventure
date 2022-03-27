using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SpellCaster : CPN_CharacterAction
{
    [SerializeField] private List<Spell> spells;

    private bool hasUsedSpell = false;
    private int currentSelectedSpell = -1;

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        Debug.Log("Affiche la zone de portée du sort");
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return !hasUsedSpell && spells.Count > 0;
    }

    public override void ResetActionData()
    {
        hasUsedSpell = false;
        currentSelectedSpell = -1;
    }

    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        if(currentSelectedSpell >= 0)
        {
            Debug.Log("Use " + spells[currentSelectedSpell].name + " at position " + actionTargetPosition);

            hasUsedSpell = true;
        }

        callback?.Invoke();
    }

    public void SelectSpell(int spellIndex)
    {
        if (spellIndex == currentSelectedSpell)
        {
            currentSelectedSpell = -1;
        }
        else
        {
            currentSelectedSpell = spellIndex;
        }

        DisplayAction(RVN_InputController.MousePosition);
    }
}
