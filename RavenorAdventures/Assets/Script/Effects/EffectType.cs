using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EffectType
{
    public EffectTrigger trigger;

    public abstract void ApplyEffect(RVN_ComponentHandler effectTarget);
    public abstract void RemoveEffect(RVN_ComponentHandler effectTarget);


    protected abstract void UseEffect(RVN_ComponentHandler effectTarget);
}
