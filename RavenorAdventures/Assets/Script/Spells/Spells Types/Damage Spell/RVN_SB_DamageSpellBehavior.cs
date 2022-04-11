using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_DamageSpellBehavior : RVN_SpellBehavior<RVN_SS_DamageSpellScriptable>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        Debug.Log("End spell : " + GetScriptable(spellToEnd));
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        Debug.Log("Is spell usable : " + GetScriptable(spellToCheck) + " at position : " + targetNode.worldPosition);
        return true;
    }

    // Est appelé quand le sort est utilisé. TO DO : Prendre en compte les animations
    protected override void OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        List<CPN_Character> charactersOnNode = targetNode.GetNodeComponent<CPN_Character>();

        RVN_SS_DamageSpellScriptable usedScriptable = GetScriptable(spellToUse); // On récupère le scriptable avec le bon type.

        for (int i = 0; i < charactersOnNode.Count; i++)
        {
            Debug.Log(spellToUse.caster.gameObject.name + " deal " + usedScriptable.DamageDealt + " damages to " + charactersOnNode[i].gameObject.name + ".");
        }

        TimerManager.CreateGameTimer(0.5f, callback);
    }
}
