using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellRessourceType
{
    None,
    Mana,
    Rage,
}

[Serializable]
public class SpellRessource
{
    [SerializeField] private int startAmount;
    [SerializeField] private Vector2Int limits;

    [SerializeField] private int currentAmount;

    public virtual SpellRessourceType RessourceType => SpellRessourceType.None;

    public int CurrentAmount => currentAmount;

    public void Initialize()
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
    }

    public void RegainRessource(int amountToRegain)
    {
        currentAmount += amountToRegain;

        if(currentAmount > limits.y)
        {
            currentAmount = limits.y;
        }
    }
}

public class BaseRessource : SpellRessource
{

}
