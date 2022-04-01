using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "Spell/Create Damage Spell")]
public class RVN_SS_DamageSpellScriptable : SpellScriptable
{
    [SerializeField] private RVN_SD_DamageSpellData spellData;

    public override SpellData GetSpellData()
    {
        return spellData;
    }
}
