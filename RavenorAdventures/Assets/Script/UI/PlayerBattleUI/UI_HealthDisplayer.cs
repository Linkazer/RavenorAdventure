using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealthDisplayer : MonoBehaviour
{
    [Header("Fill Image")]
    [SerializeField] protected Image healthImage;
    [SerializeField] protected bool isReversed;

    [Header("Text")]
    [SerializeField] protected TextMeshProUGUI currentHealthText;
    [SerializeField] protected TextMeshProUGUI maxHealthText;

    protected float maxHealth;

    protected RVN_ComponentHandler currentCharacter;

    public virtual void SetCharacter(RVN_ComponentHandler nCharacter)
    {
        if(currentCharacter != nCharacter)
        {
            UnsetCharacter();

            if (nCharacter != null)
            {
                currentCharacter = nCharacter;

                if (currentCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler health))
                {
                    health.actOnChangeHealth += SetCurrentHealth;


                    SetMaxHealth(health.MaxHealth);
                    SetCurrentHealth(health.CurrentHealth);
                }
            }
        }
    }

    public virtual void UnsetCharacter()
    {
        if(currentCharacter != null)
        {
            if(currentCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler health))
            {
                health.actOnChangeHealth -= SetCurrentHealth;
            }
        }

        currentCharacter = null;
    }

    public void SetMaxHealth(float nMaxHealth)
    {
        maxHealth = nMaxHealth;

        if(maxHealthText != null)
        {
            maxHealthText.text = maxHealth.ToString();
        }
    }

    public void SetCurrentHealth(float currentHealth)
    {
        if (isReversed)
        {
            healthImage.fillAmount = 1 - (currentHealth / maxHealth);
        }
        else
        {
            healthImage.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealthText != null)
        {
            currentHealthText.text = currentHealth.ToString();
        }
    }
}
