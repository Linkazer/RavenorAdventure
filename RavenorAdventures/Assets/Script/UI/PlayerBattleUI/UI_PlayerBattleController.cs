using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerBattleController : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;
    [Header("Actions")]
    [SerializeField] private CanvasGroup spellActions;
    [SerializeField] private List<UI_PlayerSpell> spellIcons;

    [SerializeField] private UnityEvent<CPN_Character> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    private CPN_Character displayedCharacter;

    public void SetCharacter(CPN_Character nCharacter)
    {
        if (nCharacter != displayedCharacter)
        {
            UnsetCharacter();

            if (nCharacter != null)
            {
                displayedCharacter = nCharacter;

                OnSetCharacter?.Invoke(displayedCharacter);

                characterPortrait.sprite = displayedCharacter.Scriptable.Portrait;

                if (displayedCharacter.Handler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
                {
                    SetSpells(caster);
                }
            }
        }
    }

    public void UnsetCharacter()
    {
        if (displayedCharacter != null)
        {
            OnUnsetCharacter?.Invoke();

            displayedCharacter = null;
        }
    }

    private void SetSpells(CPN_SpellCaster caster)
    {
        List<SpellScriptable> spells = caster.Spells;

        for(int i = 0; i < spellIcons.Count; i++)
        {
            if(i < spells.Count)
            {
                spellIcons[i].SetSpell(spells[i]);
            }
            else
            {
                spellIcons[i].UnsetSpell();
            }
        }
    }

    private void SetDisplayCharacterEvent(CPN_Character toSet)
    {
        if (toSet.Handler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.actOnSetActionLeft += SetUsableActionSpell;
        }
    }

    private void UnsetDisplayCharacterEvent(CPN_Character toUnset)
    {
        if(toUnset.Handler.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler health))
        {
           //Voir si on fait ça ici ou dans un autre script qui gère uniquement la vie du personnage
        }

        if (toUnset.Handler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.actOnSetActionLeft -= SetUsableActionSpell;
        }
    }

    private void SetUsableActionSpell(int actionLeft)
    {
        if(actionLeft > 0)
        {
            spellActions.interactable = true;
        }
        else
        {
            spellActions.interactable = false;
        }
    }
}
