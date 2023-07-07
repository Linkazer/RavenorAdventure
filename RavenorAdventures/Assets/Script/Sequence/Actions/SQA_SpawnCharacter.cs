using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_SpawnCharacter : SequenceAction
{
    [SerializeField] private SpawnableCharacterTeam charactersToSpawn;
    [SerializeField] private Transform[] spawnPositions;

    protected override void OnStartAction()
    {
        for(int i = 0; i < charactersToSpawn.charaToSpawns.Count; i++)
        {
            if(spawnPositions.Length != 0)
            {
                RVN_BattleManager.SpawnCharacter(charactersToSpawn.charaToSpawns[i], charactersToSpawn.teamIndex, spawnPositions[i].position);
            }
            else
            {
                RVN_BattleManager.SpawnCharacter(charactersToSpawn.charaToSpawns[i], charactersToSpawn.teamIndex);
            }
        }
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }
}
