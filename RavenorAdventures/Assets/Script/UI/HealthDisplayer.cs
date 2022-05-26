using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayer : MonoBehaviour
{
    [Serializable]
    private struct ArmorDisplay
    {
        public Image fullImage;
        public Image emptyImage;
    }
    
    [SerializeField] private Image healthImage;

    [SerializeField] private float maxHealth;

    [SerializeField] private List<ArmorDisplay> armorDisplayers;

    public void SetMaxHealth(float nMaxHealth)
    {
        maxHealth = nMaxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    public void SetMaxArmor(int maxArmor)
    {
        for(int i = 0; i < armorDisplayers.Count; i++)
        {
            if (i < maxArmor)
            {
                armorDisplayers[i].emptyImage.gameObject.SetActive(true);
            }
            else
            {
                armorDisplayers[i].emptyImage.gameObject.SetActive(false);
            }
        }
    }

    public void SetArmor(int armor)
    {
        for (int i = 0; i < armorDisplayers.Count; i++)
        {
            if (i < armor)
            {
                armorDisplayers[i].fullImage.gameObject.SetActive(true);
            }
            else
            {
                armorDisplayers[i].fullImage.gameObject.SetActive(false);
            }
        }
    }

    public void GainArmor(int armor)
    {
        
    }

    public void LoseArmor(int armor)
    {

    }
}
