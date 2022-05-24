using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "Spell/Create Damage Spell")]
public class RVN_SS_DamageSpellScriptable : SpellScriptable
{
    [Header("Damage")]
    [SerializeField] private DamageType type;
    [SerializeField] private int diceUsed;
    [SerializeField] private int baseDamage;
    [SerializeField] private int armorPierced;

    private int bonusAccuracy;
    private int bonusBaseDamage;
    private int possibleReroll;


    public DamageType Type => type;

    public int DiceUsed => diceUsed;

    public int ArmorPierced => armorPierced;

    public int BaseDamage => baseDamage + bonusBaseDamage;

    public int Accuracy => bonusAccuracy;

    public int PossibleReroll => possibleReroll;

    public override void SetCaster(CPN_SpellCaster caster)
    {
        bonusAccuracy = caster.Accuracy;
        bonusBaseDamage = caster.Power;
        possibleReroll = caster.PossibleReroll;
    }
}
