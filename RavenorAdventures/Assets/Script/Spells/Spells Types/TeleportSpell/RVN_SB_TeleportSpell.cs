using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_TeleportSpell : RVN_SpellBehavior<RVN_SS_TeleportSpell>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return targetNode.IsWalkable;
    }

    protected override bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        if (spellToUse.caster.Handler.GetComponentOfType<CPN_Movement>(out CPN_Movement casterMovement))
        {
            casterMovement.Teleport(targetNode);

            callback?.Invoke();

            return true;
        }

        return false;
    }
}
