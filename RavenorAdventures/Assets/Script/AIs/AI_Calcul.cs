using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AI_Calcul
{
    public float calculImportance;
    public AiAbscissaType abscissaValue;

    public abstract float Calculate(float abscissa);
}
