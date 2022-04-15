using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public void StartCharacterTurn(CPN_Character characterToPlay)
    {
        if (!playedThisTurn.Contains(characterToPlay) && teams[currentPlayingTeam].characters.Contains(characterToPlay))
        {
            currentPlayingCharacter = characterToPlay;

            OnStartCharacterTurn?.Invoke(characterToPlay);
        }
    }

    public void EndCharacterTurn()
    {
        EndCharacterTurn(currentPlayingCharacter);
    }

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

    private void StartNextTeamRound()
    {
        currentPlayingTeam = (currentPlayingTeam + 1) % teams.Count;

        if(currentPlayingTeam == 0)
        {
            StartNewRound();
        }

        StartCharacterTurn(teams[currentPlayingTeam].characters[0]);
    }

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

    public void AddCharacter(CPN_Character toAdd, int teamIndex)
    {
        if (!teams[teamIndex].characters.Contains(toAdd))
        {
            teams[teamIndex].characters.Add(toAdd);
        }
    }

    public void RemoveCharacter(CPN_Character toRemove, int teamIndex)
    {
        if (teams[teamIndex].characters.Contains(toRemove))
        {
            teams[teamIndex].characters.Remove(toRemove);
        }
    }
}
