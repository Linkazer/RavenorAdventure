using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_AddSpell : Effect
{
    [SerializeField] private SPL_SpellScriptable spellToAdd;
    [SerializeField] private SPL_SpellScriptable spellToChange;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if(effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            if (spellToChange != null)
            {
                caster.ChangeSpell(spellToAdd, spellToChange);
            }
            else
            {
                caster.AddSpell(spellToAdd);
            }
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            if (spellToChange != null)
            {
                caster.ResetChangeSpell(spellToAdd);
            }
            else
            {
                caster.RemoveSpell(spellToAdd);
            }
        }
    }
}
