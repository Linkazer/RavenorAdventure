using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_CastedSpell
{
    private SPL_SpellScriptable spellData;
    private CPN_SpellCaster caster;
    private Node targetNode;

    public SPL_SpellScriptable SpellData => spellData;
    public CPN_SpellCaster Caster => caster;
    public Node TargetNode => targetNode;

    public SPL_CastedSpell(SPL_SpellScriptable spellData, CPN_SpellCaster caster, Node targetNode)
    {
        this.spellData = spellData;
        this.caster = caster;
        this.targetNode = targetNode;
    }
}
