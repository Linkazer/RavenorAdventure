using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_DamageSpellBehavior : RVN_SpellBehavior<RVN_SD_DamageSpellData>
{
    protected override void OnEndSpell(RVN_SD_DamageSpellData spellToEnd)
    {
        Debug.Log("End spell : " + spellToEnd);
    }

    protected override bool OnIsSpellUsable(RVN_SD_DamageSpellData spellToCheck, Node targetNode)
    {
        Debug.Log("Is spell usable : " + spellToCheck + " at position : " + targetNode.worldPosition);
        return true;
    }

    protected override void OnUseSpell(RVN_SD_DamageSpellData spellToUse, Node targetNode, Action callback)
    {
        Debug.Log("Use Spell : " + spellToUse.Name + " at position : " + targetNode.worldPosition);

        TimerManager.CreateGameTimer(0.5f, callback);
    }
}
