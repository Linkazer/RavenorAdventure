using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "Spell/Create Damage Spell")]
public class RVN_SS_DamageSpellScriptable : SpellScriptable
{
    [Header("Damage")]
    [SerializeField] private SPL_DamageType type;
    [SerializeField] private int diceUsed;
    [SerializeField] private int baseDamage;
    [SerializeField] private int armorPierced;
    [SerializeField, Range(0f, 1f)] private float lifestealPercent;

    [Header("Hit effects")]
    [SerializeField] private bool areEffectsRandom;
    [SerializeField] private List<EffectScriptable> hitEffectsOnTarget;
    [SerializeField] private List<EffectScriptable> hitEffectsOnCaster;

    private int bonusAccuracy;
    private int bonusBaseDamage;
    private int offensiveRerolls;
    private int offensiveRerollsMalus;


    public SPL_DamageType Type => type;

    public int DiceUsed => diceUsed;

    public int ArmorPierced => armorPierced;

    public int BaseDamage => baseDamage + bonusBaseDamage;

    public float Lifesteal => lifestealPercent;

    public int Accuracy => bonusAccuracy;

    public int OffensiveRerolls => offensiveRerolls;
    public int OffensiveRerollsMalus => offensiveRerollsMalus;

    public bool AreEffectRandom => areEffectsRandom;
    public List<EffectScriptable> HitEffectsOnTarget => hitEffectsOnTarget;
    public List<EffectScriptable> HitEffectsOnCaster => hitEffectsOnCaster;

    public override void SetCaster(CPN_SpellCaster caster)
    {
        bonusAccuracy = caster.Accuracy;
        bonusBaseDamage = caster.Power;
        offensiveRerolls = caster.OffensiveRerolls;
        offensiveRerollsMalus = caster.OffensiveRerollsMalus;
    }

    public override string GetDescription()
    {
        string toReturn = base.GetDescription();

        if (hitEffectsOnTarget.Count > 0)
        {
            foreach (EffectScriptable eff in hitEffectsOnTarget)
            {
                if (!eff.HideOnApply)
                {
                    toReturn += $"\n <b> {eff.Name} </b>  : {eff.Description}";
                }
            }
        }

        if (hitEffectsOnCaster.Count > 0)
        {
            foreach (EffectScriptable eff in hitEffectsOnCaster)
            {
                if (!eff.HideOnApply)
                {
                    toReturn += $"\n <b> {eff.Name} </b>  : {eff.Description}";
                }
            }
        }

        return toReturn;
    }
}
