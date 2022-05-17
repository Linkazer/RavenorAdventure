using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_SpellCaster : CPN_CharacterAction
{
    [SerializeField] private List<SpellScriptable> spells;
    [SerializeField] private NodeDataHanlder nodeData;

    [SerializeField] private UnityEvent<CharacterAnimationType, float> OnCastSpell;

    private bool hasUsedSpell = false;
    private int currentSelectedSpell = -1;

    /// <summary>
    /// Display every node on which the action can be used.
    /// </summary>
    /// <param name="actionTargetPosition">The current target position the player aim for.</param>
    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (currentSelectedSpell >= 0)
        {
            List<Node> possibleTargetZone = Pathfinding.GetAllNodeInDistance(nodeData.CurrentNode, spells[currentSelectedSpell].Range, true);
            RVN_GridDisplayer.SetGridFeedback(possibleTargetZone, Color.blue);

            if (possibleTargetZone.Contains(Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition)))
            {
                List<Node> zoneNodes = Pathfinding.GetAllNodeInDistance(Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition), spells[currentSelectedSpell].ZoneRange, false);
                RVN_GridDisplayer.SetGridFeedback(zoneNodes, Color.red);
            }
        }
    }

    /// <summary>
    /// Hide the node of the action.
    /// </summary>
    /// <param name="actionTargetPosition"></param>
    public override void UndisplayAction(Vector2 actionTargetPosition)
    {
        RVN_GridDisplayer.UnsetGridFeedback();
    }

    /// <summary>
    /// Check if the action can be used at the target position.
    /// </summary>
    /// <param name="actionTargetPosition">The position where we check if the action is usable.</param>
    /// <returns></returns>
    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return !hasUsedSpell && spells.Count > 0 && currentSelectedSpell >= 0 && Grid.IsNodeVisible(nodeData.CurrentNode, Grid.GetNodeFromWorldPoint(actionTargetPosition), spells[currentSelectedSpell].Range);
    }

    /// <summary>
    /// Reset the action datas.
    /// </summary>
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
        LaunchedSpellData launchedSpell = new LaunchedSpellData();

        launchedSpell.scriptable = spells[currentSelectedSpell];
        launchedSpell.caster = this;

        if (currentSelectedSpell >= 0 && RVN_SpellManager.CanUseSpell(launchedSpell, Grid.GetNodeFromWorldPoint(actionTargetPosition)))
        {
            CastSpell(launchedSpell, actionTargetPosition, callback);

            hasUsedSpell = true;
        }
    }

    /// <summary>
    /// Select a spell.
    /// </summary>
    /// <param name="spellIndex">The index of the spell to choose.</param>
    public void SelectSpell(int spellIndex)
    {
        UndisplayAction(RVN_InputController.MousePosition); //CODE REVIEW : Voir pour mieux gérer l'affichage/désaffichage du cadrillage

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

    /// <summary>
    /// Prepare a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to cast.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void CastSpell(LaunchedSpellData launchedSpell, Vector2 actionTargetPosition, Action callback)
    {
        OnCastSpell?.Invoke(launchedSpell.scriptable.CastingAnimation, launchedSpell.scriptable.CastDuration);

        TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => UseSpell(launchedSpell, actionTargetPosition, callback));
    }

    /// <summary>
    /// Launch a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to launch.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void UseSpell(LaunchedSpellData launchedSpell, Vector2 actionTargetPosition, Action callback)
    {
        RVN_SpellManager.UseSpell(launchedSpell, Grid.GetNodeFromWorldPoint(actionTargetPosition), () => EndUseSpell(callback));

        hasUsedSpell = true;
    }

    /// <summary>
    /// End a spell.
    /// </summary>
    /// <param name="callback">The callback to call.</param>
    private void EndUseSpell(Action callback)
    {
        callback?.Invoke();

        SelectSpell(-1);
    }
}
