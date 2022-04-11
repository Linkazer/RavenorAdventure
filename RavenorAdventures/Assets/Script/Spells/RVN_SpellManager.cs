using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_SpellManager : RVN_Singleton<RVN_SpellManager>
{
    [SerializeField] private List<RVN_SpellBehavior> spellsBehaviors;

    /// <summary>
    /// Demande � v�rifier si le sort est utilisable sur la case vis�e.
    /// </summary>
    /// <param name="spellToCheck">Sort � tester.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <returns>TRUE si le sort peut �tre utilis�.</returns>
    public static bool CanUseSpell(LaunchedSpellData spellToCheck, Node targetNode)
    {
        if (spellToCheck != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToCheck.scriptable);

            if (behaviorUsed != null)
            {
                return behaviorUsed.IsSpellUsable(spellToCheck, targetNode);
            }
        }

        return false;
    }

    /// <summary>
    /// Lance le sort voulut sur la case cibl�.
    /// </summary>
    /// <param name="spellToUse">Sort � lancer.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <param name="callbackEndSpell">Action � lanc� une fois le sort lanc�.</param>
    public static void UseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callbackEndSpell)
    {
        if (spellToUse != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToUse.scriptable);

            if (behaviorUsed != null)
            {
                List<Node> targetsNodes = Pathfinding.GetAllNodeInDistance(targetNode, spellToUse.scriptable.ZoneRange, false);
                for (int i = 0; i < targetsNodes.Count; i++)
                {
                    if (targetsNodes[i] != targetNode)
                    {
                        behaviorUsed.UseSpell(spellToUse, targetsNodes[i]);
                    }
                    else
                    {
                        behaviorUsed.UseSpell(spellToUse, targetsNodes[i], () => EndSpell(spellToUse, callbackEndSpell));
                    }
                }
            }
            else
            {
                Debug.LogError("Behavior is Null");
            }
        }
        else
        {
            Debug.LogError("Spell is Null");
        }
    }

    /// <summary>
    /// Est appel� quand le sort a finit d'�tre lanc�.
    /// </summary>
    /// <param name="spellToEnd">Sort lanc�.</param>
    /// <param name="callback">Action � jouer.</param>
    public static void EndSpell(LaunchedSpellData spellToEnd, Action callback)
    {
        if (spellToEnd != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToEnd.scriptable);

            if (behaviorUsed != null)
            {
                behaviorUsed.EndSpell(spellToEnd);
                callback?.Invoke();
            }
            else
            {
                Debug.LogError("Behavior is Null");
            }
        }
        else
        {
            Debug.LogError("Spell is Null");
        }
    }

    /// <summary>
    /// R�cup�re le fonctionnement d'un sort de type T.
    /// </summary>
    /// <typeparam name="T">Le type du SpellBehavior recherc�.</typeparam>
    /// <returns>Retourn le SpellBehavior voulut s'il existe. Sinon, retourne null.</returns>
    private RVN_SpellBehavior GetBehaviorOfType<T>() where T : RVN_SpellBehavior
    {
        for(int i = 0; i < spellsBehaviors.Count; i++)
        {
            if(spellsBehaviors[i] is T)
            {
                return spellsBehaviors[i];
            }
        }

        return null;
    }

    /// <summary>
    /// R�cup�re le SpellBehavior utiliant le SpellData demand�.
    /// </summary>
    /// <param name="toCheck">Le SpellData qui doit �tre utilis�.</param>
    /// <returns>Retourn le SpellBehavior voulut s'il existe. Sinon, retourne null.</returns>
    private RVN_SpellBehavior GetSpellBehaviorForSpellData(SpellScriptable toCheck)
    {
        if (toCheck != null)
        {
            for (int i = 0; i < spellsBehaviors.Count; i++)
            {
                if (spellsBehaviors[i].GetSpellDataType() == toCheck.GetType())
                {
                    return spellsBehaviors[i];
                }
            }
        }

        return null;
    }
}
