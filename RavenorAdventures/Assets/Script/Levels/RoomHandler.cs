using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

[Serializable]
public class SpawnableCharacterTeam
{
    public int teamIndex = 1;
    public bool playBefore = false;
    public List<CPN_Character> charaToSpawns;
}

public class RoomHandler : MonoBehaviour
{
    private const float cameraTime = 2f;

    [SerializeField] private List<SpawnableCharacterTeam> teams;
    [SerializeField] private UnityEvent OnOpenDoor;

    [SerializeField] private CinemachineVirtualCamera doorFocusedCamera;

    private bool hasSpawned = false;
    private TimerManager.Timer doorCameraTimer;

    public void OpenRoom()
    {
        if (!hasSpawned)
        {
            foreach (SpawnableCharacterTeam team in teams)
            {
                if (team.playBefore)
                {
                    for(int i = team.charaToSpawns.Count - 1; i >= 0; i--)
                    {
                        RVN_BattleManager.SpawnCharacter(team.charaToSpawns[i], team.teamIndex, true);
                    }
                }
                else
                {
                    foreach (CPN_Character chara in team.charaToSpawns)
                    {
                        RVN_BattleManager.SpawnCharacter(chara, team.teamIndex);
                    }
                }
            }

            OnOpenDoor?.Invoke();

            if(doorFocusedCamera != null)
            {
                doorFocusedCamera.enabled = true;
                doorCameraTimer = TimerManager.CreateGameTimer(cameraTime, () => doorFocusedCamera.enabled = false);
            }

            hasSpawned = true;
        }
    }

    private void OnDestroy()
    {
        doorCameraTimer?.Stop();
    }
}
