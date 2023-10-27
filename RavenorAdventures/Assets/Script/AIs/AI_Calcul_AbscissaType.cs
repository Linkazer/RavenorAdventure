using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AI_Calcul_AbscissaType
{
    public abstract float GetAbcissaValue(Ai_PlannedAction plannedAction);
}

public class AI_CA_DistanceFromTarget_CalculatedPosition : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        return Pathfinding.GetDistance(plannedAction.movementTarget, plannedAction.spellNodeTarget);
    }
}

public class AI_CA_IsTargetVisible_BasePositiona : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        return Grid.IsNodeVisible(plannedAction.caster.CurrentNode, plannedAction.spellNodeTarget) ? 1 : 0;
    }
}

public class AI_CA_TargetMaxHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetHealth))
        {
            return targetHealth.MaxHealth;
        }

        return 0;
    }
}

public class AI_CA_TargetCurrentHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetHealth))
        {
            return targetHealth.CurrentHealth;
        }

        return 0;
    }
}

public class AI_CA_TargetPercentHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetHealth))
        {
            return targetHealth.CurrentHealth / targetHealth.MaxHealth;
        }

        return 0;
    }
}

public class AI_CA_CasterMaxHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.caster != null && plannedAction.caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterHealth))
        {
            return casterHealth.MaxHealth;
        }

        return 0;
    }
}

public class AI_CA_CasterCurrentHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.caster != null && plannedAction.caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterHealth))
        {
            return casterHealth.CurrentHealth;
        }

        return 0;
    }
}

public class AI_CA_CasterPercentHp : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.caster != null && plannedAction.caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterHealth))
        {
            return casterHealth.CurrentHealth / casterHealth.MaxHealth;
        }

        return 0;
    }
}

public class AI_CA_TargetDangerosity : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        return RVN_AiBattleManager.CalculateDangerosityVulnerability(plannedAction.actualTarget, true);
    }
}

public class AI_CA_TargetVulnerability : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        return RVN_AiBattleManager.CalculateDangerosityVulnerability(plannedAction.actualTarget, false);
    }
}

public class AI_CA_TargetMaxArmor : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetCurrentArmor))
        {
            return targetCurrentArmor.MaxArmor;
        }

        return 0;
    }
}

public class AI_CA_TargetCurrentArmor : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetCurrentArmor))
        {
            return targetCurrentArmor.CurrentArmor;
        }

        return 0;
    }
}

public class AI_CA_NumberEnnemyArea : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        float toReturn = 0;

        foreach (Node n in plannedAction.hitedTargets)
        {
            if (n != plannedAction.caster.CurrentNode)
            {
                List<CPN_Character> enemiesInArea = n.GetNodeComponent<CPN_Character>();

                foreach (CPN_Character c in enemiesInArea)
                {
                    if (!RVN_BattleManager.AreCharacterAllies(plannedAction.caster, c))
                    {
                        toReturn++;
                    }
                }
            }
        }

        return toReturn;
    }
}

public class AI_CA_NumberAllyArea : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        float toReturn = 0;

        foreach (Node n in plannedAction.hitedTargets)
        {
            if (n != plannedAction.caster.CurrentNode)
            {
                List<CPN_Character> alliesInArea = n.GetNodeComponent<CPN_Character>();

                foreach (CPN_Character c in alliesInArea)
                {
                    if (RVN_BattleManager.AreCharacterAllies(plannedAction.caster, c))
                    {
                        toReturn++;
                    }
                }
            }
        }

        return toReturn;
    }
}

public class AI_CA_DistranceFromTarget_BasePosition : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        return Pathfinding.GetDistance(plannedAction.caster.CurrentNode, plannedAction.spellNodeTarget);
    }
}


public class AI_CA_MovementToMake : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        Pathfinding.CalculatePathfinding(plannedAction.caster.CurrentNode, plannedAction.movementTarget, -1);
        return plannedAction.movementTarget.gCost;
    }
}

public class AI_CA_TargetHasEffect : AI_Calcul_AbscissaType
{
    [SerializeField] private EffectScriptable effectToCheck;

    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if(plannedAction.actualTarget != null && plannedAction.actualTarget.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
        {
            if(effectHandler.HasEffect(effectToCheck))
            {
                return 1;
            }
        }

        return 0;
    }
}

public class AI_CA_ActionLeft : AI_Calcul_AbscissaType
{
    public override float GetAbcissaValue(Ai_PlannedAction plannedAction)
    {
        if (plannedAction.caster != null && plannedAction.caster.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            return caster.ActionLeftThisTurn;
        }

        return 0;
    }
}