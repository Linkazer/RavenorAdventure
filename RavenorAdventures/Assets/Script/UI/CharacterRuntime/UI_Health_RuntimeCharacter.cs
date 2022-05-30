using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health_RuntimeCharacter : UI_HealthDisplayer
{
    [SerializeField] private List<UI_CharacterArmorDisplayer> armorDisplayers;

    [SerializeField] private UI_Character_DamageDisplayer damageDisplayerPrefab;
    [SerializeField] private Transform damageDisplayerHandler;

    public void TakeDamage(int damageAmount)
    {
        UI_Character_DamageDisplayer newDamageText = Instantiate(damageDisplayerPrefab, damageDisplayerHandler);

        newDamageText.Display(damageAmount);
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
}
