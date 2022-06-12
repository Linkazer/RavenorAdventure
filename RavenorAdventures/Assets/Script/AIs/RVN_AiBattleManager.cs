using System;
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

    private bool usedAction = false;

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

        usedAction = false;

        plannedAction = SearchForNextAction(currentCharacter, false);

        PrepareNextAction(2f);
    }

    public void EndCharacterTurn(CPN_Character toEndTurn)
    {
        currentCharacter = null;

        RVN_BattleManager.EndCharacterTurn();
    }

    private void PrepareNextAction(float timeToWait)
    {
        TimerManager.CreateGameTimer(timeToWait, DoNextMove);
    }

    private void DoNextMove()
    {
        if(currentCharacterHealth.CurrentHealth <= 0)
        {
            EndCharacterTurn(currentCharacter);
        }
        else if(plannedAction != null && plannedAction.actionIndex >= 0)
        {
            if(plannedAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                currentCharacterMovement.AskToMoveTo(plannedAction.movementTarget.worldPosition, () => PrepareNextAction(1f));
            }
            else
            {
                currentCharacterSpell.SelectSpell(plannedAction.actionIndex, false);
                currentCharacterSpell.TryDoAction(plannedAction.actionTarget.worldPosition, () => PrepareNextAction(2f));

                usedAction = true;
                plannedAction = null;
            }
        }
        else if(!usedAction && currentCharacterMovement.CanMove)
        {
            usedAction = true;

            Ai_PlannedAction nextTurnAction = SearchForNextAction(currentCharacter, true);

            if(nextTurnAction != null && nextTurnAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                currentCharacterMovement.AskToMoveTo(nextTurnAction.movementTarget.worldPosition, () => PrepareNextAction(1f));
            }
            else
            {
                EndCharacterTurn(currentCharacter);
            }
        }
        else
        {
            EndCharacterTurn(currentCharacter);
        }
    }

    private Ai_PlannedAction SearchForNextAction(CPN_Character caster, bool forNextTurn)
    {
        Ai_PlannedAction toReturn = null;

        AI_CharacterScriptable currentAi = caster.Scriptable as AI_CharacterScriptable;

        List<CPN_Character> allTargets = RVN_BattleManager.GetAllCharacter();

        Node casterNode = currentCharacterMovement.CurrentNode;

        List<Ai_PlannedAction> possibleActions = new List<Ai_PlannedAction>();

        float maxScore = -1;

        foreach(AI_Consideration consideration in currentAi.Comportement)
        {
            foreach(CPN_Character target in allTargets)
            {
                List<Node> possibleMovements = new List<Node>();
                if (forNextTurn)
                {
                    possibleMovements = Pathfinding.CalculatePathfinding(casterNode, currentCharacterMovement.Width, null, currentCharacterMovement.MovementLeft + currentCharacterMovement.MaxMovement);
                }
                else
                {
                    possibleMovements = Pathfinding.CalculatePathfinding(casterNode, currentCharacterMovement.Width, null, currentCharacterMovement.MovementLeft);
                }

                Ai_PlannedAction actionOnTarget = null;

                float minimumDistance = -1;

                foreach (Node n in possibleMovements)
                {
                    Ai_PlannedAction actionToCheck = new Ai_PlannedAction();
                    actionToCheck.actionTarget = target.CurrentNode;
                    actionToCheck.movementTarget = n;
                    actionToCheck.actionIndex = consideration.wantedActionIndex;

                    if (CanDoAction(caster, actionToCheck, consideration, forNextTurn))
                    {
                        float calculatedScore = CalculateActionScore(actionToCheck, consideration, casterNode);

                        if(calculatedScore >= maxScore)
                        {
                            float calculatedDistance = -1;
                            if(forNextTurn)
                            {
                                calculatedDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, actionToCheck.actionTarget);
                            }
                            else
                            {
                                calculatedDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, casterNode);
                            }

                            if (actionOnTarget == null || calculatedDistance < minimumDistance)
                            {
                                actionOnTarget = actionToCheck;

                                minimumDistance = calculatedDistance;
                            }

                            if(calculatedScore > maxScore)
                            {
                                possibleActions = new List<Ai_PlannedAction>();

                                maxScore = calculatedScore;
                            }
                        }
                    }
                }

                if (actionOnTarget != null)
                {
                    possibleActions.Add(actionOnTarget);
                }
            }
        }

        if(possibleActions.Count > 0)
        {
            toReturn = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
        }

        return toReturn;
    }

    private bool CanDoAction(CPN_Character caster, Ai_PlannedAction actionToCheck, AI_Consideration consideration, bool isForNextTurn) //TO DO : Prise en compte des tours prochains
    {
        List<CPN_Character> targets = actionToCheck.actionTarget.GetNodeComponent<CPN_Character>();

        CPN_Character target = null;
        if(targets.Count > 0)
        {
            target = targets[0];
        }

        SpellScriptable action = consideration.wantedAction;

        if (target != null)
        {
            switch (action.CastTargets)
            {
                case SpellTargets.Self:
                    if (caster != target)
                    {
                        return false;
                    }
                    break;
                case SpellTargets.Allies:
                    if (!RVN_BattleManager.AreCharacterAllies(caster, target) || caster == target)
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
            }
        }

        if(!isForNextTurn && !Grid.IsNodeVisible(actionToCheck.movementTarget, actionToCheck.actionTarget))
        {
            return false;
        }

        //Condition de la Considération de l'IA

        if(!isForNextTurn && !currentCharacterSpell.IsActionUsable(actionToCheck.movementTarget.worldPosition, actionToCheck.actionTarget.worldPosition, consideration.wantedAction))
        {
            return false;
        }

        return true;
    }

    private float CalculateActionScore(Ai_PlannedAction plannedAction, AI_Consideration consideration, Node casterCurrentNode)
    {
        float result = 0;
        float coef = 0;

        foreach(ValueForCalcul calculValue in consideration.calculs)
        {
            result += CalculateConsideration(plannedAction, calculValue, casterCurrentNode) * (calculValue.calculImportance + 1);
            coef += calculValue.calculImportance + 1;
        }

        return consideration.bonusScore + (result / coef);
    }

    private float CalculateConsideration(Ai_PlannedAction plannedAction, ValueForCalcul calculValues, Node casterCurrentNode)
    {
        float result = 0;
        float abscissa = GetAbscissaValue(calculValues.abscissaValue, plannedAction, casterCurrentNode);

        switch(calculValues.calculType)
        {
            case AiCalculType.Conditionnal:
                result = CalculConditionnal(abscissa, calculValues.constant, calculValues.coeficient);
                break;
            case AiCalculType.Affine:
                result = CalculAffine(abscissa, calculValues.constant, calculValues.coeficient, calculValues.maxValue);
                break;
            case AiCalculType.Logistical:
                result = CalculLogistical(abscissa, calculValues.constant, calculValues.coeficient, calculValues.maxValue, calculValues.checkAroundMax);
                break;
            case AiCalculType.Exponential:
                result = CalculExponential(abscissa, calculValues.constant, calculValues.coeficient, calculValues.maxValue);
                break;
            case AiCalculType.Logarythm:
                result = CalculLogarythm(abscissa, calculValues.constant, calculValues.coeficient, calculValues.maxValue);
                break;
            case AiCalculType.ReverseExponential:
                result = CalculExponentialReverse(abscissa, calculValues.constant, calculValues.coeficient);
                break;
        }

        return Mathf.Clamp(result, 0, 1);
    }

    private float GetAbscissaValue(AiAbscissaType abscissaWanted, Ai_PlannedAction actionToCheck, Node casterCurrentNode)
    {
        float toReturn = 0;

        CPN_Character caster = casterCurrentNode.GetNodeComponent<CPN_Character>()[0];

        CPN_Character target = actionToCheck.actionTarget.GetNodeComponent<CPN_Character>()[0];

        switch(abscissaWanted)
        {
            case AiAbscissaType.DistanceFromTarget_CalculatedPosition:
                toReturn = Pathfinding.GetDistance(actionToCheck.movementTarget, actionToCheck.actionTarget);
                break;
            case AiAbscissaType.DistranceFromTarget_BasePosition:
                toReturn = Pathfinding.GetDistance(casterCurrentNode, actionToCheck.actionTarget);
                break;
            case AiAbscissaType.CasterMaxHp:
                if(caster != null && caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterMaxHealth))
                {
                    toReturn = casterMaxHealth.MaxHealth;
                }
                break;
            case AiAbscissaType.CasterCurrentHp:
                if (caster != null && caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterCurrentHealth))
                {
                    toReturn = casterCurrentHealth.CurrentHealth;
                }
                break;
            case AiAbscissaType.CasterPercentHp:
                if (caster != null && caster.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler casterPercentHealth))
                {
                    toReturn = casterPercentHealth.CurrentHealth / casterPercentHealth.MaxHealth;
                }
                break;
            case AiAbscissaType.TargetMaxHp:
                if(target != null && target.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetMaxHealth))
                {
                    toReturn = targetMaxHealth.MaxHealth;
                }
                break;
            case AiAbscissaType.TargetCurrentHp:
                if (target != null && target.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetCurrentHealth))
                {
                    toReturn = targetCurrentHealth.CurrentHealth;
                }
                break;
            case AiAbscissaType.TargetPercentHp:
                if (target != null && target.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetPercentHealth))
                {
                    toReturn = targetPercentHealth.CurrentHealth / targetPercentHealth.MaxHealth;
                }
                break;
            case AiAbscissaType.TargetMaxArmor:
                if (target != null && target.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetMaxArmor))
                {
                    toReturn = targetMaxArmor.MaxArmor;
                }
                break;
            case AiAbscissaType.TargetCurrentArmor:
                if (target != null && target.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler targetCurrentArmor))
                {
                    toReturn = targetCurrentArmor.CurrentArmor;
                }
                break;
            case AiAbscissaType.TargetDangerosity:
                break;
            case AiAbscissaType.TargetVulnerability:
                break;
        }

        return toReturn;
    }


    #region Calculs de Considérations


    public float CalculConditionnal(float abscissa, float constant, float coeficient)
    {
        if (coeficient >= 0 && abscissa > constant)
        {
            return 1;
        }
        else if (coeficient < 0 && abscissa < constant)
        {
            return 1;
        }
        return 0;
    }

    public float CalculAffine(float abscissa, float constant, float coeficient, float maxValue)
    {
        return (constant + abscissa * coeficient)/maxValue;
    }

    public float CalculLogarythm(float abscissa, float constant, float coeficient, float maxValue)
    {
        return (constant + (Mathf.Pow(abscissa, coeficient))) / Mathf.Pow(maxValue, coeficient);
    }

    public float CalculExponential(float abscissa, float constant, float coeficient, float maxValue)
    {
        if (coeficient >= 1)
        {
            return Mathf.Exp(abscissa / coeficient + constant) / Mathf.Exp(maxValue / coeficient);
        }
        else
        {
            return 0;
        }
    }

    public float CalculExponentialReverse(float abscissa, float constant, float coeficient)
    {
        if (coeficient > 0)
        {
            return 1 / Mathf.Exp(abscissa / coeficient + constant);
        }
        else
        {
            return 0;
        }
    }

    public float CalculLogistical(float abscissa, float constant, float coeficient, float maxValue, bool checkAroundMax)
    {
        if (coeficient != 0)
        {
            if (checkAroundMax)
            {
                return 2 / (1 + (Mathf.Exp(Mathf.Abs(abscissa - constant) * coeficient)));
            }
            return 1 / (1 + (Mathf.Exp((abscissa - constant) * coeficient)));
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
