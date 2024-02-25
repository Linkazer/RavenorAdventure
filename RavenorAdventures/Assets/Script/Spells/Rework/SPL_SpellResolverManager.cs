using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_SpellResolverManager : RVN_Singleton<SPL_SpellResolverManager>
{
    [SerializeField] private SPL_SpellActionBehavior[] spellActionBehaviors;

    private List<SPL_SpellResolver> currentlyRunningResolvers = new List<SPL_SpellResolver>();

    public SPL_SpellActionBehavior GetBehaviorForType(SPL_SpellAction toCheck)
    {
        if (toCheck != null)
        {
            for (int i = 0; i < spellActionBehaviors.Length; i++)
            {
                if (spellActionBehaviors[i].GetSpellType() == toCheck.GetType())
                {
                    return spellActionBehaviors[i];
                }
            }
        }

        return null;
    }

    public void ResolveSpell(SPL_CastedSpell castedSpell, Action callback)
    {
        currentlyRunningResolvers.Add(new SPL_SpellResolver(castedSpell, callback));
    }

    private void EndResolve(SPL_SpellResolver toEnd)
    {
        currentlyRunningResolvers.Remove(toEnd);
    }
}
