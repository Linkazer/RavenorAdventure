using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellRessourceType
{
    None = -1,
    Mana = 0,
    Rage = 1,
    Combo = 2,
}

[Serializable]
public class SpellRessource
{
    [SerializeField] private int startAmount;
    [SerializeField] private Vector2Int limits;

    [SerializeField] private int currentAmount;

    public Action<int> actOnRessourceUpdate;

    public virtual SpellRessourceType RessourceType => SpellRessourceType.None;

    public int CurrentAmount => currentAmount;

    public virtual void Initialize(CPN_Character characterLinked)
    {
        currentAmount = startAmount;
    }

    public bool HasEnoughRessource(int amountWanted)
    {
        return amountWanted >= currentAmount;
    }

    public void UseRessource(int amountToUse)
    {
        currentAmount -= amountToUse;

        if(currentAmount < limits.x)
        {
            currentAmount = limits.x;
        }

        actOnRessourceUpdate?.Invoke(currentAmount);
    }

    public void RegainRessource(int amountToRegain)
    {
        currentAmount += amountToRegain;

        if(currentAmount > limits.y)
        {
            currentAmount = limits.y;
        }

        actOnRessourceUpdate?.Invoke(currentAmount);
    }
}

public class SpellRessource_Maana : SpellRessource
{
    public override SpellRessourceType RessourceType => SpellRessourceType.Mana;
}

public class SpellRessource_Combo : SpellRessource
{
    public override SpellRessourceType RessourceType => SpellRessourceType.Combo;

    public override void Initialize(CPN_Character characterLinked)
    {
        base.Initialize(characterLinked);

        if(characterLinked != null)
        {
            if(characterLinked.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                caster.actOnDealDamageSelf += OnTouchTarget;
            }
        }
    }

    private void OnTouchTarget(RVN_ComponentHandler casterHandler)
    {
        //RegainRessource(1);
    }
}