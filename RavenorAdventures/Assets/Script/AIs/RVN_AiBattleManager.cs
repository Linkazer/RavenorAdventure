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
        public float minimalDistance;
    }

    [SerializeField] private float timeBetweenActions = 0.5f;
    [SerializeField] private float timeDelayBeginTurn = 1f;
    [SerializeField] private float timeDelayEndTurn = 0.5f;

    [Header("Calcul Values")]
    [SerializeField] private float opportunityDefenseCoef = 0.2f;

    [Header("Debugs")]
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

        Debug.Log("New Turn : " +  currentCharacter.gameObject);

        SearchNextAction(timeDelayBeginTurn);
    }

    /// <summary>
    /// Met fin au tour du personnage IA.
    /// </summary>
    /// <param name="toEndTurn">Le personnage dont le tour est finit.</param>
    public void EndCharacterTurn(CPN_Character toEndTurn)
    {
        currentCharacter = null;

        TimerManager.CreateGameTimer(timeDelayEndTurn, () => RVN_BattleManager.EndCharacterTurn());
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
            return;// EndCharacterTurn(currentCharacter);
        }
        else if (plannedAction != null && plannedAction.actionIndex >= 0)
        {
            if (plannedAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                currentCharacterMovement.AskToMoveTo(plannedAction.movementTarget.worldPosition, () => PrepareNextAction(timeBetweenActions));
            }
            else
            {
                currentCharacterSpell.SelectSpell(plannedAction.actionIndex, false);
                currentCharacterSpell.TryDoAction(plannedAction.actionTarget.worldPosition, () => SearchNextAction(timeBetweenActions));

                plannedAction = null;
            }
        }
        else if (currentCharacterMovement.CanMove)
        {
            Ai_PlannedAction nextTurnAction = SearchForBestAction(currentCharacter, true);

            if (!isDoneMoving)
            {
                if (nextTurnAction != null && nextTurnAction.movementTarget != currentCharacterMovement.CurrentNode)
                {
                    currentCharacterMovement.AskToMoveTo(nextTurnAction.movementTarget.worldPosition, () => PrepareNextAction(timeBetweenActions));
                }
                else
                {
                    currentCharacterMovement.AskToMoveTo(SearchForBestMovement().worldPosition, () => PrepareNextAction(timeBetweenActions));
                }

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

        foreach (AI_Consideration consideration in currentAi.Comportement) //Check de chacune des considérations de l'IA
        {
            possibleTargets = new List<CPN_Character>();

            switch(consideration.wantedAction.CastTargets) //Récupération des targets possibles
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

            //Recherche des mouvements possibles
            List<Node> possibleMovements = GetPossibleMovements(casterNode, possibleTargets, forNextTurn);

            float opportunityAttackScore = OpportunityAttackScore(currentCharacterHealth, casterNode); // On fait le calcul ici puisque le résultat de changera pas pendant la recherche de l'attaque

            foreach (CPN_Character target in possibleTargets)
            {
                //List<Node> possibleTargetPosition = Pathfinding.GetAllNodeInDistance(target.CurrentNode, consideration.wantedAction.Range, true); Useless pour l'instant, à voir si on réutilisé

                Ai_PlannedAction actionOnTarget = null;

                float minimumDistance = 99999;

                foreach (Node n in possibleMovements)
                {
                    Ai_PlannedAction actionToCheck = new Ai_PlannedAction();
                    actionToCheck.actionTarget = target.CurrentNode;
                    actionToCheck.movementTarget = n;
                    actionToCheck.actionIndex = consideration.wantedActionIndex;

                    if (CanDoAction(caster, actionToCheck, consideration, forNextTurn))
                    {
                        float calculatedScore = CalculateActionScore(actionToCheck, consideration, casterNode);

                        //Debug.Log($"AVANT : {currentCharacter} on {target} : {calculatedScore}");

                        if(n != casterNode)
                        {
                            calculatedScore -= opportunityAttackScore;
                        }

                        //Debug.Log($"APRES : {currentCharacter} on {target} : {calculatedScore}");

                        if(forNextTurn)
                        {
                            actionToCheck.minimalDistance = Mathf.Abs(Pathfinding.GetDistance(actionToCheck.movementTarget, target.CurrentNode) - consideration.wantedAction.Range);
                        }
                        else
                        {
                            if (n != casterNode)
                            {
                                actionToCheck.minimalDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, casterNode);
                            }
                            else
                            {
                                actionToCheck.minimalDistance = -1f;
                            }
                        }

                        //if (!forNextTurn || (n != casterNode || Pathfinding.GetDistance(casterNode, target.CurrentNode) <= 15))
                        {
                            if (calculatedScore > maxScore)
                            {
                                possibleActions = new List<Ai_PlannedAction>();

                                maxScore = calculatedScore;
                            }

                            if (calculatedScore == maxScore)
                            {
                                if (actionToCheck.minimalDistance <= minimumDistance)
                                {
                                    minimumDistance = actionToCheck.minimalDistance;

                                    actionOnTarget = actionToCheck;
                                }
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
            List<Ai_PlannedAction> closestActions = new List<Ai_PlannedAction>();

            float minimalDistance = possibleActions[0].minimalDistance;

            foreach (Ai_PlannedAction pa in possibleActions)
            {
                if (pa.minimalDistance < minimalDistance)
                {
                    closestActions = new List<Ai_PlannedAction>();

                    minimalDistance = pa.minimalDistance;
                }

                if(minimalDistance == pa.minimalDistance)
                {
                    closestActions.Add(pa);
                }
            }

            toReturn = closestActions[UnityEngine.Random.Range(0, closestActions.Count)];
        }

        return toReturn;
    }

    private Node SearchForBestMovement()
    {
        Debug.Log(currentCharacterMovement);

        Node casterNode = currentCharacterMovement.CurrentNode;
        AI_CharacterScriptable casterScriptable = (currentCharacter.Scriptable as AI_CharacterScriptable);

        float maxScore = 9999;

        List<Node> possibleMovements = Pathfinding.CalculatePathfinding(casterNode, null, currentCharacterMovement.MovementLeft);

        List<Node> possibleTargetNodes = new List<Node>();

        Node toReturn = null;

        foreach(Node n in possibleMovements)
        {
            float distance = Pathfinding.GetDistance(n, GetClosestCharacter(n, true).CurrentNode);

            float opportunityAttackScore = 0;

            List<Node> path = Pathfinding.CalculatePathfinding(casterNode, n, currentCharacterMovement.MovementLeft);

            opportunityAttackScore += OpportunityAttackScore(currentCharacterHealth, path);

            if (distance > casterScriptable.DistanceFromTarget.y)
            {
                distance = Mathf.Abs(distance - casterScriptable.DistanceFromTarget.y) / casterScriptable.DistanceFromTarget.y;
            }
            else if (distance < casterScriptable.DistanceFromTarget.x)
            {
                distance = Mathf.Abs(distance - casterScriptable.DistanceFromTarget.x) / casterScriptable.DistanceFromTarget.x;
            }
            else
            {
                distance = 0;
            }

            if(n != casterNode)
            {
                distance += opportunityAttackScore;
            }

            if(n == casterNode)
            {
                Debug.Log($"Distance with cster node : {distance}");
            }

            Debug.Log($"Distance : {distance} < {maxScore}");

            if (distance < maxScore)
            {
                possibleTargetNodes = new List<Node>();

                maxScore = distance;

                toReturn = n;
            }
            else if(distance == maxScore && Pathfinding.GetDistance(casterNode, toReturn) < Pathfinding.GetDistance(casterNode, n))
            {
                Debug.Log($"Equal distance for {GetClosestCharacter(n, true)} : {Pathfinding.GetDistance(casterNode, toReturn)} < {Pathfinding.GetDistance(casterNode, GetClosestCharacter(n, true).CurrentNode)}");

                toReturn = n;

                possibleTargetNodes.Add(n);
            }
        }

        return toReturn;
    }

    private float OpportunityAttackScore(CPN_HealthHandler casterHealthHandler, Node casterNode)
    {
        float opportunityAttackDiceCounter = 0;

        List<Node> casterNeighbours = Grid.GetNeighbours(casterNode);

        foreach(Node n in casterNeighbours)
        {
            List<CPN_SpellCaster> casters = n.GetNodeComponent<CPN_SpellCaster>();
            foreach(CPN_SpellCaster c in casters)
            {
                if(c.hasOpportunityAttack && c.OpportunitySpell is RVN_SS_DamageSpellScriptable)
                {
                    opportunityAttackDiceCounter += (c.OpportunitySpell as RVN_SS_DamageSpellScriptable).DiceUsed;
                }
            }
        }

        opportunityAttackDiceCounter = ((opportunityAttackDiceCounter - casterHealthHandler.CurrentArmor) * (1f - ((casterHealthHandler.Defense / 6f) * opportunityDefenseCoef)));

        return opportunityAttackDiceCounter / casterHealthHandler.CurrentHealth;
    }

    private float OpportunityAttackScore(CPN_HealthHandler casterHealthHandler, List<Node> pathNode)
    {
        float opportunityAttackDiceCounter = 0;

        List<CPN_SpellCaster> alreadyAttackedCaster = new List<CPN_SpellCaster>();

        foreach (Node casterNode in pathNode)
        {

            List<Node> casterNeighbours = Grid.GetNeighbours(casterNode);

            foreach (Node n in casterNeighbours)
            {
                List<CPN_SpellCaster> casters = n.GetNodeComponent<CPN_SpellCaster>();
                foreach (CPN_SpellCaster c in casters)
                {
                    if (!alreadyAttackedCaster.Contains(c) && c.hasOpportunityAttack && c.OpportunitySpell is RVN_SS_DamageSpellScriptable)
                    {
                        opportunityAttackDiceCounter += (c.OpportunitySpell as RVN_SS_DamageSpellScriptable).DiceUsed;

                        alreadyAttackedCaster.Add(c);
                    }
                }
            }
        }
        opportunityAttackDiceCounter = ((opportunityAttackDiceCounter - casterHealthHandler.CurrentArmor) * (1f - ((casterHealthHandler.Defense / 6f) * opportunityDefenseCoef)));

        return opportunityAttackDiceCounter / casterHealthHandler.CurrentHealth;
    }

    private List<Node> GetPossibleMovements(Node startNode, List<CPN_Character> possibleTargets, bool forNextTurn)
    {
        List<Node> possibleMovements = Pathfinding.CalculatePathfinding(startNode, null, currentCharacterMovement.MovementLeft);
        if (forNextTurn)
        {
            possibleMovements.Add(startNode); //POTENTIAL BUG : S'il y a un problème de comportement d'IA lors de la préparation du tour suivant, ça peut être ici

            foreach (CPN_Character target in possibleTargets)
            {
                List<Node> targetNeighbours = Grid.GetNeighbours(target.CurrentNode);

                Node toAdd = null;

                int dist = 99999;

                foreach (Node n in targetNeighbours)
                {
                    if (n.IsWalkable && Pathfinding.GetDistance(n, startNode) < dist)
                    {
                        toAdd = n;
                        dist = Pathfinding.GetDistance(n, startNode);
                    }
                }

                if (toAdd != null && !possibleMovements.Contains(toAdd))
                {
                    possibleMovements.Add(toAdd);
                }
            }
        }
        return possibleMovements;
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

    private CPN_Character GetClosestCharacter(Node nodeToCheck, bool isEnemy)
    {
        CPN_Character toReturn = null;

        List<CPN_Character> targets = new List<CPN_Character>();

        if (!isEnemy)
        {
            targets = RVN_BattleManager.GetAllyCharacters(currentCharacter);
        }
        else
        {
            targets = RVN_BattleManager.GetEnnemyCharacters(currentCharacter);
        }

        if (targets.Count > 0)
        {
            int minDistance = Pathfinding.GetDistance(targets[0].CurrentNode, nodeToCheck);

            toReturn = targets[0];

            for(int i = 1; i < targets.Count; i++)
            {
                if(Pathfinding.GetDistance(targets[i].CurrentNode, nodeToCheck) < minDistance)
                {
                    minDistance = Pathfinding.GetDistance(targets[i].CurrentNode, nodeToCheck);

                    toReturn = targets[i];
                }
            }
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


