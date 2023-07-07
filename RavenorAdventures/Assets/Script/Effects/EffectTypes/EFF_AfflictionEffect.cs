using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Afflicition
{
    Stun,
    Evasive,
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
                    Debug.Log("Un stun ??");

                    (effectTarget as CPN_Character).canPlay++;
                }
                break;
            case Afflicition.Evasive:
                if(effectTarget.GetComponentOfType<CPN_Movement>(out CPN_Movement evasiveMovement))
                {
                    evasiveMovement.currentEvasiveAmount++;
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
                    Debug.Log("Fin du stun ??");

                    (effectTarget as CPN_Character).canPlay--;
                }
                break;
            case Afflicition.Evasive:
                if (effectTarget.GetComponentOfType<CPN_Movement>(out CPN_Movement evasiveMovement))
                {
                    evasiveMovement.currentEvasiveAmount--;
                }
                break;
        }
    }

    
}
