using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_HealthHandler : RVN_Component<CPN_Data_HealthHandler>
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int maxArmor = 0;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private int currentArmor = 0;

    [SerializeField] private int defense;
    [SerializeField] private int defensiveRerolls;

    [SerializeField] private UnityEvent<float> OnSetMaxHealth;
    [SerializeField] private UnityEvent<int> OnSetMaxArmor;
    [SerializeField] private UnityEvent<int> OnGainHealth;
    [SerializeField] private UnityEvent<int> OnLoseHealth;
    [SerializeField] private UnityEvent<List<Dice>> OnLoseHealthDices;
    [SerializeField] private UnityEvent<float> OnChangeHealth;
    [SerializeField] private UnityEvent<int> OnGainArmor;
    [SerializeField] private UnityEvent<int> OnLoseArmor;
    [SerializeField] private UnityEvent OnDeath;

    public Action<RVN_ComponentHandler> actOnDeath;
    public Action<float> actOnChangeHealth;
    public Action<RVN_ComponentHandler> actOnTakeDamageSelf;
    public Action<RVN_ComponentHandler> actOnTakeDamageTarget;
    public Action<RVN_ComponentHandler> actOnAttackReceivedTowardSelf;
    public Action<RVN_ComponentHandler> actOnAttackReceivedTowardTarget;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxArmor => maxArmor;
    public int CurrentArmor => currentArmor;
    public int Defense => defense;
    public int DefensiveRerolls => defensiveRerolls;

    public bool IsAlive => currentHealth > 0;

    public override void SetData(CPN_Data_HealthHandler toSet)
    {
        maxHealth = toSet.MaxHealth();
        maxArmor = toSet.MaxArmor();

        currentHealth = maxHealth;
        currentArmor = maxArmor;

        defense = toSet.Defense();
        defensiveRerolls = toSet.DefensiveRerolls();

        OnSetMaxHealth?.Invoke(maxHealth);
        OnSetMaxArmor?.Invoke(maxArmor);

        OnGainArmor?.Invoke(currentArmor);
    }

    /// <summary>
    /// Take Damage from a dice list.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="dices"></param>
    /// <param name="damage"></param>
    public void TakeDamage(CPN_SpellCaster caster, List<Dice> dices, float damage)
    {
        OnLoseHealthDices?.Invoke(dices);

        TakeDamage(caster, damage);
    }

    /// <summary>
    /// Take damage from a SpellCaster
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="damage"></param>
    public void TakeDamage(CPN_SpellCaster caster, float damage)
    {
        TakeDamage(damage);

        actOnTakeDamageSelf?.Invoke(Handler);
        if (caster != null)
        {
            actOnTakeDamageTarget?.Invoke(caster.Handler);
        }
    }

    /// <summary>
    /// Apply the damage taken.
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= Mathf.RoundToInt(damageAmount);

        if (damageAmount > 0)
        {
            OnLoseHealth?.Invoke((int)damageAmount);
        }
        OnChangeHealth?.Invoke(currentHealth);
        actOnChangeHealth?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

    }

    /// <summary>
    /// Apply a heal.
    /// </summary>
    /// <param name="healAmount"></param>
    public void TakeHeal(float healAmount)
    {
        if (healAmount > 0)
        {
            currentHealth += Mathf.RoundToInt(healAmount);

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnGainHealth?.Invoke((int)healAmount);
            OnChangeHealth?.Invoke(currentHealth);
            actOnChangeHealth?.Invoke(currentHealth);
        }
    }

    public void AddMaxHealth(int healthToAdd)
    {
        if(healthToAdd > 0)
        {
            maxHealth += healthToAdd;

            currentHealth += healthToAdd;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnChangeHealth?.Invoke(currentHealth);
            actOnChangeHealth(currentHealth);
        }
    }

    public void RemoveMaxHealth(int healthToRemove)
    {
        if (healthToRemove > 0)
        {
            maxHealth -= healthToRemove;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnChangeHealth?.Invoke(currentHealth);
            actOnChangeHealth(currentHealth);
        }
    }

    private void Die()
    {
        actOnDeath?.Invoke(Handler);

        handler.animationController?.PlayAnimation("Character_Death");
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
            if (maxArmor > 0)
            {
                currentArmor = maxArmor;
            }
            else
            {
                currentArmor = 0;
            }
        }

        OnSetMaxArmor?.Invoke(maxArmor);
    }

    public void AddDefense(int toAdd)
    {
        defense += toAdd;
    }

    public void AddDefensiveRerolls(int toAdd)
    {
        defensiveRerolls += toAdd;
    }
}
