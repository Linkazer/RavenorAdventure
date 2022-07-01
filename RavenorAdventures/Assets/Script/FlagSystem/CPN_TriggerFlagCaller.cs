using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_TriggerFlagCaller : MonoBehaviour
{
    [SerializeField] private string flagToTrigger;

    public void TriggerFlag(string flagName)
    {
        VLY_FlagManager.TriggerFlag(flagName);
    }

    public void TriggerFlag()
    {
        TriggerFlag(flagToTrigger);
    }
}
