using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains the main logic of the Battle phase.
/// </summary>
public class RVN_BattleManager : RVN_Singleton<RVN_BattleManager>
{
    [System.Serializable]
    public class CombatTeam
    {
        [SerializeField] public List<CPN_Character> characters = new List<CPN_Character>();
    }

    [SerializeField] private List<CombatTeam> teams = new List<CombatTeam>();

    private List<CPN_Character> playedThisTurn = new List<CPN_Character>();
    private int currentPlayingTeam = 0;

    private CPN_Character currentPlayingCharacter;

    [SerializeField] private UnityEvent<CPN_Character> OnStartCharacterTurn;
    [SerializeField] private UnityEvent<CPN_Character> OnEndCharacterTurn;
    [SerializeField] private UnityEvent OnBeginNewRound;

    private void Start()
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
    public void EndCharacterTurn()
    {
        EndCharacterTurn(currentPlayingCharacter);
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
        currentPlayingTeam = (currentPlayingTeam + 1) % teams.Count;

        if(currentPlayingTeam == 0)
        {
            StartNewRound();
        }

        StartCharacterTurn(teams[currentPlayingTeam].characters[0]);
    }

    /// <summary>
    /// Start a all new round.
    /// </summary>
    private void StartNewRound()
    {
        playedThisTurn = new List<CPN_Character>();

        for(int i = 0; i < teams.Count; i++)
        {
            for(int j = 0; j < teams[i].characters.Count; j++)
            {
                teams[i].characters[j].StartTurn();
            }
        }

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
    public void RemoveCharacter(CPN_Character toRemove, int teamIndex)
    {
        if (teams[teamIndex].characters.Contains(toRemove))
        {
            teams[teamIndex].characters.Remove(toRemove);
        }
    }
}
