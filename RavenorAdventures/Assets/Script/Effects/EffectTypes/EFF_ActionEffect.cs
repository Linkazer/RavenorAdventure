using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_ActionEffect : Effect
{
    public SPL_SpellScriptable spellToUse;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        SPL_CastedSpell castedSpell = new SPL_CastedSpell(spellToUse, null, Grid.GetNodeFromWorldPoint(effectTarget.transform.position));

        SPL_SpellResolverManager.Instance.ResolveSpell(castedSpell, null);
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {

    }
}

