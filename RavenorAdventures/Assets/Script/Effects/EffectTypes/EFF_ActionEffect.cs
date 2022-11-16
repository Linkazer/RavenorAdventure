using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_ActionEffect : Effect
{
    public SpellScriptable spellToUse;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        LaunchedSpellData launchedSpell = new LaunchedSpellData(spellToUse, null, Grid.GetNodeFromWorldPoint(effectTarget.transform.position));

        if (RVN_SpellManager.CanUseSpell(launchedSpell))
        {
            RVN_SpellManager.UseSpell(launchedSpell, null);
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {

    }
}

