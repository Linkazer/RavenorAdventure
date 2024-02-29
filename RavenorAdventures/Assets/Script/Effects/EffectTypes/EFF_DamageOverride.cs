using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageOverrider
{
    public SPL_DamageData damageOverrider;
}

public class EFF_DamageOverride : Effect
{
    [SerializeField] private DamageOverrider[] doneDamageOverriders;
    [SerializeField] private DamageOverrider[] receivedDamageOverriders;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster targetCaster))
        {
            foreach (DamageOverrider dmgOverrider in doneDamageOverriders)
            {
                targetCaster.AddDamageOverrider(dmgOverrider);
            }
        }

        if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetHealth))
        {
            foreach (DamageOverrider dmgOverrider in receivedDamageOverriders)
            {
                targetHealth.AddReceivedDamageOverrider(dmgOverrider);
            }
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster targetCaster))
        {
            foreach (DamageOverrider dmgOverrider in doneDamageOverriders)
            {
                targetCaster.RemoveDamageOverrider(dmgOverrider);
            }
        }

        if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetHealth))
        {
            foreach (DamageOverrider dmgOverrider in receivedDamageOverriders)
            {
                targetHealth.RemoveReceivedDamageOverrider(dmgOverrider);
            }
        }
    }
}
