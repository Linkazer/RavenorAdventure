using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SpellCaster : CPN_CharacterAction
{
    [SerializeField] private List<SpellScriptable> spells;

    private bool hasUsedSpell = false;
    private int currentSelectedSpell = -1;

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        Debug.Log("Affiche la zone de portée du sort");
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        //TO DO : Vérifier la faisabilité du spell sélectionné.
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
            RVN_SpellManager.UseSpell(spells[currentSelectedSpell].GetSpellData().GetCopy(), Grid.GetNodeFromWorldPoint(actionTargetPosition), callback);

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
