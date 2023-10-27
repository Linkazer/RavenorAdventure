using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_LockSpell : Effect
{
    [SerializeField] private SpellScriptable spellToLock;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            if (spellToLock != null)
            {
                caster.LockSpell(spellToLock, true);
            }
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            if (spellToLock != null)
            {
                caster.LockSpell(spellToLock, false);
            }
        }
    }
}
