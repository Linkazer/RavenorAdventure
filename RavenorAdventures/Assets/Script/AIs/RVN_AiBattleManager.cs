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

    private Ai_PlannedAction plannedAction;

    private bool isDoneMoving;

    /// <summary>
    /// Débute le tour d'un personnage IA. (Appelé par UnityEvent)
    /// </summary>
    /// <param name="toBeginTurn">Le personnage qui commence son tour.</param>
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

        isDoneMoving = false;

        SearchNextAction(2f);
    }

    /// <summary>
    /// Met fin au tour du personnage IA.
    /// </summary>
    /// <param name="toEndTurn">Le personnage dont le tour est finit.</param>
    public void EndCharacterTurn(CPN_Character toEndTurn)
    {
        currentCharacter = null;

        RVN_BattleManager.EndCharacterTurn();
    }

    private void SearchNextAction(float timeToWait)
    {
        plannedAction = SearchForBestAction(currentCharacter, false);

        PrepareNextAction(timeToWait);
    }

    /// <summary>
    /// Créer un délai avant de rechercher la prochain action de l'IA.
    /// </summary>
    /// <param name="timeToWait">Le temps à attendre avant la prochaine action.</param>
    private void PrepareNextAction(float timeToWait)
    {
        TimerManager.CreateGameTimer(timeToWait, DoNextMove);
    }

    /// <summary>
    /// Demande à l'IA de faire sa prochaine action.
    /// </summary>
    private void DoNextMove()
    {
        if (currentCharacterHealth.CurrentHealth <= 0)
        {
            EndCharacterTurn(currentCharacter);
        }
        else if (plannedAction != null && plannedAction.actionIndex >= 0)
        {
            if (plannedAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                currentCharacterMovement.AskToMoveTo(plannedAction.movementTarget.worldPosition, () => PrepareNextAction(1f));
            }
            else
            {
                currentCharacterSpell.SelectSpell(plannedAction.actionIndex, false);
                currentCharacterSpell.TryDoAction(plannedAction.actionTarget.worldPosition, () => SearchNextAction(1.5f));

                plannedAction = null;
            }
        }
        else if (currentCharacterMovement.CanMove)
        {
            Ai_PlannedAction nextTurnAction = SearchForBestAction(currentCharacter, true);

            if (!isDoneMoving && nextTurnAction != null && nextTurnAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                currentCharacterMovement.AskToMoveTo(nextTurnAction.movementTarget.worldPosition, () => PrepareNextAction(1f));

                isDoneMoving = true;
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

    /// <summary>
    /// Cherche la prochaine action que le caster doit effectuer.
    /// </summary>
    /// <param name="caster">Le personnage IA qui cherche à faire l'action.</param>
    /// <param name="forNextTurn">Si TRUE, le personnage cherche une action pour son prochain tour.</param>
    /// <returns>L'action que le personnage IA doit faire.</returns>
    private Ai_PlannedAction SearchForBestAction(CPN_Character caster, bool forNextTurn)
    {
        Ai_PlannedAction toReturn = null;

        AI_CharacterScriptable currentAi = caster.Scriptable as AI_CharacterScriptable;

        List<CPN_Character> possibleTargets = new List<CPN_Character>();

        Node casterNode = currentCharacterMovement.CurrentNode;

        List<Ai_PlannedAction> possibleActions = new List<Ai_PlannedAction>();

        float maxScore = -1;

        foreach (AI_Consideration consideration in currentAi.Comportement)
        {
            possibleTargets = new List<CPN_Character>();

            switch(consideration.wantedAction.CastTargets)
            {
                case SpellTargets.Self:
                    possibleTargets.Add(caster);
                    break;
                case SpellTargets.Allies:
                    possibleTargets = RVN_BattleManager.GetAllyCharacters(caster);
                    break;
                case SpellTargets.Ennemies:
                    possibleTargets = RVN_BattleManager.GetEnnemyCharacters(caster);
                    break;
                case SpellTargets.All:
                    possibleTargets = RVN_BattleManager.GetAllCharacter();
                    break;
            }

            if(!currentCharacterSpell.Spells[consideration.wantedActionIndex].IsUsable)
            {
                continue;
            }

            List<Node> possibleMovements = new List<Node>();
            if (forNextTurn)
            {
                if (possibleTargets.Contains(caster))
                {
                    possibleMovements.Add(casterNode);
                }

                foreach (CPN_Character target in possibleTargets)
                {
                    List<Node> targetNaighbours = Grid.GetNeighbours(target.CurrentNode);

                    Node toAdd = null;

                    int dist = 1023;

                    foreach(Node n in targetNaighbours)
                    {
                        if(n.IsWalkable && Pathfinding.GetDistance(n, casterNode) < dist)
                        {
                            toAdd = n;
                            dist = Pathfinding.GetDistance(n, casterNode);
                        }
                    }

                    if(toAdd != null)
                    {
                        possibleMovements.Add(toAdd);
                    }
                }
            }
            else
            {
                possibleMovements = Pathfinding.CalculatePathfinding(casterNode, null, currentCharacterMovement.MovementLeft);
            }

            foreach (CPN_Character target in possibleTargets)
            {
                List<Node> possibleTargetPosition = Pathfinding.GetAllNodeInDistance(target.CurrentNode, consideration.wantedAction.Range, true);

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

                        if (calculatedScore > maxScore)
                        {
                            Debug.Log(n.worldPosition + " : " + calculatedScore);
                            Debug.Log(target.gameObject);

                            actionOnTarget = actionToCheck;

                            minimumDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, casterNode);

                            possibleActions = new List<Ai_PlannedAction>();

                            maxScore = calculatedScore;
                        }
                        else if (calculatedScore == maxScore)
                        {
                            float calculatedDistance = -1;

                            calculatedDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, casterNode);
                            
                            Debug.Log(n.worldPosition + " = " + calculatedScore);
                            Debug.Log(target.gameObject);
                            Debug.Log(calculatedDistance);

                            if (minimumDistance < 0 || calculatedDistance < minimumDistance)
                            {
                                actionOnTarget = actionToCheck;

                                minimumDistance = calculatedDistance;
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

        if (possibleActions.Count > 0)
        {
            toReturn = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
        }

        return toReturn;
    }

    /// <summary>
    /// Vérifie si le personnage IA peut faire l'action voulue.
    /// </summary>
    /// <param name="caster">Le personnage IA.</param>
    /// <param name="actionToCheck">L'action à vérifier.</param>
    /// <param name="consideration">La considération à prendre en compte pour cette action.</param>
    /// <param name="isForNextTurn">Si TRUE, vérifie le lancement de l'action pour le tour prochain.</param>
    /// <returns>Renvoie TRUE si l'action peut être effectuée. Sinon, renvoie FALSE.</returns>
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

        if (!CheckConsiderationCondition(caster, actionToCheck, consideration))
        {
            return false;
        }

        if (!isForNextTurn)
        {
            if (!Grid.IsNodeVisible(actionToCheck.movementTarget, actionToCheck.actionTarget))
            {
                return false;
            }

            if (!currentCharacterSpell.IsActionUsable(actionToCheck.movementTarget.worldPosition, actionToCheck.actionTarget.worldPosition, consideration.wantedAction))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckConsiderationCondition(CPN_Character caster, Ai_PlannedAction actionToCheck, AI_Consideration consideration)
    {
        float abscissa = 0;

        foreach (AIC_Conditional condition in consideration.conditions)
        {
            abscissa = GetAbscissaValue(condition.abscissaValue, actionToCheck, caster.CurrentNode);

            if(condition.Calculate(abscissa) <= 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calcul le score d'une action pour le tour actuel.
    /// </summary>
    /// <param name="plannedAction">L'action à effectuer.</param>
    /// <param name="consideration">La considération à calculer.</param>
    /// <param name="casterCurrentNode">La case sur laquelle se trouve le personnage IA.</param>
    /// <returns>Le score de l'action.</returns>
    private float CalculateActionScore(Ai_PlannedAction plannedAction, AI_Consideration consideration, Node casterCurrentNode)
    {
        float result = 0;
        float coef = 0;

        foreach(AI_Calcul calculValue in consideration.calculs)
        {
            result += CalculateConsideration(plannedAction, calculValue, casterCurrentNode) * (calculValue.calculImportance + 1);
            coef += calculValue.calculImportance + 1;
        }

        if(coef == 0)
        {
            coef = 1;
        }

        return consideration.bonusScore + (result / coef);
    }

    /// <summary>
    /// Calcul la considération d'une action
    /// </summary>
    /// <param name="plannedAction">L'action à vérifier.</param>
    /// <param name="calcul">Les calculs à effectuer.</param>
    /// <param name="casterCurrentNode">La case sur laquelle se trouve le personnage IA.</param>
    /// <returns>Le score de la considération.</returns>
    private float CalculateConsideration(Ai_PlannedAction plannedAction, AI_Calcul calcul, Node casterCurrentNode)
    {
        float abscissa = GetAbscissaValue(calcul.abscissaValue, plannedAction, casterCurrentNode);

        return Mathf.Clamp(calcul.Calculate(abscissa), 0, 1);
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


