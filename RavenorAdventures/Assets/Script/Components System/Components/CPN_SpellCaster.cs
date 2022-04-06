using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SpellCaster : CPN_CharacterAction
{
    [SerializeField] private List<SpellScriptable> spells;
    [SerializeField] private NodeDataHanlder nodeData;

    private bool hasUsedSpell = false;
    private int currentSelectedSpell = -1;

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        Debug.Log("Affiche la zone de portée du sort");
    }

    public override void UndisplayAction(Vector2 actionTargetPosition)
    {

    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return !hasUsedSpell && spells.Count > 0 && Grid.IsNodeVisible(nodeData.CurrentNode, Grid.GetNodeFromWorldPoint(actionTargetPosition), spells[currentSelectedSpell].GetSpellData().Range);
    }

    public override void ResetActionData()
    {
        hasUsedSpell = false;
        currentSelectedSpell = -1;
    }

    /// <summary>
    /// Use a spell if the target is available.
    /// </summary>
    /// <param name="actionTargetPosition">The position of the target wanted.</param>
    /// <param name="callback">The callback to play once the spell end.</param>
    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        if(currentSelectedSpell >= 0)
        {
            RVN_SpellManager.UseSpell(spells[currentSelectedSpell].GetSpellData().GetCopy(), Grid.GetNodeFromWorldPoint(actionTargetPosition), callback);

            hasUsedSpell = true;
        }

        callback?.Invoke();
    }

    /// <summary>
    /// Select a spell.
    /// </summary>
    /// <param name="spellIndex">The index of the spell to choose.</param>
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
