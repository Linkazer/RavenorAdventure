using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/AI Character")]
public class AI_CharacterScriptable : CharacterScriptable_Battle
{
    [SerializeField] private Vector2 wantedDistanceFromTarget;

    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    [SerializeField] private List<AI_Consideration> comportement;

    public Vector2 DistanceFromTarget => wantedDistanceFromTarget;

    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    public List<AI_Consideration> Comportement => comportement;
}
