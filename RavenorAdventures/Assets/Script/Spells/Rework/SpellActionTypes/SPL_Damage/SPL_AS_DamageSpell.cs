using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_AS_DamageSpell : SPL_SpellAction
{
    [Header("Damage Datas")]
    [SerializeField] private int dicesUsed;
    [SerializeField] private SPL_DamageData[] damageData;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_SpellActionAnimation))] private SPL_SpellActionAnimation[] damageAnimations;

    [Header("Actions")]
    [SerializeField] private SPL_SpellAcionChooser touchAction;
    [SerializeField] private SPL_SpellAcionChooser noTouchAction;
    [SerializeField] private SPL_SpellAcionChooser nextAction;

    public int DiceUsed => dicesUsed;
    public SPL_DamageData[] DamageData => damageData;

    public SPL_SpellActionAnimation[] DamageAnimations => damageAnimations;

    public SPL_SpellAction GetTouchAction(SPL_CastedSpell castedSpellData)
    {
        return touchAction.GetFirstUsableAction(castedSpellData);
    }

    public SPL_SpellAction GetNoTouchAction(SPL_CastedSpell castedSpellData)
    {
        return noTouchAction.GetFirstUsableAction(castedSpellData);
    }

    public SPL_SpellAction GetNextAction(SPL_CastedSpell castedSpellData)
    {
        return nextAction.GetFirstUsableAction(castedSpellData);
    }
}
