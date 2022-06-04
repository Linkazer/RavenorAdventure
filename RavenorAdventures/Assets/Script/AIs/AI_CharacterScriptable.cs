using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character/AI Character")]
public class AI_CharacterScriptable : CharacterScriptable_Battle
{
    [SerializeField] private List<AI_Consideration> comportement;

    public List<AI_Consideration> Comportement => comportement;
}
