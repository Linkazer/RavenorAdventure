using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "Spell/Create Damage Spell")]
public class RVN_SS_DamageSpellScriptable : SpellScriptable
{
    [Header("Damage")]
    [SerializeField] private DamageType type;
    [SerializeField] private int damageDealt;
    [SerializeField] private int armorPierced;

    public DamageType Type => type;

    public int DamageDealt => damageDealt;

    public int ArmorPierced => armorPierced;
}
