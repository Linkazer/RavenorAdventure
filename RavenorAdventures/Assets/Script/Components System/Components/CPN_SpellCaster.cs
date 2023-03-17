using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_SpellCaster : CPN_CharacterAction<CPN_Data_SpellCaster>
{
    private SpellScriptable opportunitySpell;
    [SerializeField] private List<SpellScriptable> spells;
    [SerializeField] private NodeDataHanlder nodeData;

    [SerializeField] private UnityEvent<LaunchedSpellData> OnCastSpell;
    [SerializeField] private UnityEvent<Vector2> OnCastSpellToDiection;
    [SerializeField] private UnityEvent onOpportunityAttack;

    [SerializeField] private int actionsLeftThisTurn = 1;
    [SerializeField] private int actionByTurn = 1;
    private SpellScriptable currentSelectedSpell = null;

    [HideInInspector] public bool hasOpportunityAttack = true;

    //Base datas
    [SerializeField] private int offensiveRerolls;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;

    [SerializeField] private SpellRessource ressource;

    public Action<RVN_ComponentHandler> actOnDealDamageSelf;
    public Action<RVN_ComponentHandler> actOnDealDamageTarget;
    public Action<RVN_ComponentHandler> actOnUseSkillSelf;
    public Action<RVN_ComponentHandler> actOnUseSkillTarget;
    public Action actOnEndCastSpell;
    public Action<int> actOnSetActionLeft;
    public Action<SpellScriptable> actOnSelectSpell;
    public Action<SpellScriptable> actOnUnselectSpell;

    public List<SpellScriptable> Spells => spells;
    public SpellScriptable OpportunitySpell => opportunitySpell;
    public int ActionByTurn => actionByTurn;
    public int ActionLeftThisTurn => actionsLeftThisTurn;
    public int OffensiveRerolls => offensiveRerolls;
    public int Accuracy => accuracy;
    public int Power => power;

    public SpellRessource Ressource => ressource;

    public Node CurrentNode => nodeData.CurrentNode;

    public void AddOffensiveRerolls(int amount)
    {
        offensiveRerolls += amount;
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

    public override void UnselectAction()
    {
        SelectSpell(-1);
    }

    /// <summary>
    /// Display every node on which the action can be used.
    /// </summary>
    /// <param name="actionTargetPosition">The current target position the player aim for.</param>
    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (currentSelectedSpell != null && currentSelectedSpell.IsUsable && (ressource == null || currentSelectedSpell.RessourceCost <= ressource.CurrentAmount))
        {
            List<Node> possibleTargetZone = Pathfinding.GetAllNodeInDistance(nodeData.CurrentNode, currentSelectedSpell.Range, true);

            RVN_GridDisplayer.SetGridFeedback(possibleTargetZone, Color.blue);

            if (possibleTargetZone.Contains(Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition)))
            {
                List<Node> zoneNodes = currentSelectedSpell.GetZone(Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition), CurrentNode);
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
        return spells.Count > 0 && /*actionsLeftThisTurn > 0 &&*/ currentSelectedSpell != null && currentSelectedSpell.IsUsable && (ressource == null || currentSelectedSpell.RessourceCost <= ressource.CurrentAmount);
    }

    /// <summary>
    /// Check if the action can be used at the target position.
    /// </summary>
    /// <param name="actionTargetPosition">The position where we check if the action is usable.</param>
    /// <returns></returns>
    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return  spells.Count > 0 && currentSelectedSpell != null &&
                (currentSelectedSpell.CastType == SpellCastType.Fast || actionsLeftThisTurn > 0) &&
                IsActionUsable(nodeData.CurrentNode.worldPosition, actionTargetPosition, currentSelectedSpell);
    }

    public bool IsActionUsable(Vector2 actionCasterPosition, Vector2 actionTargetPosition, SpellScriptable spellToCheck)
    {
        return  (spellToCheck.CastType == SpellCastType.Fast || actionsLeftThisTurn > 0) &&
                spellToCheck != null &&
                spellToCheck.IsUsable &&
                (ressource == null || spellToCheck.RessourceCost <= ressource.CurrentAmount) &&
                Grid.IsNodeVisible(Grid.GetNodeFromWorldPoint(actionCasterPosition), Grid.GetNodeFromWorldPoint(actionTargetPosition), spellToCheck.Range);
    }

    /// <summary>
    /// Reset the action datas.
    /// </summary>
    public override void ResetData()
    {
        SetActionLeft(actionByTurn);
        currentSelectedSpell = null;

        hasOpportunityAttack = opportunitySpell != null;
    }

    public void OpportunityAttack(Vector2 actionTargetPosition, Action callback)
    {
        if (opportunitySpell != null)
        {
            onOpportunityAttack?.Invoke();

            currentSelectedSpell = opportunitySpell;

            TryDoAction(actionTargetPosition, callback);
        }
    }

    /// <summary>
    /// Use a spell if the target is available.
    /// </summary>
    /// <param name="actionTargetPosition">The position of the target wanted.</param>
    /// <param name="callback">The callback to play once the spell end.</param>
    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        LaunchedSpellData launchedSpell = new LaunchedSpellData(currentSelectedSpell, this, Grid.GetNodeFromWorldPoint(actionTargetPosition));

        if (currentSelectedSpell != null && RVN_SpellManager.CanUseSpell(launchedSpell))
        {
            CastSpell(launchedSpell, callback);

            if(launchedSpell.scriptable.IsCooldownGlobal)
            {
                for(int i = 1; i < spells.Count; i++)
                {
                    spells[i].SetCooldown(launchedSpell.scriptable.StartCooldown);
                }
            }
            else
            {
                launchedSpell.scriptable.ResetCooldown();
            }

            if (launchedSpell.scriptable.CastType != SpellCastType.Fast)
            {
                StopMovementAction();
                SetActionLeft(actionsLeftThisTurn - 1);
            }
            else
            {
                SetActionLeft(actionsLeftThisTurn);
            }
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
    public void SelectSpell(int spellIndex, bool displayAction = true)
    {
        SpellScriptable lastSpell = currentSelectedSpell;
        if (currentSelectedSpell != null)
        {
            UnselectSpell();
        }

        if (spellIndex >= 0 && spells[spellIndex] != lastSpell)
        {
            currentSelectedSpell = spells[spellIndex];

            actOnSelectSpell?.Invoke(currentSelectedSpell);
        }

        if (displayAction)
        {
            DisplayAction(RVN_InputController.MousePosition);
        }
    }

    public void UnselectSpell()
    {
        UndisplayAction(RVN_InputController.MousePosition);

        actOnUnselectSpell?.Invoke(currentSelectedSpell);

        currentSelectedSpell = null;
    }

    /// <summary>
    /// Prepare a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to cast.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void CastSpell(LaunchedSpellData launchedSpell, Action callback)
    {
        ressource?.UseRessource(launchedSpell.scriptable.RessourceCost);
        launchedSpell.scriptable.UseSpell();

        OnCastSpell?.Invoke(launchedSpell);
        handler.animationController?.PlayAnimation(launchedSpell.scriptable.CastingAnimation.ToString(), launchedSpell);

        OnCastSpellToDiection?.Invoke(launchedSpell.targetNode.worldPosition - launchedSpell.caster.transform.position);

        if (launchedSpell.scriptable.Projectile != null)
        {
            SpellProjectile projectile = Instantiate(launchedSpell.scriptable.Projectile);

            projectile.SetProjectile(launchedSpell.caster.CurrentNode, launchedSpell.targetNode, () => UseSpell(launchedSpell, callback));
        }
        else
        {
            TimerManager.CreateGameTimer(launchedSpell.scriptable.CastDuration, () => UseSpell(launchedSpell, callback));
        }
    }

    /// <summary>
    /// Launch a spell at the target position.
    /// </summary>
    /// <param name="launchedSpell">The spell to launch.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void UseSpell(LaunchedSpellData launchedSpell, Action callback)
    {
        if (launchedSpell.scriptable.Projectile == null)
        {
            handler.animationController?.PlayAnimation(launchedSpell.scriptable.LaunchSpellAnimation.ToString(), launchedSpell);
        }

        RVN_SpellManager.UseSpell(launchedSpell, () => EndUseSpell(callback));
    }

    /// <summary>
    /// End a spell.
    /// </summary>
    /// <param name="callback">The callback to call.</param>
    private void EndUseSpell(Action callback)
    {
        callback?.Invoke();

        actOnEndCastSpell?.Invoke();

        SelectSpell(-1);
    }

    public override void SetData(CPN_Data_SpellCaster toSet)
    {
        actionByTurn = toSet.MaxSpellUseByTurn();

        spells = new List<SpellScriptable>();
        foreach (SpellScriptable spell in toSet.AvailableSpells())
        {
            spells.Add(Instantiate(spell));

            spells[spells.Count - 1].SetSpell();
        }

        opportunitySpell = toSet.OpportunitySpell();

        ressource = toSet.Ressource();
        ressource?.Initialize();

        offensiveRerolls = toSet.OffensiveRerolls();

        accuracy = toSet.Accuracy();
        power = toSet.Power();

        ResetData();
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

    //CODE REVIEW : Voir si il faut mettre cette fonction autre part pour éviter le lien entre Mouvement et SpellCast
    private void StopMovementAction()
    {
        if(handler.TryGetComponent<CPN_Movement>(out CPN_Movement movement))
        {
            if(movement.MaxMovement != movement.MovementLeft)
            {
                movement.SetCurrentMovement(0);
            }
        }
    }
}
