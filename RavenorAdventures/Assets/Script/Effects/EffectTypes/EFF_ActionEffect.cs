using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_ActionEffect : Effect
{
    public SPL_SpellScriptable spellToUse;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        SPL_CastedSpell launchedSpell = new SPL_CastedSpell(spellToUse, null, Grid.GetNodeFromWorldPoint(effectTarget.transform.position));

        if (RVN_SpellManager.CanUseSpell(launchedSpell))
        {
            //RVN_SpellManager.UseSpell(launchedSpell, null); //TODO Spell Rework : A corriger
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {

    }
}

