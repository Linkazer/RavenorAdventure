using ravenor.referencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/AI Character")]
public class AI_CharacterScriptable : CharacterScriptable_Battle
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(AI_MovementBehavior))] private AI_MovementBehavior movementBehaviorUsed;

    [SerializeField, Tooltip("The health added to the current health when calculate the opportunity attack score of a movement.")] private int oppportunityHealthBonus = 0;

    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    [SerializeField] private List<AI_Consideration> comportement;

    public AI_MovementBehavior MovementBehaviorUsed => movementBehaviorUsed;

    public int OppportunityHealthBonus => oppportunityHealthBonus;

    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    public List<AI_Consideration> Comportement => comportement;
}
