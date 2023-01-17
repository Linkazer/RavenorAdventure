using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LevelManager : RVN_Singleton<RVN_LevelManager>
{
    public DialogueScriptable startDialogue;
    public DialogueScriptable endDialogue;

    public RVN_LevelManager nextLevel;

    [Serializable]
    private struct CharacterTeam
    {
        public List<CPN_Character> characters;
    }
    [SerializeField] private List<CharacterTeam> teams;
    
    public List<CPN_Character> GetTeam(int teamIndex)
    {
        return teams[teamIndex].characters;
    }
}
