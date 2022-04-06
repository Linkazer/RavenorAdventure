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
    public static bool CanUseSpell(SpellData spellToCheck, Node targetNode)
    {
        if (spellToCheck != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToCheck);

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
    public static void UseSpell(SpellData spellToUse, Node targetNode, Action callbackEndSpell)
    {
        if (spellToUse != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToUse);

            if (behaviorUsed != null)
            {
                behaviorUsed.UseSpell(spellToUse, targetNode, () => EndSpell(spellToUse, callbackEndSpell));
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
    public static void EndSpell(SpellData spellToEnd, Action callback)
    {
        if (spellToEnd != null)
        {
            RVN_SpellBehavior behaviorUsed = instance.GetSpellBehaviorForSpellData(spellToEnd);

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
    private RVN_SpellBehavior GetSpellBehaviorForSpellData(SpellData toCheck)
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
