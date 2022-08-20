using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/AI Character")]
public class AI_CharacterScriptable : CharacterScriptable_Battle
{
    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    [SerializeField] private List<AI_Consideration> comportement;

    /// <summary>
    /// Comportements possibles du personnage.
    /// </summary>
    public List<AI_Consideration> Comportement => comportement;
}
