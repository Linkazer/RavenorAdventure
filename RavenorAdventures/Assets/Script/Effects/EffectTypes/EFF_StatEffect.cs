using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_StatEffect : Effect
{
    [Serializable]
    public struct StatEffect
    {
        public StatEnum stat;
        public float valueToChange;
    }
    
    [SerializeField] private List<StatEffect> effects;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        ApplyBonus(effectTarget, 1);
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        ApplyBonus(effectTarget, -1);
    }

    private void ApplyBonus(RVN_ComponentHandler effectTarget, float multiplier)
    {
        foreach (StatEffect statEff in effects)
        {
            float value = statEff.valueToChange * multiplier;

            switch (statEff.stat)
            {
                case StatEnum.MaxHealth:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthMaxHealth))
                    {
                        if (value > 0)
                        {
                            healthMaxHealth.AddMaxHealth((int)value);
                        }
                        else if (value < 0)
                        {
                            if(-value < healthMaxHealth.MaxHealth)
                            {
                                healthMaxHealth.RemoveMaxHealth((int)-value);
                            }
                        }
                    }
                    break;
                case StatEnum.BaseDamage:

                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster damageCaster))
                    {
                        damageCaster.AddPower((int)value);
                    }
                    break;
                case StatEnum.Accuracy:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster accuracyCaster))
                    {
                        accuracyCaster.AddAccuracy((int)value);
                    }
                    break;
                case StatEnum.OffensiveRerolls:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster rerollCaster))
                    {
                        rerollCaster.AddOffensiveRerolls((int)value);
                    }
                    break;
                case StatEnum.OffensiveRerollsMalus:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster rerollMalusCaster))
                    {
                        rerollMalusCaster.AddOffensiveRerollsMalus((int)value);
                    }
                    break;
                case StatEnum.ActionByTurn:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster actionCaster))
                    {
                        actionCaster.AddBonusAction((int)value);
                    }
                    break;
                case StatEnum.Armor:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthArmor))
                    {
                        if (value > 0)
                        {
                            healthArmor.AddArmor((int)value);
                        }
                        else if(value < 0)
                        {
                            healthArmor.RemoveArmor((int)-value);
                        }
                    }
                    break;
                case StatEnum.MaxArmor:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthMaxArmor))
                    {
                        if (value > 0)
                        {
                            healthMaxArmor.AddMaxArmor((int)value);
                        }
                        else if (value < 0)
                        {
                            healthMaxArmor.RemoveMaxArmor((int)-value);
                        }
                    }
                    break;
                case StatEnum.Defense:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefense))
                    {
                        healthDefense.AddDefense((int)value);
                    }
                    break;
                case StatEnum.DefenseiveRerolls:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefensiveReroll))
                    {
                        healthDefensiveReroll.AddDefensiveRerolls((int)value);
                    }
                    break;
                case StatEnum.DefensiveRerollsMalus:
                    if (effectTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefensiveRerollMalus))
                    {
                        healthDefensiveRerollMalus.AddDefensiveRerollsMalus((int)value);
                    }
                    break;
                case StatEnum.Movement:
                    if (effectTarget.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                    {
                        movement.AddMovement((int)value);
                    }
                    break;
                case StatEnum.SpellRessource:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster ressourceCaster))
                    {
                        if (value > 0)
                        {
                            ressourceCaster.Ressource.RegainRessource((int)value);
                        }
                        else
                        {
                            ressourceCaster.Ressource.UseRessource((int)value);
                        }
                    }
                    break;
            }
        }
    }
}
