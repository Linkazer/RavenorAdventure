using ravenor.referencePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_AS_EffectAction : SPL_SpellAction
{
    [Header("Effects")]
    [SerializeField] private List<EffectScriptable> effectsOnTarget;

    [Header("Animation")]
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_SpellActionAnimation))] private SPL_SpellActionAnimation[] effectAnimation;

    [Header("Actions")]
    [SerializeField] private SPL_SpellAcionChooser nextAction;

    public List<EffectScriptable> EffectsOnTarget => effectsOnTarget;

    public SPL_SpellActionAnimation[] DamageAnimations => effectAnimation;

    public SPL_SpellAction GetNextAction(SPL_CastedSpell castedSpellData)
    {
        return nextAction.GetFirstUsableAction(castedSpellData);
    }
}
