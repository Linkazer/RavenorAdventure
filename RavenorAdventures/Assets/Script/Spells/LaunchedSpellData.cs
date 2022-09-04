using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Est utilis� pour contenir les donn�es d'un sort lanc�.
/// Permet de sauvegarder :
/// - Le sort lanc� (Scriptable)
/// - Le lanceur du sort (Caster)
/// </summary>
public class LaunchedSpellData
{
    public SpellScriptable scriptable;

    public CPN_SpellCaster caster;

    public Node targetNode;

    public T GetScriptableAs<T>() where T : SpellScriptable
    {
        if(scriptable is T)
        {
            return scriptable as T;
        }
        return null;
    }

    public LaunchedSpellData()
    {

    }

    public LaunchedSpellData(SpellScriptable spell, CPN_SpellCaster nCaster, Node nTargetNode)
    {
        scriptable = spell;

        caster = nCaster;
        if(caster != null)
        {
            scriptable.SetCaster(caster);
        }

        targetNode = nTargetNode;
    }
}
