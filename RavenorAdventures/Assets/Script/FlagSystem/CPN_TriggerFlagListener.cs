using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_TriggerFlagListener : MonoBehaviour
{
    [SerializeField] private string flagToTrigger;
    [SerializeField] private UnityEvent OnTriggerFlagEvent;

    private void OnEnable()
    {
        VLY_FlagManager.AddTriggerFlagListener(flagToTrigger, OnTriggerFlag);
    }

    private void OnDisable()
    {
        VLY_FlagManager.RemoveTriggerFlagListener(flagToTrigger, OnTriggerFlag);
    }

    public void OnTriggerFlag()
    {
        //Debug.Log("OnTriggerFlag");
        OnTriggerFlagEvent?.Invoke();
    }
}
