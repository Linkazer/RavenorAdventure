using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manage the Input of the player in the Combat Phase.
/// </summary>
public class RVN_CombatInputController : RVN_Singleton<RVN_CombatInputController>
{
    [SerializeField] private CPN_Character currentCharacter;

    private CPN_Character nextCharacter;

    private CPN_CharacterAction selectedAction = null;

    [SerializeField] private bool canPlayerDoInput = true;

    private Node lastFrameMouseNode = null;

    [SerializeField] private List<MonoBehaviour> disableCount;

    public Action<bool> OnSetPlayerInputState;

    private void Start()
    {
        RVN_BattleManager.Instance.OnCharacterStartTurn += SetCurrentCharacter;
        RVN_BattleManager.Instance.OnStartTeamRound += SetCombatInput;
    }

    private void OnDestroy()
    {
        if(RVN_BattleManager.Instance != null)
        {
            RVN_BattleManager.Instance.OnCharacterStartTurn -= SetCurrentCharacter;
            RVN_BattleManager.Instance.OnStartTeamRound -= SetCombatInput;
        }
    }

    private void Update()
    {
        if (lastFrameMouseNode != Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition))
        {
            lastFrameMouseNode = Grid.GetNodeFromWorldPoint(RVN_InputController.MousePosition);
            if (canPlayerDoInput && selectedAction != null)
            {
                ResetActionDisplay(selectedAction);
            }
        }
    }

    private void LateUpdate()
    {
        if (nextCharacter != null) //CODE REVIEW : A revoir avec le MouseHandler (Eviter de faire une action si on sélectionne un personnage)
        {
            ChangeCharacter(nextCharacter);

            nextCharacter = null;
        }
    }

    /// <summary>
    /// Set the character that the player control.
    /// </summary>
    /// <param name="nCharacter">The character that will be controlled.</param>
    public void SetCurrentCharacter(CPN_Character nCharacter)
    {
        if (RVN_BattleManager.GetPlayerTeam.Contains(nCharacter))
        {
            if (nCharacter != currentCharacter)
            {
                UnselectAction();
            }

            currentCharacter = nCharacter;

            SelectAction(0);
        }
    }

    /// <summary>
    /// Change the current playing character.
    /// </summary>
    /// <param name="nCharacter">The character that will play.</param>
    public static void ChangeCharacter(CPN_Character nCharacter)
    {
        if(instance.canPlayerDoInput && RVN_BattleManager.CanCharacterStartTurn(nCharacter))
        {
            RVN_BattleManager.TrySetCharacterTurn(nCharacter);

            instance.SetCurrentCharacter(nCharacter);
        }
    }

    public static void ChangeCharacter(int characterIndex)
    {
        List<CPN_Character> playerCharacter = RVN_BattleManager.GetPlayerTeam;

        ChangeCharacter(playerCharacter[characterIndex]);
    }

    /// <summary>
    /// Select the Action the character will do.
    /// </summary>
    /// <param name="actionSelected">The ID of the ation chosen.</param>
    public void SelectAction(int actionSelected)//CODE REVIEW : Voir si on peut mettre la sélection de l'action directement dans les component (Avoir une liste de CPN_Action et sélectionner depuis la liste)
    {
        if (currentCharacter != null)
        {
            CPN_CharacterAction nextAction = null;
            switch (actionSelected)
            {
                case 0:
                    if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement movement))
                    {
                        nextAction = movement;
                    }
                    break;
                case 1:
                    if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
                    {
                        nextAction = caster;
                    }
                    break;
            }

            if (nextAction != selectedAction)
            {
                if (!nextAction.CanSelectAction())
                {
                    nextAction = selectedAction;
                }

                if (selectedAction != null && nextAction != null)
                {
                    UnselectAction();
                }

                selectedAction = nextAction;

                DisplayAction(selectedAction);
            }
        }
    }

    /// <summary>
    /// Unselect an Action.
    /// </summary>
    private void UnselectAction()
    {
        if (selectedAction != null)
        {
            selectedAction.UnselectAction();
        }

        UndisplayAction(selectedAction);

        selectedAction = null;
    }

    /// <summary>
    /// Select a spell.
    /// </summary>
    /// <param name="spellIndex">The ID of the spell.</param>
    public void SelectSpell(int spellIndex) //CODE REVIEW : Voir si on peut mettre la sélection du spell dans le CPN_SpellCaster
    {
        if(currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster caster))
        {
            caster.SelectSpell(spellIndex);
        }
    }

    /// <summary>
    /// Fait une action dépendant de l'action sélectionné actuellement.
    /// </summary>
    /// <param name="actionPosition">La position à laquelle faire l'action.</param>
    public void DoAction(Vector2 actionPosition)
    {
        if (currentCharacter != null && canPlayerDoInput)
        {
            if (selectedAction!= null && selectedAction.IsActionUsable(actionPosition))
            {
                if (!(selectedAction is CPN_Movement) || RVN_BattleManager.IsInBattle)
                {
                    DisableCombatInput(this);
                }

                selectedAction.TryDoAction(actionPosition, OnEndAction);

                RVN_GridDisplayer.UnsetGridFeedback();
            }
        }
    }

    /// <summary>
    /// Appelé à la fin d'une action.
    /// </summary>
    private void OnEndAction()
    {
        SelectAction(0);

        EnableCombatInput(this);
    }

    public void DisplayAction(CPN_CharacterAction toDisplay)
    {
        if (toDisplay != null)
        {
            toDisplay.DisplayAction(RVN_InputController.MousePosition);
        }
    }

    public void UndisplayAction(CPN_CharacterAction toUndisplay)
    {
        if (toUndisplay != null)
        {
            toUndisplay.UndisplayAction(RVN_InputController.MousePosition);
        }
    }

    private void ResetActionDisplay(CPN_CharacterAction toReset)
    {
        UndisplayAction(toReset);
        DisplayAction(toReset);
    }

    private void SetCombatInput(int teamIndex)
    {
        if(teamIndex == 0)
        {
            EnableCombatInput(this);
        }
        else
        {
            DisableCombatInput(this);
        }
    }

    public void EnableCombatInput(MonoBehaviour asker)
    {
        if (disableCount.Contains(asker))
        {
            disableCount.Remove(asker);
        }

        if (disableCount.Count <= 0)
        {
            canPlayerDoInput = true;

            OnSetPlayerInputState?.Invoke(true);
        }
    }

    public void DisableCombatInput(MonoBehaviour asker)
    {
        if (disableCount.Count <= 0)
        {
            canPlayerDoInput = false;

            OnSetPlayerInputState?.Invoke(false);
        }

        disableCount.Add(asker);
    }
}
