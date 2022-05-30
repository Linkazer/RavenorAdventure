using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpellChoiceDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup spellActions;
    [SerializeField] private List<UI_PlayerSpell> spellIcons;

    private RVN_ComponentHandler currentCharacter;

    public void SetCharacter(RVN_ComponentHandler nCharacter)
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            if (nCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                currentCharacter = nCharacter;

                caster.actOnSetActionLeft += SetUsableActionSpell;

                SetSpells(caster);

                SetUsableActionSpell(caster.ActionLeftThisTurn);
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

    public void UnsetCharacter()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
            {
                caster.actOnSetActionLeft -= SetUsableActionSpell;
            }
        }
    }

    private void SetUsableActionSpell(int actionLeft)
    {
        if (actionLeft > 0)
        {
            spellActions.interactable = true;
        }
        else
        {
            spellActions.interactable = false;
        }
    }
}
