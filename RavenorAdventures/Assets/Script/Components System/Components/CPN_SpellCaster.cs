using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_SpellCaster : CPN_CharacterAction<CPN_Data_SpellCaster>
{
    [SerializeField] private List<SpellScriptable> spells;
    [SerializeField] private NodeDataHanlder nodeData;

    [SerializeField] private UnityEvent<LaunchedSpellData> OnCastSpell;
    [SerializeField] private UnityEvent<Vector2> OnCastSpellToDiection;

    [SerializeField] private int actionsLeftThisTurn = 1;
    [SerializeField] private int actionByTurn = 1;
    private int currentSelectedSpell = -1;

    //Base datas
    [SerializeField] private int possibleReroll;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;

    public Action<RVN_ComponentHandler> actOnDealDamageSelf;
    public Action<RVN_ComponentHandler> actOnDealDamageTarget;
    public Action<int> actOnSetActionLeft;
    public Action<SpellScriptable> actOnSelectSpell;
    public Action<SpellScriptable> actOnUnselectSpell;


    public List<SpellScriptable> Spells => spells;
    public int ActionByTurn => actionByTurn;
    public int ActionLeftThisTurn => actionsLeftThisTurn;
    public int PossibleReroll => possibleReroll;
    public int Accuracy => accuracy;
    public int Power => power;

    public void AddPossibleReroll(int amount)
    {
        possibleReroll += amount;
    }

    public void AddAccuracy(int amount)
    {
        accuracy += amount;
    }

    public void AddPower(int amount)
    {
        power += amount;
    }

    public void AddBonusAction(int amount)
    {
        actionByTurn += amount;
        SetActionLeft(actionsLeftThisTurn + amount);//CODE REVIEW : Voir si on garde ça niveau GD
    }

    private void SetActionLeft(int amount)
    {
        actionsLeftThisTurn = amount;

        actOnSetActionLeft?.Invoke(actionsLeftThisTurn);
    }

    /// <summary>
    /// Display every node on which the action can be used.
    /// </summary>
    /// <param name="actionTargetPosition">The current target position the player aim for.</param>
    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (currentSelectedSpell >= 0 && spells[currentSelectedSpell].IsUsable)
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

    public override bool CanSelectAction()
    {
        return spells.Count > 0 && actionsLeftThisTurn > 0 && currentSelectedSpell >= 0 && spells[currentSelectedSpell].IsUsable;
    }

    /// <summary>
    /// Check if the action can be used at the target position.
    /// </summary>
    /// <param name="actionTargetPosition">The position where we check if the action is usable.</param>
    /// <returns></returns>
    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return  actionsLeftThisTurn > 0 && 
                spells.Count > 0 && currentSelectedSpell >= 0 && 
                spells[currentSelectedSpell].IsUsable &&
                Grid.IsNodeVisible(nodeData.CurrentNode, Grid.GetNodeFromWorldPoint(actionTargetPosition), spells[currentSelectedSpell].Range);
    }

    /// <summary>
    /// Reset the action datas.
    /// </summary>
    public override void ResetData()
    {
        SetActionLeft(actionByTurn);
        currentSelectedSpell = -1;
    }

    /// <summary>
    /// Use a spell if the target is available.
    /// </summary>
    /// <param name="actionTargetPosition">The position of the target wanted.</param>
    /// <param name="callback">The callback to play once the spell end.</param>
    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        LaunchedSpellData launchedSpell = new LaunchedSpellData(spells[currentSelectedSpell], this, Grid.GetNodeFromWorldPoint(actionTargetPosition));

        if (currentSelectedSpell >= 0 && RVN_SpellManager.CanUseSpell(launchedSpell))
        {
            CastSpell(launchedSpell, callback);

            launchedSpell.scriptable.ResetCooldown();

            SetActionLeft(actionsLeftThisTurn - 1);
        }
        else
        {
            callback?.Invoke();
        }
    }

    /// <summary>
    /// Select a spell.
    /// </summary>
    /// <param name="spellIndex">The index of the spell to choose.</param>
    public void SelectSpell(int spellIndex)
    {
        //UndisplayAction(RVN_InputController.MousePosition); //CODE REVIEW : Voir pour mieux gérer l'affichage/désaffichage du cadrillage

        int lastSpell = currentSelectedSpell;
        if (currentSelectedSpell >= 0)
        {
            UnselectSpell();
        }

        if (spellIndex != lastSpell && spellIndex >= 0)
        {
            currentSelectedSpell = spellIndex;

            actOnSelectSpell?.Invoke(spells[currentSelectedSpell]);
        }

        DisplayAction(RVN_InputController.MousePosition);
    }

    public void UnselectSpell()
    {
        UndisplayAction(RVN_InputController.MousePosition);

        actOnUnselectSpell?.Invoke(spells[currentSelectedSpell]);

        currentSelectedSpell = -1;
    }

    /// <summary>
    /// Prepare a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to cast.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void CastSpell(LaunchedSpellData launchedSpell, Action callback)
    {
        OnCastSpell?.Invoke(launchedSpell);

        OnCastSpellToDiection?.Invoke(launchedSpell.targetNode.worldPosition - launchedSpell.caster.transform.position);

        TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => UseSpell(launchedSpell, callback));
    }

    /// <summary>
    /// Launch a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to launch.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void UseSpell(LaunchedSpellData launchedSpell, Action callback)
    {
        RVN_SpellManager.UseSpell(launchedSpell, () => EndUseSpell(callback));
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

    public override void SetData(CPN_Data_SpellCaster toSet)
    {
        actionByTurn = toSet.MaxSpellUseByTurn();

        spells = new List<SpellScriptable>();
        foreach (SpellScriptable spell in toSet.AvailableSpells())
        {
            spells.Add(Instantiate(spell));
        }

        possibleReroll = toSet.PossibleRelance();

        accuracy = toSet.Accuracy();
        power = toSet.Power();
    }

    public void UpdateCooldowns()
    {
        foreach(SpellScriptable spell in spells)
        {
            spell.UpdateCurrentCooldown();
        }
    }

    //CODE REVIEW : Voir comment on peut faire pour éviter de juste avoir les events ici ?
    public void DealDamage(CPN_HealthHandler target)
    {
        actOnDealDamageSelf?.Invoke(Handler);
        actOnDealDamageTarget?.Invoke(target.Handler);
    }
}
