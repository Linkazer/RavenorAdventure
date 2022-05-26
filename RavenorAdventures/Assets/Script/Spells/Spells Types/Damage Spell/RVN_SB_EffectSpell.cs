using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_EffectSpell : RVN_SpellBehavior<RVN_SS_EffectSpell>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return true;
    }

    protected override bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        Debug.Log("Use effect spell");
        return true;
    }
}
