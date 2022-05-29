using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_HealthHandler : RVN_Component<CPN_Data_HealthHandler>
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private int maxArmor = 0;
    [SerializeField] private float currentHealth = 10;
    [SerializeField] private int currentArmor = 0;

    [SerializeField] private int defense;

    [SerializeField] private UnityEvent<float> OnSetMaxHealth;
    [SerializeField] private UnityEvent<int> OnSetMaxArmor;
    [SerializeField] private UnityEvent<int> OnGainHealth;
    [SerializeField] private UnityEvent<int> OnLoseHealth;
    [SerializeField] private UnityEvent<float> OnChangeHealth;
    [SerializeField] private UnityEvent<int> OnGainArmor;
    [SerializeField] private UnityEvent<int> OnLoseArmor;
    [SerializeField] private UnityEvent OnDeath;

    public Action<RVN_ComponentHandler> actOnDeath;
    public Action<RVN_ComponentHandler> actOnTakeDamageSelf;
    public Action<RVN_ComponentHandler> actOnTakeDamageTarget;

    public int Armor => currentArmor;
    public int Defense => defense;

    public override void SetData(CPN_Data_HealthHandler toSet)
    {
        maxHealth = toSet.MaxHealth();
        maxArmor = toSet.MaxArmor();

        currentHealth = maxHealth;
        currentArmor = maxArmor;

        defense = toSet.Defense();

        OnSetMaxHealth?.Invoke(maxHealth);
        OnSetMaxArmor?.Invoke(maxArmor);

        OnGainArmor?.Invoke(currentArmor);
    }

    public void TakeDamage(CPN_SpellCaster caster, float damageAmount, int armorPierced)
    {
        actOnTakeDamageSelf?.Invoke(handler);
        if (caster != null)
        {
            actOnTakeDamageTarget?.Invoke(caster.Handler);
        }

        TakeDamage(damageAmount, armorPierced);
    }

    public void TakeDamage(float damageAmount, int armorPierced)
    {
        RemoveArmor(armorPierced);

        if (damageAmount > currentArmor)
        {
            currentHealth -= damageAmount - currentArmor;

            OnLoseHealth?.Invoke((int)damageAmount);
            OnChangeHealth?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
                return;
            }
        }

        RemoveArmor(1);
    }

    public void TakeHeal(float healAmount)
    {
        if (healAmount > 0)
        {
            currentHealth += healAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnGainHealth?.Invoke((int)healAmount);
            OnChangeHealth?.Invoke(currentHealth);
        }
    }

    private void Die()
    {
        actOnDeath?.Invoke(Handler);
        OnDeath?.Invoke();
    }

    public void AddArmor(int toAdd)
    {
        if (toAdd > 0 && currentArmor < maxArmor)
        {
            currentArmor += toAdd;

            currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);

            OnGainArmor?.Invoke(currentArmor);
        }
    }

    public void RemoveArmor(int toRemove)
    {
        if (maxArmor > 0 && toRemove > 0 && currentArmor > 0)
        {
            currentArmor -= toRemove;

            currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);

            OnLoseArmor?.Invoke(currentArmor);
        }
    }

    public void AddMaxArmor(int toAdd)
    {
        maxArmor += toAdd;
        AddArmor(toAdd);

        OnSetMaxArmor?.Invoke(maxArmor);
    }

    public void RemoveMaxArmor(int toRemove)
    {
        maxArmor -= toRemove;
        if(currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
        }

        OnSetMaxArmor?.Invoke(maxArmor);
    }

    public void AddDefense(int toAdd)
    {
        defense += toAdd;
    }
}
