using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell/Create Spell")]
public abstract class SpellScriptable : ScriptableObject
{
    public abstract SpellData GetSpellData();
}
