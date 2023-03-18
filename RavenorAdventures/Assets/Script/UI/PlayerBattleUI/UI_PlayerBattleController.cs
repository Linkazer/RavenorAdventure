using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerBattleController : MonoBehaviour
{
    [SerializeField] private CanvasGroup actionGroup;
    [SerializeField] private CanvasGroup battleOnlyActionGroup;
    [SerializeField] private Image characterPortrait;

    [SerializeField] private UnityEvent<RVN_ComponentHandler> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    private CPN_Character displayedCharacter;

    private void Start()
    {
        RVN_BattleManager.Instance.OnCharacterStartTurn += SetCharacter;
        RVN_BattleManager.Instance.OnStartCombat += SetBattleActionInteractable;
        RVN_BattleManager.Instance.OnEndCombat += SetBattleActionNonInteractable;
        RVN_CombatInputController.Instance.OnSetPlayerInputState += SetInteractibleAction;
    }

    private void OnDestroy()
    {
        if (RVN_BattleManager.Instance != null)
        {
            RVN_BattleManager.Instance.OnCharacterStartTurn -= SetCharacter;
            RVN_BattleManager.Instance.OnStartCombat -= SetBattleActionInteractable;
            RVN_BattleManager.Instance.OnEndCombat -= SetBattleActionNonInteractable;
        }

        if (RVN_CombatInputController.Instance != null)
        {
            RVN_CombatInputController.Instance.OnSetPlayerInputState -= SetInteractibleAction;
        }
    }

    private void SetInteractibleAction(bool toSet)
    {
        actionGroup.interactable = toSet;
    }

    private void SetBattleActionInteractable()
    {
        SetBattleInteractibleAction(true);
    }

    private void SetBattleActionNonInteractable()
    {
        SetBattleInteractibleAction(false);
    }

    private void SetBattleInteractibleAction(bool toSet)
    {
        battleOnlyActionGroup.interactable = toSet;
    }

    public void SetCharacter(CPN_Character nCharacter)
    {
        if (RVN_BattleManager.GetPlayerTeam.Contains(nCharacter))
        {
            if (nCharacter != displayedCharacter)
            {
                UnsetCharacter();

                if (nCharacter != null)
                {
                    displayedCharacter = nCharacter;

                    OnSetCharacter?.Invoke(displayedCharacter);

                    characterPortrait.sprite = displayedCharacter.Scriptable.Portrait;
                    characterPortrait.color = Color.white;
                }
            }
        }
    }

    public void UnsetCharacter()
    {
        if (displayedCharacter != null)
        {
            OnUnsetCharacter?.Invoke();

            displayedCharacter = null;
        }
    }
}
