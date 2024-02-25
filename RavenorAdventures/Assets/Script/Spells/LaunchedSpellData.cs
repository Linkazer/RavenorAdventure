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
[Obsolete("Use CastData")]
public class LaunchedSpellData : ISoundHolder //TODO Rework Spell : Supprimer toutes les ref à ça (C'est principalement dans les behavior donc ça va)
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

    public AudioData GetAudioData()
    {
        return scriptable.AnimationAudioData;
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
