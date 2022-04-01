using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RVN_SpellBehavior : MonoBehaviour
{
    /// <summary>
    /// V�rifie si le sort peut �tre utilis�.
    /// </summary>
    /// <param name="spellToCheck">Sort � v�rifier.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <returns>TRUE si le sort peut �tre lanc�.</returns>
    public abstract bool IsSpellUsable(SpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case cibl�.
    /// </summary>
    /// <param name="spellToUse">Sort � lancer.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <param name="callback">Action � jou� � la fin du lancment du sort.</param>
    public abstract void UseSpell(SpellData spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appel� quand le sort a finit d'�tre lanc�.
    /// </summary>
    /// <param name="spellToEnd">Sort lanc�.</param>
    public abstract void EndSpell(SpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de SpellData utilis� par le SpellBehavior.
    /// </summary>
    /// <returns></returns>
    public abstract Type GetSpellDataType();
}

public abstract class RVN_SpellBehavior<T> : RVN_SpellBehavior where T : SpellData
{
    /// <summary>
    /// Est appel� pour v�rifier si le sort peut �tre lanc�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToCheck"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    protected abstract bool OnIsSpellUsable(T spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case cibl�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToUse">Sort � lancer.</param>
    /// <param name="targetNode">Case vis�e.</param>
    ///  /// <param name="callback">Action � jou� � la fin du lancment du sort.</param>
    protected abstract void OnUseSpell(T spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appel� quand le sort a finit d'�tre lanc�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToEnd">Sort lanc�.</param>
    protected abstract void OnEndSpell(T spellToEnd);

    public override Type GetSpellDataType()
    {
        return typeof(T);
    }

    public override bool IsSpellUsable(SpellData spellToCheck, Node targetNode)
    {
        return OnIsSpellUsable(spellToCheck as T, targetNode);
    }

    public override void UseSpell(SpellData spellToUse, Node targetNode, Action callback)
    {
        OnUseSpell(spellToUse as T, targetNode, callback);
    }

    public override void EndSpell(SpellData spellToEnd)
    {
        OnEndSpell(spellToEnd as T);
    }
}
