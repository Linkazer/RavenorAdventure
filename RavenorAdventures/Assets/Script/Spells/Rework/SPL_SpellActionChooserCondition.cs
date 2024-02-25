using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SPL_SpellActionChooserCondition
{
    public abstract bool IsConditionValid(SPL_CastedSpell castedSpellData);
}
