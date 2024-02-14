using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerBattleController : MonoBehaviour
{
    [SerializeField] private Image characterPortrait;

    [SerializeField] private UI_PlayerActionCountDisplayer[] actionCounts;

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
                if (displayedCharacter.TryGetComponent(out CPN_SpellCaster characterCaster))
                {
                    characterCaster.actOnSetActionLeft += UpdateCharacterAction;

                    for (int i = 0; i < actionCounts.Length; i++)
                    {
                        if (i < characterCaster.ActionByTurn)
                        {
                            actionCounts[i].SetVisible(true);

                            if (i < characterCaster.ActionLeftThisTurn)
                            {
                                actionCounts[i].SetAvailable(true);
                            }
                            else
                            {
                                actionCounts[i].SetAvailable(false);
                            }
                        }
                        else
                        {
                            actionCounts[i].SetVisible(false);
                        }
                    }
                }
            }
        }
    }

    public void UnsetCharacter()
    {
        if (displayedCharacter != null)
        {
            if (displayedCharacter.TryGetComponent(out CPN_SpellCaster characterCaster))
            {
                characterCaster.actOnSetActionLeft -= UpdateCharacterAction;

            }

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

    private void UpdateCharacterAction(int actionLeft)
    {

    }
}
