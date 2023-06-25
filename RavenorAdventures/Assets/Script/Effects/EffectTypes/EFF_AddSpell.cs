using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_AddSpell : Effect
{
    [SerializeField] private SpellScriptable spellToAdd;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if(effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.AddSpell(spellToAdd);
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.RemoveSpell(spellToAdd);
        }
    }
}
