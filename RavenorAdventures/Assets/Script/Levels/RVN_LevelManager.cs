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

    public DialogueScriptable startDialogue;
    public DialogueScriptable endDialogue;

    public LevelInformation nextLevel;

    public UnityEvent onStartLevel;

    [SerializeField] private List<CharacterTeam> teams;
    
    public List<CPN_Character> GetTeam(int teamIndex)
    {
        return teams[teamIndex].characters;
    }
}
