using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SPL_SpellAction
{
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_SpellActionShape))] protected SPL_SpellActionShape actionShape = new SPL_Shape_TargetNode();

    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_SpellActionAnimation))] protected SPL_SpellActionAnimation[] startAnimations;

    public SPL_SpellActionShape Shape => actionShape;

    public SPL_SpellActionAnimation[] StartAnimations => startAnimations;
}
