using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_ComponentHandler
{
    [SerializeField] private CharacterScriptable_Battle scriptable;

    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable_Battle> OnSetCharacter;
    [SerializeField] private UnityEvent<CPN_Character> OnUnsetCharacter;
    [SerializeField] private UnityEvent OnStartTurn;
    [SerializeField] private UnityEvent OnEndTurn;

    [SerializeField] private List<CPN_Character> characterOnMelee = new List<CPN_Character>();

    public int canPlay;

    public Action<RVN_ComponentHandler> ActOnBeginTurn;
    public Action<RVN_ComponentHandler> ActOnEndTeamTurn;
    public Action<RVN_ComponentHandler> ActOnEndSelfTurn;

    public bool IsSet => gameObject.activeSelf;

    public CharacterScriptable_Battle Scriptable => scriptable;

    [ContextMenu("Set Character")]
    public void SetCharacter()
    {
        SetCharacterNonCopy(scriptable);
    }

    /// <summary>
    /// Créer une copie du CharacterScriptable et initialise le personnage.
    /// </summary>
    /// <param name="nScriptable">Le scriptable de base du personnage.</param>
    public void SetCharacter(CharacterScriptable_Battle nScriptable)
    {
        scriptable = Instantiate(nScriptable);

        gameObject.name = scriptable.Nom;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }

    /// <summary>
    /// Initialise le personnage avec le scriptable voulut.
    /// </summary>
    /// <param name="nScriptable">Le scriptable du personnage.</param>
    public void SetCharacterNonCopy(CharacterScriptable_Battle nScriptable)
    {
        scriptable = nScriptable;

        gameObject.name = scriptable.Nom;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }

    /// <summary>
    /// Supprime le personnage.
    /// </summary>
    /// <param name="unsetDelay">Délai avant la suppréssion du personnage</param>
    public void UnsetCharacter(float unsetDelay)
    {
        scriptable = null;

        RVN_BattleManager.Instance.OnCharacterDie(this);

        OnUnsetCharacter?.Invoke(this);

        if (unsetDelay > 0)
        {
            TimerManager.CreateGameTimer(unsetDelay, () => gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Début du tour du personnage.
    /// </summary>
    public void StartTurn()
    {
        ActOnBeginTurn?.Invoke(this);
        OnStartTurn?.Invoke();

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetData();
        }
    }

    /// <summary>
    /// Fin du tour du personnage.
    /// </summary>
    public void EndSelfTurn()
    {
        ActOnEndSelfTurn?.Invoke(this);
    }

    /// <summary>
    /// Fin du tour de l'équipe du personnage.
    /// </summary>
    public void EndTeamTurn()
    {
        ActOnEndTeamTurn?.Invoke(this);
        OnEndTurn?.Invoke();
    }

    [Obsolete("Flank non utilisé")]
    public void AddMeleeCharacter(CPN_Character toAdd)
    {
        if(!characterOnMelee.Contains(toAdd))
        {
            characterOnMelee.Add(toAdd);

            int enemyCount = 0;

            foreach(CPN_Character chara in characterOnMelee)
            {
                if(!RVN_BattleManager.AreCharacterAllies(chara, this))
                {
                    enemyCount++;
                }
            }
        }
    }

    [Obsolete("Flank non utilisé")]
    public void RemoveMeleeCharacter(CPN_Character toRemove)
    {
        if (characterOnMelee.Contains(toRemove))
        {
            characterOnMelee.Remove(toRemove);

            int enemyCount = 0;

            foreach (CPN_Character chara in characterOnMelee)
            {
                if (!RVN_BattleManager.AreCharacterAllies(chara, this))
                {
                    enemyCount++;
                }
            }
        }
    }
}
