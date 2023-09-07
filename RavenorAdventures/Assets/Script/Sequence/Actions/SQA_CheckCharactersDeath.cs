using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharactersDeath : SequenceAction
{
    [SerializeField] private List<RVN_ComponentHandler> charactersToCheck;

    protected override void OnStartAction()
    {
        foreach(CPN_Character chara in charactersToCheck)
        {
            if(chara.GetComponentOfType(out CPN_HealthHandler healthHandler))
            {
                healthHandler.actOnDeath += OnCharacterDeath;
            }
        }
    }

    protected override void OnEndAction()
    {
        foreach (CPN_Character chara in charactersToCheck)
        {
            if (chara.GetComponentOfType(out CPN_HealthHandler healthHandler))
            {
                healthHandler.actOnDeath -= OnCharacterDeath;
            }
        }
    }

    protected override void OnSkipAction()
    {
        foreach (CPN_Character chara in charactersToCheck)
        {
            if (chara.GetComponentOfType(out CPN_HealthHandler healthHandler))
            {
                healthHandler.actOnDeath -= OnCharacterDeath;
            }
        }
    }

    private void OnCharacterDeath(RVN_ComponentHandler deadCharacter)
    {
        if (deadCharacter.GetComponentOfType(out CPN_HealthHandler healthHandler))
        {
            healthHandler.actOnDeath -= OnCharacterDeath;
        }

        charactersToCheck.Remove(deadCharacter);

        if(charactersToCheck.Count == 0)
        {
            EndAction();
        }
    }
}
