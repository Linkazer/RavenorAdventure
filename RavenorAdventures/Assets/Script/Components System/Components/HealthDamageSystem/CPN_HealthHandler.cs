using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_HealthHandler : RVN_Component<CPN_Data_HealthHandler>
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float maxArmor = 0;
    [SerializeField] private float currentHealth = 10;
    [SerializeField] private float currentArmor = 0;

    [SerializeField] private int defense;

    [SerializeField] private UnityEvent<float> SetMaxHealth;
    [SerializeField] private UnityEvent<float> SetMaxArmor;
    [SerializeField] private UnityEvent<float> OnGainHealth;
    [SerializeField] private UnityEvent<float> OnLoseHealth;
    [SerializeField] private UnityEvent<float> OnGainArmor;
    [SerializeField] private UnityEvent<float> OnLoseArmor;
    [SerializeField] private UnityEvent OnDeath;

    public float Armor => currentArmor;
    public int Defense => defense;

    public override void SetData(CPN_Data_HealthHandler toSet)
    {
        maxHealth = toSet.MaxHealth();
        maxArmor = toSet.MaxArmor();

        currentHealth = maxHealth;
        currentArmor = maxArmor;

        SetMaxHealth?.Invoke(maxHealth);
        SetMaxArmor?.Invoke(maxArmor);
    }

    public void TakeDamage(float damageAmount, float armorPierced)
    {
        RemoveArmor(armorPierced);

        if (damageAmount > 0)
        {
            if (damageAmount > currentArmor)
            {
                currentHealth -= damageAmount - currentArmor;

                if(currentHealth <= 0)
                {
                    Die();
                    return;
                }
                else
                {
                    OnLoseHealth?.Invoke(currentHealth);
                }
            }

            RemoveArmor(1);
        }
    }

    public void TakeHeal(float healAmount)
    {
        if (healAmount > 0)
        {
            currentHealth += healAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnGainHealth?.Invoke(currentHealth);
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public void AddArmor(float toAdd)
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

            OnLoseArmor?.Invoke(currentArmor);
        }
    }
}
