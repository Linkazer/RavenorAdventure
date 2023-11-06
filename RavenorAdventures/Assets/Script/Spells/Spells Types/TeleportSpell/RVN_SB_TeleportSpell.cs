using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SB_TeleportSpell : RVN_SpellBehavior<RVN_SS_TeleportSpell>
{
    protected override void OnEndSpell(LaunchedSpellData spellToEnd)
    {
        
    }

    protected override bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        RVN_SS_TeleportSpell usedScriptable = GetScriptable(spellToCheck);

        if (!usedScriptable.DoesTargetCharacter)
        {
            return targetNode.IsWalkable;
        }
        else
        {
            List<Node> possibleDestinations = Grid.GetNeighbours(targetNode);

            for(int i = 0; i < possibleDestinations.Count; i++)
            {
                if (!possibleDestinations[i].IsWalkable || !Grid.IsNodeVisible(targetNode, possibleDestinations[i]))
                {
                    possibleDestinations.RemoveAt(i);
                    i--;
                }
            }

            return possibleDestinations.Count > 0;
        }
    }

    protected override bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode)
    {
        if (spellToUse.caster.Handler.GetComponentOfType<CPN_Movement>(out CPN_Movement casterMovement))
        {
            RVN_SS_TeleportSpell usedScriptable = GetScriptable(spellToUse);

            if (!usedScriptable.DoesTargetCharacter)
            {
                casterMovement.Teleport(targetNode);
            }
            else
            {
                List<Node> possibleDestinations = Grid.GetNeighbours(targetNode);

                for (int i = 0; i < possibleDestinations.Count; i++)
                {
                    if (!possibleDestinations[i].IsWalkable || !Grid.IsNodeVisible(targetNode, possibleDestinations[i]))
                    {
                        possibleDestinations.RemoveAt(i);
                        i--;
                    }
                }

                if(possibleDestinations.Count > 0)
                {
                    casterMovement.Teleport(possibleDestinations[UnityEngine.Random.Range(0, possibleDestinations.Count)]);
                }
            }

            //callback?.Invoke();

            return true;
        }

        return false;
    }
}
