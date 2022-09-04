using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType
{
    Incremental,
    Trigger,
}

public class DialogueEffect_Flag : DialogueResponseEffect
{
    [SerializeField] private FlagType flagType;
    [SerializeField] private string flagName;
    [SerializeField] private int incrementValue;

    public override void ApplyEffect()
    {
        switch(flagType)
        {
            case FlagType.Incremental:
                VLY_FlagManager.IncrementFlagValue(flagName, incrementValue);
                break;
            case FlagType.Trigger:
                VLY_FlagManager.TriggerFlag(flagName);
                break;
        }
    }
}
