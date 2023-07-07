using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_InvocationSpellBehavior : RVN_SpellBehavior<RVN_SS_InvocationSpellScriptable>
{
    [SerializeField] private int maxSpawnDistance;

    protected override bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        Node spawnNode = GetSpawnNode(targetNode);

        if (spawnNode != null)
        {
            RVN_SS_InvocationSpellScriptable spellData = GetScriptable(spellToUse);

            RVN_BattleManager.SpawnCharacter(spellData.ToSpawn, spellData.TeamIndex, spawnNode.worldPosition);

            return true;
        }

        return false;
    }

    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return GetSpawnNode(targetNode) != null;
    }

    protected Node GetSpawnNode(Node targetNode)
    {
        Node currentSpawnNode = null;

        if(!targetNode.IsWalkable)
        {
            List<Node> nodesToCheck = Pathfinding.GetAllNodeInDistance(targetNode, maxSpawnDistance, false);

            foreach(Node node in nodesToCheck)
            {
                if(node.IsWalkable)
                {
                    currentSpawnNode = node;
                    break;
                }
            }
        }
        else
        {
            currentSpawnNode = targetNode;
        }

        return currentSpawnNode;
    }
}
