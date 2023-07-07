using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIC_Conditional : AI_Calcul
{
    public enum ConditionalWanted
    {
        More,
        MoreOrEqual, 
        Equal,
        LessOrEqual, 
        Less,
    }

    [SerializeField] private ConditionalWanted condition = ConditionalWanted.Equal;
    [SerializeField] private float baseValue;

    public override float Calculate(Ai_PlannedAction plannedAction)
    {
        float toReturn = 0;

        float abscissa = abcissaCalcul.GetAbcissaValue(plannedAction);

        switch (condition)
        {
            case ConditionalWanted.More:
                if(abscissa > baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.MoreOrEqual:
                if (abscissa >= baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.Equal:
                if (abscissa == baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.LessOrEqual:
                if (abscissa <= baseValue)
                {
                    toReturn = 1;
                }
                break;
            case ConditionalWanted.Less:
                if (abscissa < baseValue)
                {
                    toReturn = 1;
                }
                break;
        }

        return toReturn;
    }
}
