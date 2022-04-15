using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "Spell/Create Damage Spell")]
public class RVN_SS_DamageSpellScriptable : SpellScriptable
{
    [Header("Damage")]
    [SerializeField] private int damageDealt;

    public int DamageDealt => damageDealt;

}
