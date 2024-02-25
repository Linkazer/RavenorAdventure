using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPL_SpellAcionChooser
{
    [Serializable]
    private class ActionByCondition
    {
        [SerializeReference, ReferenceEditor(typeof(SPL_SpellActionChooserCondition))] public SPL_SpellActionChooserCondition condition = null;
        [SerializeReference, ReferenceEditor(typeof(SPL_SpellAction))] public SPL_SpellAction spellAction = null;
    }

    [SerializeField] private ActionByCondition[] possibleActions;

    public SPL_SpellAction GetFirstUsableAction(SPL_CastedSpell castedSpellData)
    {
        foreach(ActionByCondition actionByCondition in possibleActions)
        {
            if(actionByCondition.condition == null || actionByCondition.condition.IsConditionValid(castedSpellData))
            {
                return actionByCondition.spellAction;
            }
        }

        return null;
    }
}
