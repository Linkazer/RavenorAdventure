using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains the main logic of the Battle phase.
/// </summary>
public class RVN_BattleManager : RVN_Singleton<RVN_BattleManager>
{
    [Serializable]
    public class CombatTeam
    {
        public CharacterAllegeance allegeance;
        [SerializeField] public List<CPN_Character> characters = new List<CPN_Character>();
    }

    [SerializeField] private RVN_LevelManager level;

    [SerializeField] private List<CombatTeam> teams = new List<CombatTeam>();

    private List<CPN_Character> playedThisTurn = new List<CPN_Character>();
    private int currentPlayingTeam = 0;

    private CPN_Character currentPlayingCharacter;

    [SerializeField] private UnityEvent<List<CPN_Character>> OnSetPlayerTeam;

    [SerializeField] private UnityEvent<CPN_Character> OnStartCharacterTurn;
    [SerializeField] private UnityEvent<CPN_Character> OnStartPlayerCharacterTurn;
    [SerializeField] private UnityEvent<CPN_Character> OnStartAICharacterTurn;
    [SerializeField] private UnityEvent OnStartPlayerTurn;
    [SerializeField] private UnityEvent OnStartAITurn;
    [SerializeField] private UnityEvent<CPN_Character> OnEndCharacterTurn;
    [SerializeField] private UnityEvent OnBeginNewRound;

    public static Action OnPlayerTeamDie;
    public static Action OnEnnemyTeamDie;

    [Header("Combat End")]
    [SerializeField] private UnityEvent OnWinBattle;
    [SerializeField] private UnityEvent OnLoseBattle;

    public static List<CPN_Character> GetPlayerTeam => instance.teams[0].characters;
    public static List<CPN_Character> GetEnemyTeam => instance.teams[1].characters;

    private void Start()
    {
        SetBattle();
    }

    public void SetBattle()
    {
        for(int i = 0; i < level.GetTeam(0).Count; i++)
        {
            teams[0].characters.Add(level.GetTeam(0)[i]);

            teams[0].characters[i].SetCharacter();
        }

        for (int i = 0; i < level.GetTeam(1).Count; i++)
        {
            teams[1].characters.Add(level.GetTeam(1)[i]);

            teams[1].characters[i].SetCharacter();
        }

        OnSetPlayerTeam?.Invoke(teams[0].characters);

        StartBattle();
    }

    public void StartBattle()
    {
        StartNewRound();
    }

    /// <summary>
    /// Set a character turn if it's available.
    /// </summary>
    /// <param name="characterToPlay">The character we want to set the turn.</param>
    public static void TrySetCharacterTurn(CPN_Character characterToPlay)
    {
        if (instance.currentPlayingCharacter != characterToPlay && CanCharacterStartTurn(characterToPlay))
        {
            instance.StartCharacterTurn(characterToPlay);
        }
    }

    /// <summary>
    /// Start the turn of a Character.
    /// </summary>
    /// <param name="characterToPlay">The character.</param>
    public void StartCharacterTurn(CPN_Character characterToPlay)
    {
        if (CanCharacterStartTurn(characterToPlay))
        {
            currentPlayingCharacter = characterToPlay;

            OnStartCharacterTurn?.Invoke(characterToPlay);

            if (teams[0].characters.Contains(characterToPlay))
            {
                OnStartPlayerCharacterTurn?.Invoke(characterToPlay);//TO DO  : Mettre un vérification une fois les IA faites
            }
            else
            {
                OnStartAICharacterTurn?.Invoke(characterToPlay);
            }
        }
    }

    /// <summary>
    /// Check if a character can start its turn.
    /// </summary>
    /// <param name="characterToCheck">The character to check.</param>
    /// <returns></returns>
    public static bool CanCharacterStartTurn(CPN_Character characterToCheck)
    {
        return !instance.playedThisTurn.Contains(characterToCheck) && instance.teams[instance.currentPlayingTeam].characters.Contains(characterToCheck);
    }

    /// <summary>
    /// End the turn of the current Character.
    /// </summary>
    public static void EndCharacterTurn()
    {
        instance.EndCharacterTurn(instance.currentPlayingCharacter);
    }

    /// <summary>
    /// End the turn of a character.
    /// </summary>
    /// <param name="characterToEnd">The character that has his turn end.</param>
    public void EndCharacterTurn(CPN_Character characterToEnd)
    {
        RVN_GridDisplayer.UnsetGridFeedback();

        if (!playedThisTurn.Contains(characterToEnd))
        {
            playedThisTurn.Add(characterToEnd);
        }

        OnEndCharacterTurn?.Invoke(characterToEnd);

        for(int i = 0; i < teams[currentPlayingTeam].characters.Count; i++)
        {
            if(!playedThisTurn.Contains(teams[currentPlayingTeam].characters[i]))
            {
                StartCharacterTurn(teams[currentPlayingTeam].characters[i]);
                return;
            }
        }

        StartNextTeamRound();
    }

    /// <summary>
    /// Start a new round for a team.
    /// </summary>
    private void StartNextTeamRound()
    {
        for (int j = 0; j < teams[currentPlayingTeam].characters.Count; j++)
        {
            teams[currentPlayingTeam].characters[j].EndTurn();
        }

        currentPlayingTeam = (currentPlayingTeam + 1) % teams.Count;

        for (int j = 0; j < teams[currentPlayingTeam].characters.Count; j++)
        {
            teams[currentPlayingTeam].characters[j].StartTurn();
        }

        if (currentPlayingTeam == 0)
        {
            OnStartPlayerTurn?.Invoke();
            StartNewRound();
        }
        else
        {
            OnStartAITurn?.Invoke();
        }

        StartCharacterTurn(teams[currentPlayingTeam].characters[0]);
    }

    /// <summary>
    /// Start a all new round.
    /// </summary>
    private void StartNewRound()
    {
        playedThisTurn = new List<CPN_Character>();

        //CODE REVIEW : Voir pour séparer la mise à jour de chaque team

        OnBeginNewRound?.Invoke();
    }

    /// <summary>
    /// Add a character to a team.
    /// </summary>
    /// <param name="toAdd">The character ti add.</param>
    /// <param name="teamIndex">The team index.</param>
    public void AddCharacter(CPN_Character toAdd, int teamIndex)
    {
        if (!teams[teamIndex].characters.Contains(toAdd))
        {
            teams[teamIndex].characters.Add(toAdd);
        }
    }
    /// <summary>
    /// Remove a character from a team.
    /// </summary>
    /// <param name="toRemove">The character to remove.</param>
    /// <param name="teamIndex">The team index.</param>
    public void RemoveCharacter(CPN_Character toRemove)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].characters.Contains(toRemove))
            {
                teams[i].characters.Remove(toRemove);
                break;
            }
        }
    }

    public static bool AreCharacterAllies(CPN_Character firstChara, CPN_Character secondChara)
    {
        return instance.GetCharacterTeam(firstChara) == instance.GetCharacterTeam(secondChara);
    }

    private CombatTeam GetCharacterTeam(CPN_Character character)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].characters.Contains(character))
            {
                return teams[i];
            }
        }

        return null;
    }

    private int GetCharacterTeamIndex(CPN_Character character)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].characters.Contains(character))
            {
                return i;
            }
        }

        return -1;
    }

    public static List<CPN_Character> GetAllCharacter()
    {
        List<CPN_Character> toReturn = new List<CPN_Character>();

        for (int i = 0; i < instance.teams.Count; i++)
        {
            foreach(CPN_Character chara in instance.teams[i].characters)
            {
                toReturn.Add(chara);
            }
        }

        return toReturn;
    }


    public void OnCharacterDie(CPN_Character diedCharacter)
    {
        CombatTeam toCheck = GetCharacterTeam(diedCharacter);

        if (currentPlayingCharacter == diedCharacter)
        {
            EndCharacterTurn();
        }

        RemoveCharacter(diedCharacter);

        if (toCheck.characters.Count <= 0)
        {
            if(toCheck.allegeance == CharacterAllegeance.Player)
            {
                OnPlayerTeamDie?.Invoke();
            }
            else if(toCheck.allegeance == CharacterAllegeance.Ennemy)
            {
                OnEnnemyTeamDie?.Invoke();
            }
        }
    }

    public static void WinBattle()
    {
        instance.OnWinBattle?.Invoke();
    }

    public static void LoseBattle()
    {
        instance.OnLoseBattle?.Invoke();
    }
}
