using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health_RuntimeCharacter : UI_HealthDisplayer
{
    [Header("Armor")]
    [SerializeField] private List<UI_CharacterArmorDisplayer> armorDisplayers;

    [Header("Damage Taken")]
    [SerializeField] private UI_Character_DamageDisplayer damageDisplayerPrefab;
    [SerializeField] private Color damageTextColor;
    [SerializeField] private UI_Character_NumberDisplayer healeDisplayerPrefab;
    [SerializeField] private Color healTextColor;
    [SerializeField] private Transform damageDisplayerHandler;

    [Header("Effect Displayer")]
    [SerializeField] private UI_Character_EffectApplyDisplayer effectApplyDisplayerPrefab;

    public void TakeDamage(int damageAmount)
    {
        UI_Character_DamageDisplayer newDamageText = Instantiate(damageDisplayerPrefab, damageDisplayerHandler);

        newDamageText.Display(damageAmount, damageTextColor);
    }

    public void TakeDiceDamage(List<Dice> dices)
    {
        UI_Character_DamageDisplayer newDamageText = Instantiate(damageDisplayerPrefab, damageDisplayerHandler);

        newDamageText.DisplayDices(dices);
    }

    public void TakeHeal(int healAmount)
    {
        UI_Character_NumberDisplayer newDamageText = Instantiate(healeDisplayerPrefab, damageDisplayerHandler);

        newDamageText.Display(healAmount, healTextColor);
    }

    public void SetMaxArmor(int maxArmor)
    {
        for(int i = 0; i < armorDisplayers.Count; i++)
        {
            if (i < maxArmor)
            {
                armorDisplayers[i].SetArmor(true);
            }
            else
            {
                armorDisplayers[i].SetArmor(false);
            }
        }
    }

    public void SetArmor(int armor)
    {
        for (int i = 0; i < armorDisplayers.Count; i++)
        {
            if (i < armor)
            {
                armorDisplayers[i].GainArmor();
            }
            else
            {
                armorDisplayers[i].LoseArmor();
            }
        }
    }

    public void AddEffect(AppliedEffect effectToApply)
    {
        if (!effectToApply.GetEffect.HideOnApply)
        {
            UI_Character_EffectApplyDisplayer newEffectDisplayer = Instantiate(effectApplyDisplayerPrefab, damageDisplayerHandler);

            newEffectDisplayer.Display(effectToApply.GetEffect.Icon, true);
        }
    }

    public void RemiveEffect(AppliedEffect effectToApply)
    {
        if (!effectToApply.GetEffect.HideOnApply)
        {
            UI_Character_EffectApplyDisplayer newEffectDisplayer = Instantiate(effectApplyDisplayerPrefab, damageDisplayerHandler);

            newEffectDisplayer.Display(effectToApply.GetEffect.Icon, false);
        }
    }
}
