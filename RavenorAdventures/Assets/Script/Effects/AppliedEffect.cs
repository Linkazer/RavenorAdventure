using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AppliedEffect
{
    [SerializeField] private Effect effect;
    [SerializeField] private int durationLeft;

    public Effect GetEffect => effect;
    public int Duration => durationLeft;

    public void UpdateDuration()
    {
        durationLeft--;
    }

    public void ResetEffect(int resetDuration)
    {
        if (durationLeft < resetDuration)
        {
            durationLeft = resetDuration;
        }
    }

    public void RemoveEffect(RVN_ComponentHandler toRemoveFrom)
    {
        effect.RemoveEffect(toRemoveFrom);
    }

    public AppliedEffect()
    {

    }
    public AppliedEffect(Effect nEffect, int nDuration)
    {
        effect = nEffect;
        durationLeft = nDuration;
    }
}
