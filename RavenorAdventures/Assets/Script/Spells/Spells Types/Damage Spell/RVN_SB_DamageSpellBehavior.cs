using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_DamageSpellBehavior : RVN_SpellBehavior<RVN_SD_DamageSpellData>
{
    protected override void OnEndSpell(RVN_SD_DamageSpellData spellToEnd)
    {
        Debug.Log("End spell : " + spellToEnd);
    }

    protected override bool OnIsSpellUsable(RVN_SD_DamageSpellData spellToCheck, Node targetNode)
    {
        Debug.Log("Is spell usable : " + spellToCheck + " at position : " + targetNode.worldPosition);
        return true;
    }

    protected override void OnUseSpell(RVN_SD_DamageSpellData spellToUse, Node targetNode, Action callback)
    {
        List<CPN_Character> charactersOnNode = targetNode.GetNodeComponent<CPN_Character>();

        for (int i = 0; i < charactersOnNode.Count; i++)
        {
            Debug.Log("Use Spell : " + spellToUse.Name + " at position : " + charactersOnNode[i].gameObject.name);
        }

        TimerManager.CreateGameTimer(0.5f, callback);
    }
}
