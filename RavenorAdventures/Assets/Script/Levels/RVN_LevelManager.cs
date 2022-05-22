using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LevelManager : RVN_Singleton<RVN_LevelManager>
{
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
