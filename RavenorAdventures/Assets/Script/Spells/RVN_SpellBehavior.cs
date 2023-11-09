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
    protected abstract bool OnUseSpell(LaunchedSpellData spellToUse, Node targetNode);

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
    protected virtual T GetScriptable(LaunchedSpellData spellData)
    {
        return spellData.GetScriptableAs<T>();
    }

    public override bool IsSpellUsable(LaunchedSpellData spellToCheck, Node targetNode)
    {
        return OnIsSpellUsable(spellToCheck, targetNode);
    }

    public override void UseSpell(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        bool hasAnimationBeenMade = false;

        List<CPN_HealthHandler> hitableObjects = targetNode.GetNodeComponent<CPN_HealthHandler>();

        if (spellToUse.scriptable.AnimationTarget == SpellAnimationTarget.All)
        {
            if (spellToUse.scriptable.AnimationDuration > 0.5f)
            {
                if (callback != null)
                {
                    Debug.Log("Animation ALL 0.5");
                }
                TimerManager.CreateGameTimer(0.5f, callback);
                PlaySpellAnimation(spellToUse, targetNode, null);
            }
            else
            {
                if (callback != null)
                {
                    Debug.Log("Animation ALL Feedback");
                }
                PlaySpellAnimation(spellToUse, targetNode, callback);
            }

            callback = null;
        }

        if (CanUseOnNode(hitableObjects, spellToUse.scriptable.HitableTargets, spellToUse.caster))
        {
            OnUseSpell(spellToUse, targetNode);

            T usedScriptable = GetScriptable(spellToUse);

            if (callback != null)
            {
                if (spellToUse.scriptable.SpellAnimation != null)
                {
                    if (spellToUse.scriptable.AnimationTarget == SpellAnimationTarget.HitedCharacters || spellToUse.scriptable.AnimationTarget == SpellAnimationTarget.Target)
                    {
                        if (usedScriptable.AnimationDuration > 0.5f)
                        {
                            Debug.Log("Animation TARGET 0.5");
                            TimerManager.CreateGameTimer(0.5f, callback);
                            PlaySpellAnimation(spellToUse, targetNode, null);
                        }
                        else
                        {
                            Debug.Log("Animation TARGET Feedback");
                            PlaySpellAnimation(spellToUse, targetNode, callback);
                        }
                    }
                }
                else if (usedScriptable.AnimationDuration < 0.5f)
                {
                    Debug.Log("Animation NO ANIMATION 0.5");
                    TimerManager.CreateGameTimer(0.5f, callback);
                }
                else
                {
                    Debug.Log("Animation NO ANIMATION AnimationDuration");
                    TimerManager.CreateGameTimer(usedScriptable.AnimationDuration, callback);
                }
            }
            else if (spellToUse.scriptable.AnimationTarget == SpellAnimationTarget.HitedCharacters)
            {
                if (usedScriptable.AnimationDuration > 0.5f)
                {
                    Debug.Log("Animation HITTED TARGET 0.5");
                    TimerManager.CreateGameTimer(0.5f, callback);
                    PlaySpellAnimation(spellToUse, targetNode, null);
                }
                else
                {
                    Debug.Log("Animation HITTED TARGET Feedback");
                    PlaySpellAnimation(spellToUse, targetNode, callback);
                }
            }

            foreach (CPN_HealthHandler hitedObject in hitableObjects)
            {
                if (spellToUse.scriptable.Range >= 10 && spellToUse.caster != null)
                {
                    spellToUse.caster.actOnUseSkillTarget?.Invoke(hitedObject.Handler);
                }

                if (hitedObject.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
                {
                    foreach (EffectScriptable eff in spellToUse.scriptable.Effects())
                    {
                        ApplyEffects(effectHandler, eff);
                    }
                }
            }

            if (spellToUse.caster != null)
            {
                if (spellToUse.scriptable.Range >= 10)
                {
                    spellToUse.caster.actOnUseSkillSelf?.Invoke(spellToUse.caster.Handler);
                }

                if (spellToUse.caster.Handler.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
                {
                    foreach (EffectScriptable eff in spellToUse.scriptable.EffectOnCaster)
                    {
                        ApplyEffects(effectHandler, eff);
                    }
                }
            }
        }
        else if(callback != null)
        {
            Debug.Log("SHIT HAPPEN 0.5");
            TimerManager.CreateGameTimer(0.5f, callback);
        }
    }

    private void PlaySpellAnimation(LaunchedSpellData spellToUse, Node targetNode, Action callback)
    {
        T usedScriptable = GetScriptable(spellToUse);

        if (callback != null)
        {
            Debug.Log("Play animation with callback : " + usedScriptable.SpellAnimation);
        }

        if (usedScriptable.SpellAnimation != null)
        {
            AnimationInstantiater.PlayAnimationAtPosition(usedScriptable.SpellAnimation, targetNode.worldPosition, callback);
        }
        else if(callback != null)
        {
            if (usedScriptable.AnimationDuration >= 0.5f)
            {
                TimerManager.CreateGameTimer(usedScriptable.AnimationDuration, callback);
            }
            else
            {
                TimerManager.CreateGameTimer(0.5f, callback);
            }
        }
    }

    public override void EndSpell(LaunchedSpellData spellToEnd)
    {
        OnEndSpell(spellToEnd);
    }

    protected void ApplyEffects(CPN_EffectHandler target, EffectScriptable effectToApply)
    {
        target.ApplyEffect(effectToApply);
    }

    protected bool CanUseOnNode(List<CPN_HealthHandler> hitablesOnNode, SpellTargets targetType, CPN_SpellCaster caster)
    {
        if(targetType == SpellTargets.All)
        {
            return true;
        }
        else
        {
            foreach(CPN_HealthHandler hitedTrgt in hitablesOnNode)
            {
                if(CanUseOnTarget(targetType, caster, hitedTrgt))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected bool CanUseOnTarget(SpellTargets targetType, CPN_SpellCaster caster, CPN_HealthHandler target)
    {
        if (caster != null && target != null)
        {
            CPN_Character casterChara = caster.Handler as CPN_Character;
            CPN_Character targetChara = target.Handler as CPN_Character;

            if (casterChara != null && targetChara != null)
            {
                switch (targetType)
                {
                    case SpellTargets.Self:
                        if (casterChara != targetChara)
                        {
                            return false;
                        }
                        break;
                    case SpellTargets.Allies:
                        if (!RVN_BattleManager.AreCharacterAllies(casterChara, targetChara))
                        {
                            return false;
                        }
                        break;
                    case SpellTargets.Ennemies:
                        if (RVN_BattleManager.AreCharacterAllies(casterChara, targetChara))
                        {
                            return false;
                        }
                        break;
                }
            }
        }

        return true;
    }
}
