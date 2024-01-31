using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerBattleController : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;

    [SerializeField] private UnityEvent<RVN_ComponentHandler> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    [SerializeField] private Button endTurnButton;

    private CPN_Character displayedCharacter;

    public void SetCharacter(CPN_Character nCharacter)
    {
        if (nCharacter != displayedCharacter)
        {
            UnsetCharacter();

            if (nCharacter != null)
            {
                displayedCharacter = nCharacter;

                OnSetCharacter?.Invoke(displayedCharacter);

                RVN_RoundManager.Instance.actOnUpdateRoundMode += OnChangeRoundMode;
                OnChangeRoundMode(RVN_RoundManager.Instance.CurrentRoundMode);

                characterPortrait.sprite = displayedCharacter.Scriptable.Portrait;
                characterPortrait.color = Color.white;
            }
        }
    }

    public void UnsetCharacter()
    {
        if (displayedCharacter != null)
        {
            OnUnsetCharacter?.Invoke();

            RVN_RoundManager.Instance.actOnUpdateRoundMode -= OnChangeRoundMode;

            displayedCharacter = null;
        }
    }

    public void SelectAction(int actionIndex)
    {
        RVN_CombatInputController.Instance.SelectAction(actionIndex);
    }

    public void EndTurn()
    {
        RVN_CombatInputController.Instance.EndTurn();
    }

    private void OnChangeRoundMode(RVN_RoundManager.RoundMode roundMode)
    {
        switch(roundMode)
        {
            case RVN_RoundManager.RoundMode.Round:
                endTurnButton.interactable = true;
                break;
            case RVN_RoundManager.RoundMode.RealTime:
                endTurnButton.interactable = false;
                break;
        }
    }
}
