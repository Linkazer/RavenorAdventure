using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayer : MonoBehaviour
{
    [SerializeField] private Image healthImage;

    [SerializeField] private float maxHealth;

    public void SetMaxHealth(float nMaxHealth)
    {
        maxHealth = nMaxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}
