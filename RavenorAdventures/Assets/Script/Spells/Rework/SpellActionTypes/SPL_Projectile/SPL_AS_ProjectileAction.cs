using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_AS_ProjectileAction : SPL_SpellAction
{
    [SerializeField] private SpellProjectile projectileToUse;

    [SerializeField] private SPL_SpellAcionChooser actionOnReachTarget;

    public SpellProjectile ProjectileToUse => projectileToUse;

    public SPL_SpellAction GetActionOnReachTarget(SPL_CastedSpell castedSpellData)
    {
        return actionOnReachTarget.GetFirstUsableAction(castedSpellData);
    }
}