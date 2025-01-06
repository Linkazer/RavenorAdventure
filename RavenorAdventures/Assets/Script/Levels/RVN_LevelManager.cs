using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RVN_LevelManager : RVN_Singleton<RVN_LevelManager>
{
    [Serializable]
    private struct CharacterTeam
    {
        public List<CPN_Character> characters;
    }

    [SerializeField] private LevelInformation levelInformation;

    [SerializeField] private Transform cameraStartPosition;
    [SerializeField] private float cameraStartZoom;

    [Header("Cutscenes")]
    [SerializeField] private SequenceCutscene startCutscene;
    [SerializeField] private SequenceCutscene endCutscene;

    [Header("Level End")]
    [SerializeField] private RVN_LevelEnd[] possibleEnds;

    public UnityEvent onStartLevel;

    [SerializeField] private List<CharacterTeam> teams;

    public LevelInformation LevelData => levelInformation;
    public Transform CameraStartPosition => cameraStartPosition;
    public float CameraStartZoom => cameraStartZoom;

    public SequenceCutscene StartCutscene => startCutscene;
    public SequenceCutscene EndCutscene => endCutscene;
    
    public List<CPN_Character> GetTeam(int teamIndex)
    {
        if (teamIndex < teams.Count)
        {
            return teams[teamIndex].characters;
        }

        return new List<CPN_Character>();
    }

    public void SetEnds()
    {
        foreach(RVN_LevelEnd end in possibleEnds)
        {
            end.SetLevelEnd();
        }
    }
}
