using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Est utilisé pour contenir les données d'un sort lancé.
/// Permet de sauvegarder :
/// - Le sort lancé (Scriptable)
/// - Le lanceur du sort (Caster)
/// </summary>
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
