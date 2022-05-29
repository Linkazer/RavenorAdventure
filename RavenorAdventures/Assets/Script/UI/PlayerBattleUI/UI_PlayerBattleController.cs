using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerBattleController : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;
    [Header("Actions")]
    [SerializeField] private CanvasGroup spellActions;
    [SerializeField] private List<UI_PlayerSpell> spellIcons;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private CPN_Character displayedCharacter;

    public void SetCharacter(CPN_Character nCharacter)
    {
        if(nCharacter != displayedCharacter)
        {
            if(displayedCharacter != null)
            {
                //Retirer les Events
            }

            displayedCharacter = nCharacter;

            characterPortrait.sprite = displayedCharacter.Scriptable.Portrait;

            if(displayedCharacter.Handler.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                SetSpells(caster);
            }
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
