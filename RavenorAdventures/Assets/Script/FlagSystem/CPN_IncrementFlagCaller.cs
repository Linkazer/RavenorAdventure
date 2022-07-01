using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_IncrementFlagCaller : MonoBehaviour
{
    [SerializeField] private string flagName;
    [SerializeField] private int incrementValue = 1;

    [ContextMenu("Increment Flag")]
    public void IncrementFlag()
    {
        VLY_FlagManager.IncrementFlagValue(flagName, incrementValue);
    }

    public void IncrementFlag(string nFlagName)
    {
        VLY_FlagManager.IncrementFlagValue(nFlagName, incrementValue);
    }
}
