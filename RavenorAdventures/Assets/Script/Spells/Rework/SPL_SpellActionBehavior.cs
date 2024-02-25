using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SPL_SpellActionBehavior : MonoBehaviour
{
    public delegate void EndActionCallback(SPL_SpellAction actionToPlayNext);

    public abstract Type GetSpellType();

    public abstract void ResolveAction(SPL_SpellAction actionToResolve, SPL_SpellResolver spellResolver);
}

public abstract class SPL_SpellActionBehavior<T> : SPL_SpellActionBehavior where T : SPL_SpellAction
{
    public override Type GetSpellType()
    {
        return typeof(T);
    }

    public override void ResolveAction(SPL_SpellAction actionToResolve, SPL_SpellResolver spellResolver)
    {
        ResolveAction(actionToResolve as T, spellResolver);
    }

    protected abstract void ResolveAction(T actionToResolve, SPL_SpellResolver spellResolver);

    protected void EndResolve(SPL_SpellAction nextAction, SPL_SpellResolver spellResolver)
    {
        spellResolver.EndSpellAction(nextAction);
    }
}