using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Character : RVN_ComponentHandler
{
    [SerializeField] private NodeDataHanlder nodeDataHandler;
    [SerializeField] private CharacterScriptable_Battle scriptable;

    [SerializeField] private List<CPN_CharacterAction> actions;

    [SerializeField] private UnityEvent<CharacterScriptable_Battle> OnSetCharacter;
    [SerializeField] private UnityEvent<CPN_Character> OnUnsetCharacter;
    [SerializeField] private UnityEvent OnStartTurn;
    [SerializeField] private UnityEvent OnEndTurn;

    [SerializeField] private List<CPN_Character> characterOnMelee = new List<CPN_Character>();

    public int canPlay;

    public Action<RVN_ComponentHandler> ActOnBeginTeamTurn;
    public Action<RVN_ComponentHandler> ActOnBeginSelfTurn;
    public Action<RVN_ComponentHandler> ActOnEndTeamTurn;
    public Action<RVN_ComponentHandler> ActOnEndSelfTurn;

    public bool IsSet => gameObject.activeSelf;

    public CharacterScriptable_Battle Scriptable => scriptable;

    public NodeDataHanlder CharacterNodeDataHandler => nodeDataHandler;

    protected override void Start()
    {
        //Done to disable the SetHandler at start
    }

    [ContextMenu("Set Character")]
    public void SetNonCpyCharacter()
    {
        SetHandler();

        gameObject.name = scriptable.name;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }

    public void SetCharacter()
    {
        scriptable = Instantiate(scriptable);

        SetHandler();

        gameObject.name = scriptable.Nom;
        gameObject.SetActive(true);

        OnSetCharacter?.Invoke(scriptable);
    }

    /// <summary>
    /// Cr�er une copie du CharacterScriptable et initialise le personnage.
    /// </summary>
    /// <param name="nScriptable">Le scriptable de base du personnage.</param>
    [Obsolete("Check if it is used in UnityEvents")]
    public void SetCharacter(CharacterScriptable_Battle nScriptable)
    {
        Debug.Log("Should pass here");
    }

    /// <summary>
    /// Supprime le personnage.
    /// </summary>
    /// <param name="unsetDelay">D�lai avant la suppr�ssion du personnage</param>
    public void UnsetCharacter(float unsetDelay) //Called in UnityEvents ?
    {
        Debug.Log("Called in UnityEvent ?");
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
    /// D�but du tour du Character
    /// </summary>
    /// <returns>TRUE if the character is still alive.</returns>
    public bool StartCharacterRound()
    {
        ActOnBeginTeamTurn?.Invoke(this);
        OnStartTurn?.Invoke();

        foreach(RVN_Component cpnt in components)
        {
            cpnt.OnStartHandlerRound();
        }

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].ResetData();
        }

        if (GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler selfHealth))
        {
            if (!selfHealth.IsAlive)
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Fin du tour du Character
    /// </summary>
    public void EndCharacterRound()
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnEndHandlerRound();
        }

        ActOnEndSelfTurn?.Invoke(this);
    }
    /// <summary>
    /// D�but du tour de la Team
    /// </summary>
    public void StartTeamRound()
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnStartHandlerGroupRound();
        }
    }
    /// <summary>
    /// Fin du tour de la Team
    /// </summary>
    public void EndTeamRound()
    {
        foreach (RVN_Component cpnt in components)
        {
            cpnt.OnEndHandlerGroupRound();
        }

        ActOnEndTeamTurn?.Invoke(this);
        OnEndTurn?.Invoke();
    }
}
