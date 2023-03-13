using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Afflicition
{
    Stun,
}

public class EFF_AfflictionEffect : Effect
{
    [SerializeField] private Afflicition toApply;

    protected override void UseEffect(RVN_ComponentHandler effectTarget)
    {
        switch(toApply)
        {
            case Afflicition.Stun:
                if(effectTarget is CPN_Character)
                {
                    (effectTarget as CPN_Character).canPlay++;
                }
                break;
        }
    }

    protected override void UndoEffect(RVN_ComponentHandler effectTarget)
    {
        switch (toApply)
        {
            case Afflicition.Stun:
                if (effectTarget is CPN_Character)
                {
                    (effectTarget as CPN_Character).canPlay--;
                }
                break;
        }
    }

    
}
