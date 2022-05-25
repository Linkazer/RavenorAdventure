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
    public abstract bool IsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case ciblé.
    /// </summary>
    /// <param name="spellToUse">Sort à lancer.</param>
    /// <param name="targetNode">Case visée.</param>
    /// <param name="callback">Action à joué à la fin du lancment du sort.</param>
    public abstract void UseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback = null);

    /// <summary>
    /// Est appelé quand le sort a finit d'être lancé.
    /// </summary>
    /// <param name="spellToEnd">Sort lancé.</param>
    public abstract void EndSpell(LaunchedSpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de SpellData utilisé par le SpellBehavior.
    /// </summary>
    /// <returns></returns>
    public abstract Type GetSpellDataType();
}

public abstract class RVN_SpellBehavior<T> : RVN_SpellBehavior where T : SpellScriptable
{
    /// <summary>
    /// Est appelé pour vérifier si le sort peut être lancé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToCheck">Sort à vérifier.</param>
    /// <param name="targetNode">Case visée.</param>
    /// <returns></returns>
    protected abstract bool OnIsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode);

    /// <summary>
    /// Lance le sort voulut sur la case ciblé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToUse">Sort à lancer.</param>
    /// <param name="targetNode">Case visée.</param>
    /// <param name="callback">Action à joué à la fin du lancment du sort.</param>
    /// <returns>Return true if the spell has been correctly used.</returns>
    protected abstract bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback);

    /// <summary>
    /// Est appelé quand le sort a finit d'être lancé. Contient la logique spécifique au type de sort du SpellBehavior.
    /// </summary>
    /// <param name="spellToEnd">Sort lancé.</param>
    protected abstract void OnEndSpell(LaunchedSpellData spellToEnd);

    /// <summary>
    /// Renvoie le type de sort utilisé par le Behavior.
    /// </summary>
    /// <returns></returns>
    public override Type GetSpellDataType()
    {
        return typeof(T);
    }

    /// <summary>
    /// Récupère le Scriptable du LaunchedSpellData avec le type générique. (Permet de récupérer des valeurs spécifiques (Ex : Récupérer les dégâts sur un scriptable de sort de dégâts)).
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
