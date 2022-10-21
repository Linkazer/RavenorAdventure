using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomHandler : MonoBehaviour
{
    [Serializable]
    private class SpawnableCharacterTeam
    {
        public int teamIndex = 1;
        public List<CPN_Character> charaToSpawns;
        
    }

    [SerializeField] private List<SpawnableCharacterTeam> teams;
    [SerializeField] private UnityEvent OnOpenDoor;

    private bool hasSpawned = false;

    public void OpenRoom()
    {
        if (!hasSpawned)
        {
            foreach (SpawnableCharacterTeam team in teams)
            {
                foreach (CPN_Character chara in team.charaToSpawns)
                {
                    RVN_BattleManager.SpawnCharacter(chara, team.teamIndex);
                }
            }

            OnOpenDoor?.Invoke();

            hasSpawned = true;
        }
    }
}
