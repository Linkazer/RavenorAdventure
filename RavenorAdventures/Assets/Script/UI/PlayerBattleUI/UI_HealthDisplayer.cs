using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthDisplayer : MonoBehaviour
{
    [SerializeField] protected Image healthImage;
    [SerializeField] protected bool isReversed;

    protected float maxHealth;

    protected CPN_Character currentCharacter;

    public virtual void SetCharacter(CPN_Character nCharacter)
    {
        if(currentCharacter != nCharacter)
        {
            UnsetCharacter();

            if (nCharacter != null)
            {
                currentCharacter = nCharacter;

                if (currentCharacter.Handler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler health))
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
            if(currentCharacter.Handler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler health))
            {
                health.actOnChangeHealth -= SetCurrentHealth;
            }
        }

        currentCharacter = null;
    }

    public void SetMaxHealth(float nMaxHealth)
    {
        maxHealth = nMaxHealth;
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
    }
}
