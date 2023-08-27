using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTrigger
{
    OnApply,
    OnEnd,
    OnBeginTurn,
    OnEndTurn,
    OnEnterNode,
    OnExitNode,
    OnTakeDamageTowardSelf,
    OnTakeDamageTowardTarget,
    OnDealDamageTowardSelf,
    OnDealDamageTowardTarget,
    OnDeath,
    OnGetAttackedTowardSelf,
    OnGetAttackedTowardTarget,
    OnAttackTowardSelf,
    OnAttackTowardTarget,
    OnApplyWithoutCancel
}
