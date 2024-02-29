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
    private int utilisationLeftForTurn = 1;
    private int utilisationLeftForLevel = -1;
    private bool isLocked;

    public Action<int> OnUpdateCooldown;
    public Action<int> OnUpdateUtilisationLeft;
    public Action<bool> OnLockSpell;

    public string SpellID => spellData.name;
    public int UtilisationLeftForTurn => utilisationLeftForTurn;
    public int UtilisationLeftForLevel => utilisationLeftForLevel;

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
        utilisationLeftForLevel = spellData.MaxUtilisationsByLevel;

        if (spellData.MaxUtilisationsByTurn > 1)
        {
            utilisationLeftForTurn = spellData.MaxUtilisationsByTurn;
        }
        else
        {
            utilisationLeftForTurn = 1;
        }
    }

    public bool IsUsable()
    {
        return !isLocked && CurrentCooldown <= 0 && (utilisationLeftForLevel != 0);
    }

    public bool CanBeCasted(int actionLeft)
    {
        return actionLeft > 0 || spellData.CastType == SpellCastType.Fast || CanBeReused();
    }

    public bool DoesCostAction()
    {
        if(spellData.CastType == SpellCastType.Fast)
        {
            return false;
        }

        if(spellData.MaxUtilisationsByTurn > 0)
        {
            if(spellData.MaxUtilisationsByTurn == utilisationLeftForTurn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;

    }

    public bool CanBeReused()
    {
        return spellData.MaxUtilisationsByTurn > 0 && utilisationLeftForTurn > 0 && utilisationLeftForTurn != spellData.MaxUtilisationsByTurn;
    }

    public void OnCaterBeginTurn()
    {
        if (spellData.MaxUtilisationsByTurn > 1)
        {
            utilisationLeftForTurn = spellData.MaxUtilisationsByTurn;
            Debug.Log(utilisationLeftForTurn);
        }
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
        utilisationLeftForLevel--;
        utilisationLeftForTurn--;

        OnUpdateUtilisationLeft?.Invoke(utilisationLeftForLevel);

        StartCooldown();
    }
}
