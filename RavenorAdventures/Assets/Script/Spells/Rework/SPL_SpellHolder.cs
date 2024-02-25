using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_SpellHolder
{
    private SPL_SpellScriptable spellData;

    //Cooldown
    protected RoundTimer cooldownTimer;

    //Usability
    private int utilisationLeft = -1;
    private bool isLocked;

    public Action<int> OnUpdateCooldown;
    public Action<int> OnUpdateUtilisationLeft;
    public Action<bool> OnLockSpell;

    public string SpellID => spellData.name;
    public int UtilisationLeft => utilisationLeft;

    public SPL_SpellScriptable SpellData => spellData;

    public int CurrentCooldown => cooldownTimer != null ? Mathf.CeilToInt(cooldownTimer.roundLeft) : 0;

    public SPL_SpellHolder()
    {

    }

    public SPL_SpellHolder(SPL_SpellScriptable spellToHold)
    {
        SetSpellData(spellToHold);
    }

    public void SetSpellData(SPL_SpellScriptable spellToHold)
    {
        spellData = spellToHold;

        cooldownTimer = null;
        utilisationLeft = spellData.MaxUtilisations;
    }

    public bool IsUsable()
    {
        return !isLocked && CurrentCooldown <= 0 && (utilisationLeft != 0);
    }

    public void UpdateCurrentCooldown()
    {
        if (cooldownTimer != null && cooldownTimer.roundLeft > 0)
        {
            if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.Round)
            {
                cooldownTimer.ProgressTimer(1);
            }

            OnUpdateCooldown?.Invoke(CurrentCooldown);
        }
    }

    public void StartCooldown()
    {
        SetCooldown(spellData.Cooldown);
    }

    public void SetCooldown(int valueToSet)
    {
        cooldownTimer = RVN_RoundManager.Instance.CreateTimer(valueToSet, UpdateCurrentCooldown, EndCooldownTimer);
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }

    private void EndCooldownTimer()
    {
        cooldownTimer = null;
        OnUpdateCooldown?.Invoke(CurrentCooldown);
    }


    public void LockSpell(bool toSet)
    {
        isLocked = toSet;
        OnLockSpell?.Invoke(isLocked);
    }

    public void UseSpell()
    {
        utilisationLeft--;
        OnUpdateUtilisationLeft?.Invoke(utilisationLeft);

        StartCooldown();
    }
}
