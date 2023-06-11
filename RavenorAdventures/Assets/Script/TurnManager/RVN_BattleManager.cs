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
    [SerializeField] private UnityEvent<CPN_Character> OnEndCharacterSelfTurn;
    [SerializeField] private UnityEvent OnBeginNewRound;
    [SerializeField] private UnityEvent<CPN_Character> OnSpawnAlly;
    [SerializeField] private UnityEvent<CPN_Character> OnSpawnEnnemy;

    public static Action OnPlayerTeamDie;
    public static Action OnEnnemyTeamDie;
    public static Action<CPN_Character> ActOnCharacterDie;

    [Header("Combat Start")]
    [SerializeField] private UnityEvent beforeBattleStart;

    [Header("Combat End")]
    [SerializeField] private UnityEvent OnWinBattle;
    [SerializeField] private UnityEvent OnLoseBattle;

    public static CPN_Character CurrentCharacter => instance.currentPlayingCharacter;
    public static List<CPN_Character> GetPlayerTeam => instance.teams[0].characters;
    public static List<CPN_Character> GetEnemyTeam => instance.teams[1].characters;

    private void Start()
    {
        if (RVN_SceneManager.CurrentLevel != null)
        {
            if(level != null)
            {
                Destroy(level.gameObject);
            }
        }

        RVN_SceneManager.ToDoAfterLoad += SetBattle;
    }

    private void OnDestroy()
    {
        OnPlayerTeamDie = null;
        OnEnnemyTeamDie = null;
        ActOnCharacterDie = null;
    }

    public void SetBattle()
    {
        if (RVN_SceneManager.CurrentLevel != null)
        {
            level = Instantiate(RVN_SceneManager.CurrentLevel);
        }

        beforeBattleStart?.Invoke();

        for (int i = 0; i < level.GetTeam(0).Count; i++)
        {
            AddCharacter(level.GetTeam(0)[i], 0);
        }

        for (int i = 0; i < level.GetTeam(1).Count; i++)
        {
            AddCharacter(level.GetTeam(1)[i], 1);
        }

        OnSetPlayerTeam?.Invoke(teams[0].characters);

        if (level.startDialogue != null)
        {
            RVN_DialogueManager.PlayDialogue(level.startDialogue, StartBattle);

        }
        else
        {
            TimerManager.CreateGameTimer(Time.deltaTime, StartBattle);
        }
    }

    public void StartBattle()
    {
        level.onStartLevel?.Invoke();

        currentPlayingTeam = -1;

        StartNextTeamRound();
    }

    public void PauseBattle(MonoBehaviour pauseCaller)
    {
        Debug.Log("Pause ?");

        RVN_CombatInputController.instance.DisableCombatInput(pauseCaller);

        RVN_AiBattleManager.instance.Pause();
    }

    public void RestartBattle(MonoBehaviour pauseCaller)
    {
        Debug.Log("Unpause ?");

        RVN_CombatInputController.instance.EnableCombatInput(pauseCaller);

        RVN_AiBattleManager.instance.Restart();
    }

    [ContextMenu("Force Win")]
    public void SetWin()
    {
        EndBattle(true);
    }

    public void EndBattle(bool didWin)
    {
        PauseBattle(this);

        if (didWin)
        {
            if (level.endDialogue != null)
            {
                TimerManager.CreateRealTimer(1f, () => RVN_DialogueManager.PlayDialogue(level.endDialogue, WinBattle));
            }
            else
            {
                TimerManager.CreateRealTimer(1f, WinBattle);
            }
        }
        else
        {
            TimerManager.CreateRealTimer(1f, LoseBattle);
        }
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
        currentPlayingCharacter = characterToPlay;

        if (CanCharacterStartTurn(characterToPlay))
        {
            characterToPlay.ActOnBeginSelfTurn?.Invoke(characterToPlay);

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
        else
        {
            EndCharacterTurn();
        }
    }

    /// <summary>
    /// Check if a character can start its turn.
    /// </summary>
    /// <param name="characterToCheck">The character to check.</param>
    /// <returns></returns>
    public static bool CanCharacterStartTurn(CPN_Character characterToCheck)
    {
        return characterToCheck.canPlay <= 0 && !instance.playedThisTurn.Contains(characterToCheck) && instance.teams[instance.currentPlayingTeam].characters.Contains(characterToCheck);
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

            characterToEnd.EndSelfTurn();
        }

        OnEndCharacterSelfTurn?.Invoke(characterToEnd);

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
        if (currentPlayingTeam >= 0)
        {
            for (int j = 0; j < teams[currentPlayingTeam].characters.Count; j++)
            {
                teams[currentPlayingTeam].characters[j].EndTeamTurn();
            }
        }

        do
        {
            currentPlayingTeam = (currentPlayingTeam + 1) % teams.Count;
        } while (teams[currentPlayingTeam].characters.Count <= 0);

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

        if (teams[currentPlayingTeam].characters.Count > 0)
        {
            Debug.Log("New Round : " + teams[currentPlayingTeam].characters[0]);

            StartCharacterTurn(teams[currentPlayingTeam].characters[0]);
        }
        else
        {
            StartNextTeamRound();
        }
    }

    /// <summary>
    /// Start a all new round.
    /// </summary>
    private void StartNewRound()
    {
        playedThisTurn = new List<CPN_Character>();

        OnBeginNewRound?.Invoke();
    }

    public static void SpawnCharacter(CPN_Character toAdd, int teamIndex)
    {
        toAdd.gameObject.SetActive(true);

        instance.AddCharacter(toAdd, teamIndex);
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

            toAdd.SetCharacter();

            if(teamIndex == 0)
            {
                OnSpawnAlly?.Invoke(toAdd);
            }
            else
            {
                OnSpawnEnnemy?.Invoke(toAdd);
            }
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

    public static int GetCharacterTeamIndex(CPN_Character character)
    {
        for (int i = 0; i < instance.teams.Count; i++)
        {
            if (instance.teams[i].characters.Contains(character))
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

    public static List<CPN_Character> GetEnnemyCharacters(CPN_Character character)
    {
        List<CPN_Character> toReturn = new List<CPN_Character>();

        CombatTeam charaTeam = instance.GetCharacterTeam(character);

        foreach(CombatTeam team in instance.teams)
        {
            if(team != charaTeam)
            {
                foreach (CPN_Character chara in team.characters)
                {
                    toReturn.Add(chara);
                }
            }
        }

        return toReturn;
    }

    public static List<CPN_Character> GetAllyCharacters(CPN_Character character)
    {
        CombatTeam charaTeam = instance.GetCharacterTeam(character);

        return charaTeam.characters;
    }


    public void OnCharacterDie(CPN_Character diedCharacter)
    {
        CombatTeam toCheck = GetCharacterTeam(diedCharacter);

        if (toCheck != null)
        {
            ActOnCharacterDie?.Invoke(diedCharacter);

            if (currentPlayingCharacter == diedCharacter)
            {
                EndCharacterTurn();
            }

            RemoveCharacter(diedCharacter);

            if (toCheck.characters.Count <= 0)
            {
                if (toCheck.allegeance == CharacterAllegeance.Player)
                {
                    OnPlayerTeamDie?.Invoke();
                }
                else if (toCheck.allegeance == CharacterAllegeance.Ennemy)
                {
                    OnEnnemyTeamDie?.Invoke();
                }
            }
        }
    }

    private void WinBattle()
    {
        instance.OnWinBattle?.Invoke();
    }

    private void LoseBattle()
    {
        instance.OnLoseBattle?.Invoke();
    }

    public void RetryBattle()
    {
        RVN_SceneManager.LoadBattle(level);
    }

    public void LoadNextBattle()
    {
        RVN_SceneManager.LoadBattle(level.nextLevel.levelPrefab);
    }
}
