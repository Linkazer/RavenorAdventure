using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharacterHealth : SequenceAction
{
    [SerializeField] private float targetHealth;
    [SerializeField] private RVN_ComponentHandler characterToCheck;

    protected override void OnStartAction()
    {
        if (characterToCheck.GetComponentOfType(out CPN_HealthHandler healthHandler))
        {
            healthHandler.actOnChangeHealth += OnHealthUpdate;
        }
        else
        {
            EndAction();
        }
    }

    protected override void OnEndAction()
    {
        if (characterToCheck.GetComponentOfType(out CPN_HealthHandler healthHandler))
        {
            healthHandler.actOnChangeHealth -= OnHealthUpdate;
        }
    }

    protected override void OnSkipAction()
    {
        if (characterToCheck.GetComponentOfType(out CPN_HealthHandler healthHandler))
        {
            healthHandler.actOnChangeHealth -= OnHealthUpdate;
        }
    }

    private void OnHealthUpdate(float healthLeft)
    {
        if(healthLeft <= targetHealth)
        {
            EndAction();
        }
    }
}
