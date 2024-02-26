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
        [SerializeReference, ReferenceEditor(typeof(SPL_SpellActionChooserCondition))] public SPL_SpellActionChooserCondition[] conditions = null;
        [SerializeReference, ReferenceEditor(typeof(SPL_SpellAction))] public SPL_SpellAction spellAction = null;

        public bool IsUsable(SPL_CastedSpell castedSpellData)
        {
            foreach(SPL_SpellActionChooserCondition condition in conditions)
            {
                if(!condition.IsConditionValid(castedSpellData))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [SerializeField] private ActionByCondition[] possibleActions;

    public SPL_SpellAction GetFirstUsableAction(SPL_CastedSpell castedSpellData)
    {
        foreach(ActionByCondition actionByCondition in possibleActions)
        {
            if(actionByCondition.IsUsable(castedSpellData))
            {
                return actionByCondition.spellAction;
            }
        }

        return null;
    }
}
