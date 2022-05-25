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
    public abstract bool IsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case cibl�.
    /// </summary>
    /// <param name="spellToUse">Sort � lancer.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <param name="callback">Action � jou� � la fin du lancment du sort.</param>
    public abstract void UseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback = null);

    /// <summary>
    /// Est appel� quand le sort a finit d'�tre lanc�.
    /// </summary>
    /// <param name="spellToEnd">Sort lanc�.</param>
    public abstract void EndSpell(LaunchedSpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de SpellData utilis� par le SpellBehavior.
    /// </summary>
    /// <returns></returns>
    public abstract Type GetSpellDataType();
}

public abstract class RVN_SpellBehavior<T> : RVN_SpellBehavior where T : SpellScriptable
{
    /// <summary>
    /// Est appel� pour v�rifier si le sort peut �tre lanc�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToCheck">Sort � v�rifier.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <returns></returns>
    protected abstract bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case cibl�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToUse">Sort � lancer.</param>
    /// <param name="targetNode">Case vis�e.</param>
    /// <param name="callback">Action � jou� � la fin du lancment du sort.</param>
    /// <returns>Return true if the spell has been correctly used.</returns>
    protected abstract bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appel� quand le sort a finit d'�tre lanc�. Contient la logique sp�cifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToEnd">Sort lanc�.</param>
    protected abstract void OnEndSpell(LaunchedSpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de sort utilis� par le Behavior.
    /// </summary>
    /// <returns></returns>
    public override Type GetSpellDataType()
    {
        return typeof(T);
    }

    /// <summary>
    /// R�cup�re le Scriptable du LaunchedSpellData avec le type g�n�rique. (Permet de r�cup�rer des valeurs sp�cifiques (Ex : R�cup�rer les d�g�ts sur un scriptable de sort de d�g�ts)).
    /// </summary>
    /// <param name="spellData"></param>
    /// <returns>Le scriptable avec le bon type.</returns>
    protected T GetScriptable(LaunchedSpellData spellData)
    {
        return spellData.GetScriptableAs<T>();
    }

    public override bool IsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return OnIsSpellUsable(spellToCheck, targetNode);
    }

    public override void UseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        OnUseSpell(spellToUse, targetNode, callback);
    }

    public override void EndSpell(LaunchedSpellData spellToEnd)
    {
        OnEndSpell(spellToEnd);
    }

    protected void ApplyEffects(CPN_EffectHandler target, EffectScriptable effectToApply)
    {
        target.ApplyEffect(effectToApply);
    }
}
