using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_CheckCharactersDeath : SequenceAction
{
    [SerializeField] private int deathNumberWanted = -1;
    [SerializeField] private List<RVN_ComponentHandler> charactersToCheck;

    protected override void OnStartAction()
    {
        if(deathNumberWanted <= 0)
        {
            deathNumberWanted = charactersToCheck.Count;
        }

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

        if (charactersToCheck.Contains(deadCharacter))
        {
            Debug.Log("Remove");
            charactersToCheck.Remove(deadCharacter);
            deathNumberWanted--;
        }

        Debug.Log(deathNumberWanted);

        if(deathNumberWanted == 0)
        {
            EndAction();
        }
    }
}
