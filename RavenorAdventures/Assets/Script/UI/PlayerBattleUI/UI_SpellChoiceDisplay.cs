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

                caster.actOnSetActionLeft += CheckSpellUsabilities;
                caster.actOnSelectSpell += OnSelectSpell;
                caster.actOnUnselectSpell += OnUnselectSpell;

                SetSpells(caster);
                caster.actOnUpdateSpell += SetSpells;

                CheckSpellUsabilities(caster.ActionLeftThisTurn);

                if (caster.Ressource != null && caster.Ressource.RessourceType != SpellRessourceType.None)
                {
                    ressources[(int)caster.Ressource.RessourceType].actOnUpdateRessource += CheckSpellUsabilities;
                    ressources[(int)caster.Ressource.RessourceType].SetCharacter(nCharacter);
                }
            }
        }
    }


    private void SetSpells(CPN_SpellCaster caster)
    {
        List<SpellScriptable> spells = caster.Spells;

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
    }

    public void OnSelectSpell(SpellScriptable selectedSpell)
    {
        foreach (UI_PlayerSpell spl in spellIcons)
        {
            if (spl.Spell == selectedSpell)
            {
                spl.SetSelectSpell();
            }
        }
    }

    public void OnUnselectSpell(SpellScriptable unselectedSpell)
    {
        foreach(UI_PlayerSpell spl in spellIcons)
        {
            if(spl.Spell == unselectedSpell)
            {
                spl.SetUnselectSpell();
            }
        }
    }

    public void UnsetCharacter()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                caster.actOnSetActionLeft -= CheckSpellUsabilities;
                caster.actOnSelectSpell -= OnSelectSpell;
                caster.actOnUnselectSpell -= OnUnselectSpell;
                caster.actOnUpdateSpell -= SetSpells;

                if (caster.Ressource != null && caster.Ressource.RessourceType != SpellRessourceType.None)
                {
                    ressources[(int)caster.Ressource.RessourceType].UnsetCharacter();
                    ressources[(int)caster.Ressource.RessourceType].actOnUpdateRessource -= CheckSpellUsabilities;
                }
            }

            foreach (UI_PlayerSpell spl in spellIcons)
            {
                spl.SetUnselectSpell();
            }
        }
    }

    private void CheckSpellUsabilities(int actionLeft)
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
