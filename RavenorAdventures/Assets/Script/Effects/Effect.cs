using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    [SerializeField] private string name;

    [SerializeField] private List<EFFTYPE_StatEffect> statEffects;
    [SerializeField] private List<EFFTYPE_ActionEffect> actionEffects;

    public string Name => name;

    public void ApplyEffect(RVN_ComponentHandler target)
    {
        foreach (EFFTYPE_StatEffect eff in statEffects)
        {
            eff.ApplyEffect(target);
        }

        foreach(EFFTYPE_ActionEffect eff in actionEffects)
        {
            eff.ApplyEffect(target);
        }
    }

    public void RemoveEffect(RVN_ComponentHandler target)
    {
        foreach (EFFTYPE_StatEffect eff in statEffects)
        {
            eff.RemoveEffect(target);
        }

        foreach (EFFTYPE_ActionEffect eff in actionEffects)
        {
            eff.RemoveEffect(target);
        }
    }
}
