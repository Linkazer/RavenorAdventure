using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.WSA;

public class CPN_SpellCaster : CPN_CharacterAction<CPN_Data_SpellCaster>
{
    private SPL_SpellHolder opportunitySpell;
    private List<SPL_SpellHolder> spells;
    [SerializeField] private NodeDataHanlder nodeData;

    [SerializeField] private UnityEvent<SPL_CastedSpell> OnCastSpell;
    [SerializeField] private UnityEvent<Vector2> OnCastSpellToDiection;
    [SerializeField] private UnityEvent onOpportunityAttack;

    [SerializeField] private int actionsLeftThisTurn = 1;
    [SerializeField] private int actionByTurn = 1;
    private SPL_SpellHolder currentSelectedSpell = null;

    [HideInInspector] public bool hasOpportunityAttack = true;

    //Base datas
    [SerializeField] private int offensiveRerolls;
    [SerializeField] private int offensiveRerollsMalus;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;

    [SerializeField] private SpellRessource ressource;

    private Dictionary<int, SPL_SpellHolder> changeSpellWithIndex = new Dictionary<int, SPL_SpellHolder>();

    private List<DamageOverrider> doneDamageOverriders = new List<DamageOverrider>();

    public Action<RVN_ComponentHandler> actOnDealDamageSelf;
    public Action<RVN_ComponentHandler> actOnDealDamageTarget;
    public Action<RVN_ComponentHandler> actOnUseSkillSelf;
    public Action<RVN_ComponentHandler> actOnUseSkillTarget;
    public Action actOnEndCastSpell;
    public Action<int, int> actOnSetActionLeft;
    public Action<SPL_SpellHolder> actOnSelectSpell;
    public Action<SPL_SpellHolder> actOnUnselectSpell;
    public Action<CPN_SpellCaster> actOnUpdateSpell;

    public List<SPL_SpellHolder> Spells => spells;
    public SPL_SpellHolder OpportunitySpell => opportunitySpell;
    public int ActionByTurn => actionByTurn;
    public int ActionLeftThisTurn => actionsLeftThisTurn;
    public int OffensiveRerolls => offensiveRerolls;
    public int OffensiveRerollsMalus => offensiveRerollsMalus;
    public int Accuracy => accuracy;
    public int Power => power;
    public List<DamageOverrider> DoneDamageOverriders => doneDamageOverriders;

    public SpellRessource Ressource => ressource;

    public Node CurrentNode => nodeData.CurrentNode;

    protected override CPN_Data_SpellCaster GetDataFromHandler()
    {
        if (handler is CPN_Character)
        {
            return (handler as CPN_Character).Scriptable;
        }

        return null;
    }

    protected override void SetData(CPN_Data_SpellCaster toSet)
    {
        actionByTurn = toSet.MaxSpellUseByTurn();

        spells = new List<SPL_SpellHolder>();
        foreach (SPL_SpellScriptable spell in toSet.AvailableSpells())
        {
            AddSpell(spell);
        }

        if (toSet.OpportunitySpell() != null)
        {
            opportunitySpell = new SPL_SpellHolder(toSet.OpportunitySpell());
        }

        ressource = toSet.Ressource();
        ressource?.Initialize(handler as CPN_Character);

        offensiveRerolls = toSet.OffensiveRerolls();

        accuracy = toSet.Accuracy();
        power = toSet.Power();

        ResetData();
    }

    public override void Activate()
    {
        
    }

    public override void Disactivate()
    {

    }

    public override void OnStartHandlerGroupRound()
    {
        base.OnStartHandlerGroupRound();

        foreach (SPL_SpellHolder spell in spells)
        {
            spell.OnCaterBeginTurn();
        }
    }

    public override void OnEndHandlerGroupRound()
    {
        base.OnEndHandlerGroupRound();

        foreach (SPL_SpellHolder spell in spells)
        {
            spell.UpdateCurrentCooldown();
        }
    }

    public void AddOffensiveRerolls(int amount)
    {
        offensiveRerolls += amount;
    }

    public void AddOffensiveRerollsMalus(int amount)
    {
        offensiveRerollsMalus += amount;
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

        actOnSetActionLeft?.Invoke(actionByTurn, actionsLeftThisTurn);
    }

    public override void UnselectAction()
    {
        UnselectSpell();
    }

    /// <summary>
    /// Display every node on which the action can be used.
    /// </summary>
    /// <param name="actionTargetPosition">The current target position the player aim for.</param>
    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (currentSelectedSpell != null && currentSelectedSpell.IsUsable() && (ressource == null || currentSelectedSpell.SpellData.RessourceCost <= ressource.CurrentAmount))
        {
            List<Node> possibleTargetZone = Pathfinding.GetAllNodeInDistance(nodeData.CurrentNode, currentSelectedSpell.SpellData.Range, true);

            RVN_GridDisplayer.SetGridFeedback(possibleTargetZone, Color.blue);

            if (possibleTargetZone.Contains(Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition)))
            {
                List<Node> zoneNodes = currentSelectedSpell.SpellData.GetDisplayzone(CurrentNode, Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition));
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
        return spells.Count > 0 && currentSelectedSpell != null && currentSelectedSpell.IsUsable() && (ressource == null || currentSelectedSpell.SpellData.RessourceCost <= ressource.CurrentAmount);
    }

    /// <summary>
    /// Check if the action can be used at the target position.
    /// </summary>
    /// <param name="actionTargetPosition">The position where we check if the action is usable.</param>
    /// <returns></returns>
    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return  spells.Count > 0 && currentSelectedSpell != null &&
                IsActionUsable(nodeData.CurrentNode.worldPosition, actionTargetPosition, currentSelectedSpell);
    }

    public bool IsActionUsable(Vector2 actionCasterPosition, Vector2 actionTargetPosition, SPL_SpellScriptable spellToCheck)
    {
        return IsActionUsable(actionCasterPosition, actionTargetPosition, GetOrCreateSpellHolderForScriptable(spellToCheck));
    }

    public bool IsActionUsable(Vector2 actionCasterPosition, Vector2 actionTargetPosition, SPL_SpellHolder spellToCheck)
    {
        return  spellToCheck.CanBeCasted(actionsLeftThisTurn) &&
                spellToCheck != null &&
                spellToCheck.IsUsable() &&
                (ressource == null || spellToCheck.SpellData.RessourceCost <= ressource.CurrentAmount) &&
                Grid.IsNodeVisible(Grid.GetNodeFromWorldPoint(actionCasterPosition), Grid.GetNodeFromWorldPoint(actionTargetPosition), spellToCheck.SpellData.Range);
    }

    public override void OnStartRound()
    {
        base.OnStartRound();

        SetActionLeft(actionByTurn);
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
    public override bool TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        if (currentSelectedSpell != null)// && RVN_SpellManager.CanUseSpell(castedSpell)) //TODO Spell Rework : A voir si le CanUseSpell est encore d'actualité
        {
            CastSpell(currentSelectedSpell, actionTargetPosition, callback);

            return true;
        }
        else
        {
            Debug.Log("Invoke without spell");
            callback?.Invoke();
        }

        return false;
    }

    public bool CanSpellBeUsed(int spellIndex)
    {
        return CanSpellBeUsed(spells[spellIndex]);
    }

    public bool CanSpellBeUsed(SPL_SpellHolder spellToCheck)
    {
        return  spellToCheck.CanBeCasted(actionsLeftThisTurn)
                && (spellToCheck.IsUsable())
                && (Ressource == null || spellToCheck.SpellData.RessourceCost <= Ressource.CurrentAmount);
    }

    private SPL_SpellHolder GetOrCreateSpellHolderForScriptable(SPL_SpellScriptable scriptable)
    {
        foreach(SPL_SpellHolder holder in spells)
        {
            if (holder.SpellData == scriptable)
            {
                return holder;
            }
        }

        return new SPL_SpellHolder(scriptable);
    }

    /// <summary>
    /// Select a spell.
    /// </summary>
    /// <param name="spellIndex">The index of the spell to choose.</param>
    /// <returns> Return TRUE if a spell has been select. </returns>
    public bool SelectSpell(int spellIndex, bool displayAction = true)
    {
        if (spellIndex < spells.Count && spellIndex >= 0)
        {
            return SelectSpell(spells[spellIndex], displayAction);
        }

        return false;
    }

    public bool SelectSpell(SPL_SpellScriptable spellScriptable, bool displayAction = true)
    {
        return SelectSpell(GetOrCreateSpellHolderForScriptable(spellScriptable), displayAction);
    }

    public bool SelectSpell(SPL_SpellHolder spellToSelect, bool displayAction = true)
    {
        bool hasSelectedSpell = false;

        SPL_SpellHolder lastSpell = currentSelectedSpell;
        if (currentSelectedSpell != null)
        {
            UnselectSpell();
        }

        if (spellToSelect != lastSpell)
        {
            if (CanSpellBeUsed(spellToSelect))
            {
                currentSelectedSpell = spellToSelect;

                actOnSelectSpell?.Invoke(currentSelectedSpell);

                hasSelectedSpell = true;
            }
        }

        if (displayAction)
        {
            DisplayAction(RVN_InputController.MousePosition);
        }

        return hasSelectedSpell;
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
    /// <param name="spellToCast">The spell to cast.</param>
    /// <param name="actionTargetPosition">The target position of the spell.</param>
    /// <param name="callback">The callback to call after the spell is done.</param>
    private void CastSpell(SPL_SpellHolder spellToCast, Vector2 actionTargetPosition, Action callback)
    {
        SPL_CastedSpell castedSpell = new SPL_CastedSpell(currentSelectedSpell.SpellData, this, Grid.GetNodeFromWorldPoint(actionTargetPosition));

        if (spellToCast.DoesCostAction() && RVN_RoundManager.Instance.CurrentRoundMode != RVN_RoundManager.RoundMode.RealTime)
        {
            spellToCast.UseSpell();
            SetActionLeft(actionsLeftThisTurn - 1);
        }
        else
        {
            spellToCast.UseSpell();
            SetActionLeft(actionsLeftThisTurn);
        }

        ressource?.UseRessource(spellToCast.SpellData.RessourceCost);


        OnCastSpell?.Invoke(castedSpell);
        
        OnCastSpellToDiection?.Invoke(castedSpell.TargetNode.worldPosition - transform.position);

        SPL_SpellResolverManager.Instance.ResolveSpell(castedSpell, () => EndUseSpell(callback));
    }

   /* /// <summary>
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
    }*/

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

    public void AddSpell(SPL_SpellScriptable toAdd)
    {
        spells.Add(new SPL_SpellHolder(toAdd));

        actOnUpdateSpell?.Invoke(this);
    }

    public void LockSpell(SPL_SpellScriptable toLock, bool lockState)
    {
        Debug.Log("Try lock spell : " + toLock);
        for (int i = 0; i < spells.Count; i++)
        {
            Debug.Log("Contains check : " + spells[i].SpellID + " C " + toLock.name);
            if (spells[i].SpellID.Contains(toLock.name))
            {
                Debug.Log("Set lock state : " + lockState);
                spells[i].LockSpell(lockState);

                actOnUpdateSpell?.Invoke(this);
                break;
            }
        }
    }

    public void RemoveSpell(SPL_SpellScriptable toRemove)
    {
        for(int i = 0; i < spells.Count;i++)
        {
            if(spells[i].SpellID.Contains(toRemove.name))
            {
                spells.RemoveAt(i);

                actOnUpdateSpell?.Invoke(this);
                break;
            }
        }
    }

    public void ChangeSpell(SPL_SpellScriptable toSet, SPL_SpellScriptable toChange)
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (spells[i].SpellID.Contains(toChange.name))
            {
                changeSpellWithIndex.Add(i, spells[i]);
                spells[i] = new SPL_SpellHolder(toSet);

                actOnUpdateSpell?.Invoke(this);
                break;
            }
        }
    }

    public void ResetChangeSpell(SPL_SpellScriptable toSet)
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (spells[i].SpellID.Contains(toSet.name))
            {
                spells[i] = changeSpellWithIndex[i];

                changeSpellWithIndex.Remove(i);

                actOnUpdateSpell?.Invoke(this);
                break;
            }
        }
    }

    public void UpdateCooldowns() //A déplacer dans une des fonctions de Round
    {
        Debug.Log("Should not pass here");
    }

    public void AddDamageOverrider(DamageOverrider toAdd)
    {
        doneDamageOverriders.Add(toAdd);
    }

    public void RemoveDamageOverrider(DamageOverrider toRemove)
    {
        doneDamageOverriders.Remove(toRemove);
    }

    //CODE REVIEW : Voir comment on peut faire pour éviter de juste avoir les events ici ?
    public void DealDamage(CPN_HealthHandler target)
    {
        actOnDealDamageSelf?.Invoke(Handler);
        actOnDealDamageTarget?.Invoke(target.Handler);
    }
}
