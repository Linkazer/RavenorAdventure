using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static AIC_Conditional;

[Serializable]
public class SPL_CCondition_CharacterStat : SPL_SpellActionChooserCondition
{
    [Serializable]
    public struct StatCondition
    {
        public StatEnum stat;
        public ConditionalWanted condition;
        public float valueToCheck;
    }

    [SerializeField] private SPL_CCondition_ConditionTarget conditionTarget;
    [SerializeField] private StatCondition[] calculCondition;

    public override bool IsConditionValid(SPL_CastedSpell castedSpellData)
    {
        List<CPN_Character> handlerInTargetNode = new List<CPN_Character>();

        switch (conditionTarget)
        {
            case SPL_CCondition_ConditionTarget.Caster:
                if (castedSpellData.Caster != null && castedSpellData.Caster.Handler is CPN_Character)
                {
                    handlerInTargetNode = new List<CPN_Character>() { castedSpellData.Caster.Handler as CPN_Character };
                }
                break;
            case SPL_CCondition_ConditionTarget.Target:
                handlerInTargetNode = castedSpellData.TargetNode.GetNodeHandler<CPN_Character>();
                break;
        }

        foreach (RVN_ComponentHandler handler in handlerInTargetNode)
        {
            bool result = false;

            foreach (StatCondition condition in calculCondition)
            {
                if(CheckCondition(condition, handler))
                {
                    result = true;
                }
            }

            if(result == true)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckCondition(StatCondition conditionToCheck, RVN_ComponentHandler targetHandler)
    {
        float statValue = GetStatValue(conditionToCheck.stat, targetHandler);

        switch (conditionToCheck.condition)
        {
            case ConditionalWanted.More:
                if (statValue > conditionToCheck.valueToCheck)
                {
                    return true;
                }
                break;
            case ConditionalWanted.MoreOrEqual:
                if (statValue >= conditionToCheck.valueToCheck)
                {
                    return true;
                }
                break;
            case ConditionalWanted.Equal:
                if (statValue == conditionToCheck.valueToCheck)
                {
                    return true;
                }
                break;
            case ConditionalWanted.LessOrEqual:
                if (statValue <= conditionToCheck.valueToCheck)
                {
                    return true;
                }
                break;
            case ConditionalWanted.Less:
                if (statValue < conditionToCheck.valueToCheck)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    private float GetStatValue(StatEnum statToGet, RVN_ComponentHandler targetHandler)
    {
        switch(statToGet)
        {
            case StatEnum.MaxHealth:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthMaxHealth))
                {
                    return healthMaxHealth.MaxHealth;
                }
                break;
            case StatEnum.BaseDamage:

                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster damageCaster))
                {
                    return damageCaster.Power;
                }
                break;
            case StatEnum.Accuracy:
                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster accuracyCaster))
                {
                    return accuracyCaster.Accuracy;
                }
                break;
            case StatEnum.OffensiveRerolls:
                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster rerollCaster))
                {
                    return rerollCaster.OffensiveRerolls;
                }
                break;
            case StatEnum.OffensiveRerollsMalus:
                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster rerollMalusCaster))
                {
                    return rerollMalusCaster.OffensiveRerollsMalus;
                }
                break;
            case StatEnum.ActionByTurn:
                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster actionCaster))
                {
                    return actionCaster.ActionByTurn;
                }
                break;
            case StatEnum.Armor:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthArmor))
                {
                    return healthArmor.CurrentArmor;
                }
                break;
            case StatEnum.MaxArmor:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthMaxArmor))
                {
                    return healthMaxArmor.MaxArmor;
                }
                break;
            case StatEnum.Defense:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefense))
                {
                    return healthDefense.Defense;
                }
                break;
            case StatEnum.DefenseiveRerolls:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefensiveReroll))
                {
                    return healthDefensiveReroll.DefensiveRerolls;
                }
                break;
            case StatEnum.DefensiveRerollsMalus:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthDefensiveRerollMalus))
                {
                    return healthDefensiveRerollMalus.DefensiveRerollsMalus;
                }
                break;
            case StatEnum.Movement:
                if (targetHandler.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                {
                    return movement.MovementLeft;
                }
                break;
            case StatEnum.SpellRessource:
                if (targetHandler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster ressourceCaster))
                {
                    return ressourceCaster.Ressource != null ? ressourceCaster.Ressource.CurrentAmount : 0;
                }
                break;
            case StatEnum.MaxMovement:
                if (targetHandler.GetComponentOfType<CPN_Movement>(out CPN_Movement maxMovement))
                {
                    return maxMovement.MaxMovement;
                }
                break;
            case StatEnum.CurrentHealth:
                if (targetHandler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthCurrent))
                {
                    return healthCurrent.CurrentHealth;
                }
                break;
        }

        return 0;
    }
}
