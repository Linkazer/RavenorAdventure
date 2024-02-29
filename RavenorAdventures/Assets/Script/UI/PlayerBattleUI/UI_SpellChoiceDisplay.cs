using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpellChoiceDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup spellActions;
    [SerializeField] private List<UI_PlayerSpell> spellIcons;

    [SerializeField] private List<UI_RessourceDisplay> ressources;

    private RVN_ComponentHandler currentCharacter;

    public RVN_ComponentHandler CurrentCharacter => currentCharacter;

    public void SetCharacter(RVN_ComponentHandler nCharacter)
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            if (nCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                currentCharacter = nCharacter;

                caster.actOnSetActionLeft += OnCasterUpdateActionLeft;
                caster.actOnSelectSpell += OnSelectSpell;
                caster.actOnUnselectSpell += OnUnselectSpell;

                SetSpells(caster);
                caster.actOnUpdateSpell += SetSpells;

                if (caster.Ressource != null && caster.Ressource.RessourceType != SpellRessourceType.None)
                {
                    ressources[(int)caster.Ressource.RessourceType].actOnUpdateRessource += OnCasterUseRessource;
                    ressources[(int)caster.Ressource.RessourceType].SetCharacter(nCharacter);
                }
            }
        }
    }

    private void SetSpells(CPN_SpellCaster caster)
    {
        List<SPL_SpellHolder> spells = caster.Spells;

        for (int i = 0; i < spellIcons.Count; i++)
        {
            if (i < spells.Count)
            {
                spellIcons[i].SetSpell(spells[i]);
            }
            else
            {
                spellIcons[i].UnsetSpell();
            }
        }

        CheckSpellUsabilities();
    }

    public void OnSelectSpell(SPL_SpellHolder selectedSpell)
    {
        foreach (UI_PlayerSpell spl in spellIcons)
        {
            if (spl.Spell == selectedSpell)
            {
                spl.SetSelectSpell();
            }
        }
    }

    public void OnUnselectSpell(SPL_SpellHolder unselectedSpell)
    {
        foreach(UI_PlayerSpell spl in spellIcons)
        {
            spl.SetUnselectSpell();
        }
    }

    public void UnsetCharacter()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                caster.actOnSetActionLeft -= OnCasterUpdateActionLeft;
                caster.actOnSelectSpell -= OnSelectSpell;
                caster.actOnUnselectSpell -= OnUnselectSpell;
                caster.actOnUpdateSpell -= SetSpells;

                if (caster.Ressource != null && caster.Ressource.RessourceType != SpellRessourceType.None)
                {
                    ressources[(int)caster.Ressource.RessourceType].UnsetCharacter();
                    ressources[(int)caster.Ressource.RessourceType].actOnUpdateRessource -= OnCasterUseRessource;
                }
            }

            foreach (UI_PlayerSpell spl in spellIcons)
            {
                spl.SetUnselectSpell();
            }
        }
    }

    private void OnCasterUpdateActionLeft(int maxAction, int actionLeft)
    {
        CheckSpellUsabilities();
    }

    private void OnCasterUseRessource(int ressourceLeft)
    {
        CheckSpellUsabilities();
    }

    private void CheckSpellUsabilities()
    {
        foreach (UI_PlayerSpell spl in spellIcons)
        {
            if (spl.Spell != null)
            {
                CheckSpellUsability(spl);
            }
        }
    }


    public void CheckSpellUsability(UI_PlayerSpell uI_PlayerSpell)
    {
        uI_PlayerSpell.CheckUsable(currentCharacter);
    }
}
