using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    public abstract class AppliedEffect //CODE REVIEW : Voir pour rename. Voir pour le mettre dans un autre fichier. Fusionner les effets Stat et Action ?
    {
        public EffectTrigger trigger;

        public abstract void ApplyEffect(RVN_ComponentHandler effectTarget);
    }

    public class StatEffect : AppliedEffect
    {
        public EffectStatEnum stat;
        public float valueToChange;

        public override void ApplyEffect(RVN_ComponentHandler effectTarget)
        {
            switch (stat)
            {
                case EffectStatEnum.BaseDamage:

                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster1))
                    {
                        caster1.AddPower((int)valueToChange);
                    }
                    break;
                case EffectStatEnum.Accuracy:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster2))
                    {
                        caster2.AddPower((int)valueToChange);
                    }
                    break;
                case EffectStatEnum.RerollDice:
                    if (effectTarget.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster3))
                    {
                        caster3.AddPower((int)valueToChange);
                    }
                    break;
                case EffectStatEnum.ActionByTurn:
                    break;
                case EffectStatEnum.Armor:
                    break;
                case EffectStatEnum.Defense:
                    break;
                case EffectStatEnum.Movement:
                    break;
            }
        }
    }

    public class ActionEffect : AppliedEffect
    {
        public SpellScriptable spellToUse;

        public override void ApplyEffect(RVN_ComponentHandler effectTarget)
        {
            //Lancement d'un sort sur le EffectTarget
        }
    }

    [SerializeField] private int duration;

    [SerializeField] private List<StatEffect> statEffects;
    [SerializeField] private List<ActionEffect> actionEffects;

    public int Duration => duration;
    public List<StatEffect> StatEffects => statEffects;
    public List<ActionEffect> ActionEffects => actionEffects;
}
