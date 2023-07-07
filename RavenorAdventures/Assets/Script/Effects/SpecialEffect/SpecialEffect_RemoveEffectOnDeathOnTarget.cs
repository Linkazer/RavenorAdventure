using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect_RemoveEffectOnDeathOnTarget : MonoBehaviour
{
    [SerializeField] private EffectScriptable effectToRemove;
    [SerializeField] private CPN_HealthHandler characterHealthToCheck;

    private void Start()
    {
        characterHealthToCheck.actOnAttackReceivedTowardTarget += CheckDeath;
    }

    private void OnDestroy()
    {
        characterHealthToCheck.actOnAttackReceivedTowardTarget -= CheckDeath;
    }

    private void CheckDeath(RVN_ComponentHandler targetHandler)
    {
        if (characterHealthToCheck.CurrentHealth <= 0)
        {
            if (targetHandler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
            {
                effectHandler.RemoveEffect(effectToRemove);
            }

            characterHealthToCheck.actOnAttackReceivedTowardTarget -= CheckDeath;
        }
    }
}
