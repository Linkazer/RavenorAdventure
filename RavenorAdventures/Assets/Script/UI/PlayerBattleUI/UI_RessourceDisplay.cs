using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RessourceDisplay : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] protected TextMeshProUGUI currentRessource;

    protected RVN_ComponentHandler currentCharacter;

    public Action<int> actOnUpdateRessource;

    public virtual void SetCharacter(RVN_ComponentHandler nCharacter)
    {
        if (currentCharacter != nCharacter)
        {
            UnsetCharacter();

            if (nCharacter != null)
            {
                currentCharacter = nCharacter;

                if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster spellCaster))
                {
                    spellCaster.Ressource.actOnRessourceUpdate += SetCharacterRessource;

                    SetCharacterRessource(spellCaster.Ressource.CurrentAmount);
                }
            }
        }

        gameObject.SetActive(true);
    }

    public virtual void UnsetCharacter()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster spellCaster))
            {
                spellCaster.Ressource.actOnRessourceUpdate -= SetCharacterRessource;

            }
        }

        currentCharacter = null;

        gameObject.SetActive(false);
    }

    protected virtual void SetCharacterRessource(int amount)
    {
        currentRessource.text = amount.ToString();

        actOnUpdateRessource?.Invoke(amount);
    }
}
