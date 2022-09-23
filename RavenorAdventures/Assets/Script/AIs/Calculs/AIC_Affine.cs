using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIC_Affine : AI_Calcul
{
    [SerializeField] private float abscissaCoeficient;
    [SerializeField] private float constantToAdd;
    [SerializeField, Tooltip("Valeur attendue pour que le score face 1.")] private float maxValue;

    public override float Calculate(float abscissa)
    {
        return (constantToAdd + abscissa * abscissaCoeficient) / maxValue;
    }
}
