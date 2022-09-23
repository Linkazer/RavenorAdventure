using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La valeur qui sera récupérer pour le calcul.
/// </summary>
public enum AiAbscissaType
{
    DistanceFromTarget_CalculatedPosition,
    TargetMaxHp, 
    TargetCurrentHp, 
    TargetPercentHp,
    CasterMaxHp, 
    CasterCurrentHp, 
    CasterPercentHp,
    TargetDangerosity,
    TargetVulnerability,
    TargetMaxArmor,
    TargetCurrentArmor,
    NumberEnnemyArea,
    NumberAllyArea, 
    DistranceFromTarget_BasePosition
}

/// <summary>
/// Type de calcul d'une IA.
/// </summary>
public enum AiCalculType
{
    Conditionnal, Affine, Logarythm,
    Exponential, ReverseExponential,
    Logistical
}

[Serializable]
public class ValueForCalcul
{
    public AiCalculType calculType;
    public AiAbscissaType abscissaValue;
    public bool checkAroundMax;
    public float maxValue;
    public float constant;
    public float coeficient;
    public float calculImportance = 1;

    public ValueForCalcul()
    {
        calculImportance = 1;
    }
}

[Serializable]
public class AI_Consideration
{
    [Header("Actions")]
    public SpellScriptable wantedAction;
    public int wantedActionIndex;
    //public OptimizePositionOption optimizePosition;
    [Header("Condition")]
    public List<AIC_Conditional> conditions;
    [Header("Calculs")]
    //[Tooltip("Minimum -1 si on veut que la Considération ne soit pas prise en compte.")] public float considerationImportance = 0;
    //[Tooltip("Met une limite au score maximum des calculs.")] public float maximumValueModifier;
    public float bonusScore;
    [SerializeReference, ReferenceEditor(typeof(AI_Calcul))] public List<AI_Calcul> calculs;
    //public int maxCooldown, cooldown;
}
