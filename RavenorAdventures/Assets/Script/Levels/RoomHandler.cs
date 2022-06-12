using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomHandler : MonoBehaviour
{
    [Serializable]
    private class SpawnableCharacter
    {
        public CPN_Character toSpawn;
        public int team = 1;
    }

    [SerializeField] private List<SpawnableCharacter> characters;
    [SerializeField] private UnityEvent OnOpenDoor;

    public void OpenRoom()
    {
        foreach(SpawnableCharacter chara in characters)
        {
            RVN_BattleManager.SpawnCharacter(chara.toSpawn, chara.team);

            OnOpenDoor?.Invoke();
        }
    }
}
