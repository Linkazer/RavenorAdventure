using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedSpellData
{
    public SpellScriptable scriptable;

    public CPN_SpellCaster caster;

    public T GetScriptableAs<T>() where T : SpellScriptable
    {
        if(scriptable is T)
        {
            return scriptable as T;
        }
        return null;
    }
}
