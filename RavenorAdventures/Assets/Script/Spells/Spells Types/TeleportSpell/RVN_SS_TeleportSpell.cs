using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport Spell", menuName = "Spell/Create Teleport Spell")]
public class RVN_SS_TeleportSpell : SpellScriptable
{
    [SerializeField] private bool targetCharacter = false;
    [SerializeField] private int maxDistanceFromTarget = -1;

    public bool DoesTargetCharacter => targetCharacter;

    public int DistanceMacFromTarget => maxDistanceFromTarget;

    public override void SetCaster(CPN_SpellCaster caster)
    {
        
    }
}
