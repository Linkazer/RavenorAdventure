using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_SpawnCharacter : SequenceAction
{
    [SerializeField] private SpawnableCharacterTeam charactersToSpawn;

    protected override void OnStartAction()
    {
        foreach (CPN_Character chara in charactersToSpawn.charaToSpawns)
        {
            RVN_BattleManager.SpawnCharacter(chara, charactersToSpawn.teamIndex);
        }
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }
}
