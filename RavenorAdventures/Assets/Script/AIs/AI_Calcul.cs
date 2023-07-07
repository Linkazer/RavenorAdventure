using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RVN_AiBattleManager;

[Serializable]
public abstract class AI_Calcul
{
    public float calculImportance;
    [SerializeReference, ReferenceEditor(typeof(AI_Calcul_AbscissaType))] protected AI_Calcul_AbscissaType abcissaCalcul;

    public abstract float Calculate(Ai_PlannedAction plannedAction);
}
