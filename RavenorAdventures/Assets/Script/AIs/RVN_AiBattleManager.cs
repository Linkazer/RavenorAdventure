using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_AiBattleManager : RVN_Singleton<RVN_AiBattleManager>
{
    [Serializable]
    public class Ai_PlannedAction
    {
        public int actionIndex;
        public Node movementTarget;
        public Node actionTarget;
    }

    [SerializeField] private CPN_Character currentCharacter;
    [SerializeField] private CPN_HealthHandler currentCharacterHealth;
    [SerializeField] private CPN_SpellCaster currentCharacterSpell;
    [SerializeField] private CPN_Movement currentCharacterMovement;

    [SerializeField] private Ai_PlannedAction plannedAction;

    [ContextMenu("Test Deplacement")]
    public void TestDeplacementDisplay()
    {
        BeginCharacterTurn(currentCharacter);
    }

    public void BeginCharacterTurn(CPN_Character toBeginTurn)
    {
        currentCharacter = toBeginTurn;
        if(currentCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler nHealth))
        {
            currentCharacterHealth = nHealth;
        }

        if (currentCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster nCaster))
        {
            currentCharacterSpell = nCaster;
        }

        if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement nMovement))
        {
            currentCharacterMovement = nMovement;
        }

        SearchForNextAction(currentCharacter);

        PrepareNextAction(2f);
    }

    public void EndCharacterTurn(CPN_Character toEndTurn)
    {
        currentCharacter = null;

        RVN_BattleManager.EndCharacterTurn();
    }

    private void PrepareNextAction(float timeToWait)
    {
        Debug.Log("Character Prepare Action");
        TimerManager.CreateGameTimer(timeToWait, DoNextMove);
    }

    private void DoNextMove()
    {
        if(currentCharacterHealth.CurrentHealth <= 0)
        {
            Debug.Log("Character Dead");
            EndCharacterTurn(currentCharacter);
        }
        else if(plannedAction != null)
        {
            if(plannedAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                Debug.Log("Character Move");
                currentCharacterMovement.AskToMoveTo(plannedAction.movementTarget.worldPosition, () => PrepareNextAction(1f));
            }
            else
            {
                Debug.Log("Character Do Action");
                currentCharacterSpell.SelectSpell(plannedAction.actionIndex, false);
                currentCharacterSpell.TryDoAction(plannedAction.actionTarget.worldPosition, () => PrepareNextAction(2f));

                plannedAction = null;
            }
        }
        else
        {
            Debug.Log("Character End turn");
            //Plan for next turn
            EndCharacterTurn(currentCharacter);
        }
    }

    private void SearchForNextAction(CPN_Character caster)
    {
        AI_CharacterScriptable currentAi = caster.Scriptable as AI_CharacterScriptable;

        List<CPN_Character> allTargets = RVN_BattleManager.GetAllCharacter();

        Node casterNode = currentCharacterMovement.CurrentNode;

        List<Ai_PlannedAction> possibleActions = new List<Ai_PlannedAction>();

        foreach(AI_Consideration consideration in currentAi.Comportement)
        {
            foreach(CPN_Character target in allTargets)
            {
                List<Node> possibleMovements = Pathfinding.CalculatePathfinding(casterNode, null, currentCharacterMovement.MovementLeft);

                foreach(Node n in possibleMovements)
                {
                    Ai_PlannedAction actionToCheck = new Ai_PlannedAction();
                    actionToCheck.actionTarget = target.CurrentNode;
                    actionToCheck.movementTarget = n;
                    actionToCheck.actionIndex = consideration.wantedActionIndex;

                    if (CanDoAction(caster, actionToCheck, consideration, false))
                    {
                        //Ajouter le calcul de score

                        possibleActions.Add(actionToCheck);
                    }
                }
            }
        }

        if(possibleActions.Count > 0)
        {
            plannedAction = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
        }
        else
        {
            plannedAction = null;
        }
    }

    private bool CanDoAction(CPN_Character caster, Ai_PlannedAction actionToCheck, AI_Consideration consideration, bool isForNextTurn) //TO DO : Prise en compte des tours prochains
    {
        SpellScriptable action = consideration.wantedAction;

        if(actionToCheck.actionTarget == caster.CurrentNode)
        {
            return false;
        }

        #region Prise en compte des équipes (Mit de côté)
        /*switch(action.HitableTargets)
        {
            case SpellTargets.Self:
                if(caster != target)
                {
                    return false;
                }
                break;
            case SpellTargets.Allies:
                if (!RVN_BattleManager.AreCharacterAllies(caster,target))
                {
                    return false;
                }
                break;
            case SpellTargets.Ennemies:
                if (RVN_BattleManager.AreCharacterAllies(caster, target))
                {
                    return false;
                }
                break;
        }*/
        #endregion

        if(!Grid.IsNodeVisible(actionToCheck.movementTarget, actionToCheck.actionTarget))
        {
            return false;
        }

        //Condition de la Considération de l'IA

        return currentCharacterSpell.IsActionUsable(actionToCheck.movementTarget.worldPosition, actionToCheck.actionTarget.worldPosition, consideration.wantedAction);
    }


    /*private void Calculate

    #region Calculs de Considérations
    public float EvaluateAction()
    {
        
    }

    public float ConsiderationCalcul()
    {
       
    }

    public float GetAbcsissaValue()
    {
        
    }

    private int GetCharacterInArea()
    {
        
    }

    public float ConditionnalCalcul(float x, float k, float c)
    {
        if (c >= 0 && x > k)
        {
            return 1;
        }
        else if (c < 0 && x < k)
        {
            return 1;
        }
        return 0;
    }

    public float AffineCalcul(float x, float k, float c)
    {
        return x * c + k;
    }

    public float LogarythmCalcul(float x, float k, float c)
    {
        return k - (Mathf.Pow(x, c));
    }

    public float ExponentialCalcul(float x, float k, float c)
    {
        return Mathf.Exp(c * x + k);
    }

    public float ExponentialReverseCalcul(float x, float k, float c)
    {
        return 1 / Mathf.Exp(c * x + k);
    }

    public float LogisticalCalcul(float x, float k, float c, bool checkAroundMax)
    {
        if (checkAroundMax)
        {
            return 2 / (1 + (Mathf.Exp(Mathf.Abs(x - k) * c)));
        }
        return 1 / (1 + (Mathf.Exp((x - k) * c)));
    }
    #endregion*/
}
