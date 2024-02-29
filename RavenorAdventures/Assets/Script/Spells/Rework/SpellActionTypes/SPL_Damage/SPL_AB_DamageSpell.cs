using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.VirtualTexturing;

public class SPL_AB_DamageSpell : SPL_SpellActionBehavior<SPL_AS_DamageSpell>
{
    protected override void ResolveAction(SPL_AS_DamageSpell actionToResolve, SPL_SpellResolver spellResolver)
    {
        bool didHit = false;

        Node casterNode = spellResolver.CastedSpellData.Caster?.CurrentNode;
        Node targetNode = spellResolver.CastedSpellData.TargetNode;

        foreach (Node node in actionToResolve.Shape.GetZone(casterNode, targetNode))
        {
            if(ResolveOnNode(actionToResolve, spellResolver.CastedSpellData, node) && !didHit)
            {
                didHit = true;
            }
        }

        foreach (SPL_SpellActionAnimation anim in actionToResolve.DamageAnimations)
        {
            anim.PlayAnimation(actionToResolve, spellResolver);
        }

        SPL_SpellAction nextAction = null;

        if (didHit)
        {
            nextAction = actionToResolve.GetTouchAction(spellResolver.CastedSpellData);
        }
        else
        {
            nextAction = actionToResolve.GetNoTouchAction(spellResolver.CastedSpellData);
        }

        if(nextAction == null)
        {
            nextAction = actionToResolve.GetNextAction(spellResolver.CastedSpellData);
        }

        EndResolve(nextAction, spellResolver);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionToResolve"></param>
    /// <param name="castData"></param>
    /// <param name="targetNode"></param>
    /// <returns>Did Hit someone</returns>
    private bool ResolveOnNode(SPL_AS_DamageSpell actionToResolve, SPL_CastedSpell castData, Node targetNode)
    {
        List<CPN_HealthHandler> hitableObjects = targetNode.GetNodeComponent<CPN_HealthHandler>();

        SPL_DamageActionData actionData = null;

        List<KeyValuePair<SPL_DamageType, int>> damagesByType = new List<KeyValuePair<SPL_DamageType, int>>();

        bool didHitAtLeastOne = false;

        foreach(CPN_HealthHandler hitedObject in hitableObjects)
        {
            //TODO Spell Rework : Check target possible

            //Roll dices
            List<Dice> actionDices = DiceManager.GetDices(actionToResolve.DiceUsed, 6, castData.Caster, castData.Caster != null ? castData.Caster.Accuracy : 0);

            RollDices(actionToResolve, actionDices, castData.Caster, hitedObject, out bool didHit);

            hitedObject.DisplayDiceResults(actionDices);

            damagesByType = new List<KeyValuePair<SPL_DamageType, int>>();

            actionData = new SPL_DamageActionData(castData.Caster, hitedObject, actionDices, didHit, damagesByType);

            //Calculate all damages
            foreach (SPL_DamageData damageData in actionToResolve.DamageData)
            {
                if (damageData.NeedHit && !didHit)
                {
                    continue;
                }
                else
                {
                    int damageToAdd = damageData.Origin.GetDamageAmount(actionData);

                    if (damageToAdd > 0)
                    {
                        damagesByType.Add(new KeyValuePair<SPL_DamageType, int>(damageData.DamageType, damageToAdd));
                    }
                }
            }

            //Damage Overriders
            if(castData.Caster != null)
            {
                foreach(DamageOverrider dmgOverride in castData.Caster.DoneDamageOverriders)
                {
                    int damageToAdd = dmgOverride.damageOverrider.Origin.GetDamageAmount(actionData);
                    damagesByType.Add(new KeyValuePair<SPL_DamageType, int>(dmgOverride.damageOverrider.DamageType, damageToAdd));
                }
            }

            if(hitedObject != null)
            {
                foreach (DamageOverrider dmgOverride in hitedObject.ReceivedDamageOverriders)
                {
                    int damageToAdd = dmgOverride.damageOverrider.Origin.GetDamageAmount(actionData);
                    damagesByType.Add(new KeyValuePair<SPL_DamageType, int>(dmgOverride.damageOverrider.DamageType, damageToAdd));
                }
            }

            bool alreadyLostArmor = false;

            //Apply all Damages
            foreach(KeyValuePair<SPL_DamageType, int> damageByType in damagesByType)
            {
                switch(damageByType.Key)
                {
                    case SPL_DamageType.Normal:
                        int damageAmount = damageByType.Value - hitedObject.CurrentArmor;
                        if(damageAmount < 0)
                        {
                            damageAmount = 0;
                        }
                        hitedObject.TakeDamage(castData.Caster, damageAmount);
                        if (!alreadyLostArmor)
                        {
                            hitedObject.RemoveArmor(1);
                            alreadyLostArmor = true;
                        }
                        break;
                    case SPL_DamageType.Heal:
                        hitedObject.TakeHeal(damageByType.Value);
                        break;
                    case SPL_DamageType.IgnoreArmor:
                        hitedObject.TakeDamage(castData.Caster, damageByType.Value);
                        break;
                    case SPL_DamageType.PierceArmor:
                        hitedObject.RemoveArmor(damageByType.Value);
                        alreadyLostArmor = true;
                        break;
                    case SPL_DamageType.RegenArmor:
                        hitedObject.AddArmor(damageByType.Value);
                        break;
                }

                if (didHit)
                {
                    didHitAtLeastOne = true;

                    //Trigger des effets de dégâts
                    if (castData.Caster != null)
                    {
                        castData.Caster.actOnDealDamageSelf?.Invoke(castData.Caster.Handler);
                        castData.Caster.actOnDealDamageTarget?.Invoke(hitedObject.Handler);
                        hitedObject.actOnAttackReceivedTowardTarget?.Invoke(castData.Caster.Handler);
                        hitedObject.actOnAttackReceivedTowardSelf?.Invoke(hitedObject.Handler);
                    }
                }
            }
        }

        return didHitAtLeastOne;
    }

    private void RollDices(SPL_AS_DamageSpell spellUsed, List<Dice> dicesToRoll, CPN_SpellCaster caster,  CPN_HealthHandler target, out bool didHit)
    {
        didHit = false;

        float totalHits = 0;
        int currentOffensiveRerolls = -target.DefensiveRerollsMalus;
        int currentDefensiveRerolls = 0;
        if (caster != null)
        {
            currentDefensiveRerolls  = - caster.OffensiveRerollsMalus;
        }


        for (int i = 0; i < dicesToRoll.Count; i++)
        {
            totalHits += CheckDiceHit(caster, dicesToRoll[i], target.Defense, currentOffensiveRerolls < caster?.OffensiveRerolls, currentDefensiveRerolls < target.DefensiveRerolls, out bool usedOffensiveReroll, out bool usedDefensiveReroll);

            if (usedDefensiveReroll)
            {
                currentDefensiveRerolls++;
                dicesToRoll[i].rerolled += 1;
            }

            if (usedOffensiveReroll)
            {
                currentOffensiveRerolls++;
                dicesToRoll[i].rerolled += 2;
            }
        }

        if (totalHits > 0 || dicesToRoll.Count == 0)
        {
            didHit = true;
        }
    }

    private int CheckDiceHit(CPN_SpellCaster caster, Dice dice, int defense, bool hasOffensiveReroll, bool hasDefensiveReroll, out bool usedOffensiveReroll, out bool usedDefensiveReroll)
    {
        int toReturn = 0;

        usedDefensiveReroll = false;
        usedOffensiveReroll = false;

        if (dice.Result > defense)
        {
            if (hasDefensiveReroll)
            {
                usedDefensiveReroll = true;
                dice.Roll(caster);
                return CheckDiceHit(caster, dice, defense, hasOffensiveReroll, false, out usedOffensiveReroll, out bool def);
            }
            else
            {
                dice.succeed = true;
                toReturn = 1;
            }
        }
        else if (hasOffensiveReroll)
        {
            usedOffensiveReroll = true;
            dice.Roll(caster);
            return CheckDiceHit(caster, dice, defense, false, hasDefensiveReroll, out bool off, out usedDefensiveReroll);
        }

        return toReturn;
    }
}
