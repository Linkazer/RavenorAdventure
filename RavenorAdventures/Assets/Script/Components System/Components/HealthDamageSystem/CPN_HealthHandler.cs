using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_HealthHandler : RVN_Component<CPN_Data_HealthHandler>
{
    private float maxHealth;
    private float maxArmor;
    private float currentHealth;
    private float currentArmor;

    [SerializeField] private UnityEvent<float> OnGainHealth;
    [SerializeField] private UnityEvent<float> OnLoseHealth;
    [SerializeField] private UnityEvent<float> OnGainArmor;
    [SerializeField] private UnityEvent<float> OnLoseArmor;

    public override void SetData(CPN_Data_HealthHandler toSet)
    {
        maxHealth = toSet.MaxHealth();
        maxArmor = toSet.MaxArmor();

        currentHealth = maxHealth;
        currentArmor = maxArmor;
    }

    public void TakeDamage(float damageAmount, float armorPierced)
    {
        currentArmor -= armorPierced;
        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);

        if (damageAmount > 0)
        {
            if (damageAmount > currentArmor)
            {
                currentHealth -= damageAmount - currentArmor;
            }

            currentArmor--;
            
        }
    }

    private void AddArmor(float toAdd)
    {
        if (toAdd > 0 && currentArmor < maxArmor)
        {
            currentArmor += toAdd;

            currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);

            OnGainArmor?.Invoke(currentArmor);
        }
    }

    private void RemoveArmor(float toRemove)
    {
        if (toRemove > 0 && currentArmor > 0)
        {
            currentArmor -= toRemove;

            currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);

            OnGainArmor?.Invoke(currentArmor);
        }
    }
}
