using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RVN_SpellBehavior : MonoBehaviour
{
    /// <summary>
    /// Vérifie si le sort peut être utilisé.
    /// </summary>
    /// <param name="spellToCheck">Sort à vérifier.</param>
    /// <param name="targetNode">Case visée.</param>
    /// <returns>TRUE si le sort peut être lancé.</returns>
    public abstract bool IsSpellUsable(SpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case ciblé.
    /// </summary>
    /// <param name="spellToUse">Sort à lancer.</param>
    /// <param name="targetNode">Case visée.</param>
    /// <param name="callback">Action à joué à la fin du lancment du sort.</param>
    public abstract void UseSpell(SpellData spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appelé quand le sort a finit d'être lancé.
    /// </summary>
    /// <param name="spellToEnd">Sort lancé.</param>
    public abstract void EndSpell(SpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de SpellData utilisé par le SpellBehavior.
    /// </summary>
    /// <returns></returns>
    public abstract Type GetSpellDataType();
}

public abstract class RVN_SpellBehavior<T> : RVN_SpellBehavior where T : SpellData
{
    /// <summary>
    /// Est appelé pour vérifier si le sort peut être lancé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToCheck"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    protected abstract bool OnIsSpellUsable(T spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case ciblé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToUse">Sort à lancer.</param>
    /// <param name="targetNode">Case visée.</param>
    ///  /// <param name="callback">Action à joué à la fin du lancment du sort.</param>
    protected abstract void OnUseSpell(T spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appelé quand le sort a finit d'être lancé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToEnd">Sort lancé.</param>
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
