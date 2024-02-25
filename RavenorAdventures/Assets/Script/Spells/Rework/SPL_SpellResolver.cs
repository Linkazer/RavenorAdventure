using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_SpellResolver
{
    private SPL_CastedSpell spellToResolve;

    private Action endSpellcallback;

    private List<SPL_SpellActionAnimation> runingAnimations = new List<SPL_SpellActionAnimation>();

    private Action postAnimationCallback;

    public SPL_CastedSpell CastedSpellData => spellToResolve;

    public SPL_SpellResolver(SPL_CastedSpell castedSpell, Action callback)
    {
        endSpellcallback = callback;

        spellToResolve = castedSpell;

        StartSpellAction(spellToResolve.SpellData.GetSpellAction(castedSpell));
    }

    public void StartSpellAction(SPL_SpellAction toStart)
    {
        if(toStart.StartAnimations.Length > 0)
        {
            foreach (SPL_SpellActionAnimation anim in toStart.StartAnimations)
            {
                anim.PlayAnimation(toStart, this);
            }

            if(runingAnimations.Count > 0)
            {
                postAnimationCallback = () => TriggerSpellAction(toStart);
            }
            else
            {
                TriggerSpellAction(toStart);
            }
        }
        else
        {
            TriggerSpellAction(toStart);
        }
    }

    public void TriggerSpellAction(SPL_SpellAction toTrigger)
    {
        SPL_SpellActionBehavior spellBehavior = SPL_SpellResolverManager.Instance.GetBehaviorForType(toTrigger);

        spellBehavior.ResolveAction(toTrigger, this);
    }

    public void EndSpellAction(SPL_SpellAction nextAction)
    {
        if(nextAction != null)
        {
            StartSpellAction(nextAction);
        }
        else
        {
            if (runingAnimations.Count == 0)
            {
                EndResolveSpell();
            }
            else
            {
                postAnimationCallback = EndResolveSpell;
            }
        }
    }

    public void EndResolveSpell()
    {
        endSpellcallback?.Invoke();
    }

    public void OnAnimationLaunch(SPL_SpellActionAnimation animationData)
    {
        if (!runingAnimations.Contains(animationData))
        {
            runingAnimations.Add(animationData);
        }
    }

    public void OnAnimationEnd(SPL_SpellActionAnimation endedAnimation)
    {
        if(runingAnimations.Contains(endedAnimation))
        {
            runingAnimations.Remove(endedAnimation);

            if(runingAnimations.Count == 0)
            {
                Action lastAction = postAnimationCallback;

                postAnimationCallback?.Invoke();

                if (lastAction == postAnimationCallback)
                {
                    postAnimationCallback = null;
                }
            }
        }
    }
}
