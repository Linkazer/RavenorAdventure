using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Ai_PlannedAction
{
    public int actionIndex;
    public CPN_Character caster;
    public Node movementTarget;
    public Node spellNodeTarget;
    public CPN_Character actualTarget;
    public List<Node> hitedTargets = new List<Node>();
    public float minimalDistance;
    public float score;
}

public class RVN_AiBattleManager : RVN_Singleton<RVN_AiBattleManager>
{
    [SerializeField] private float timeBetweenActions = 0.5f;
    [SerializeField] private float timeDelayBeginTurn = 1f;
    [SerializeField] private float timeDelayEndTurn = 0.5f;
    [SerializeField] private int checksByFrame = 50;

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

        SearchNextAction(timeDelayBeginTurn);
    }


    public void Pause()
    {
        enabled = false;
    }

    public void Restart()
    {
        enabled = true;
    }

    /// <summary>
    /// Met fin au tour du personnage IA.
    /// </summary>
    /// <param name="toEndTurn">Le personnage dont le tour est finit.</param>
    public void EndCharacterTurn(CPN_Character toEndTurn)
    {
        currentCharacter = null;

        TimerManager.CreateGameTimer(timeDelayEndTurn, RVN_BattleManager.EndCharacterTurn);
    }

    private void SearchNextAction(float timeToWait)
    {
        //Debug.Log("Search for next action");

        StartCoroutine(SearchForBestAction(currentCharacter, false, FindNextAction));
    }

    private void FindNextAction(Ai_PlannedAction findedAction)
    {
        plannedAction = findedAction;

        PrepareNextAction(1f);
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
        if (currentCharacterHealth.CurrentHealth <= 0 || enabled == false)
        {
            //Debug.Log("No turn needed");
            return;
        }
        else if (plannedAction != null && plannedAction.actionIndex >= 0)
        {
            if (plannedAction.movementTarget != currentCharacterMovement.CurrentNode)
            {
                //Debug.Log($"Movement Action toward : {plannedAction.movementTarget.worldPosition}");
                currentCharacterMovement.AskToMoveTo(plannedAction.movementTarget.worldPosition, () => PrepareNextAction(timeBetweenActions));
            }
            else
            {
                //Debug.Log("Spell Action");
                currentCharacterSpell.SelectSpell(plannedAction.actionIndex, false);
                currentCharacterSpell.TryDoAction(plannedAction.spellNodeTarget.worldPosition, () => SearchNextAction(timeBetweenActions));

                plannedAction = null;
            }
        }
        else if (currentCharacterMovement.CanMove)
        {
            if (!isDoneMoving)
            {
                Node destination = SearchForBestMovement();
                //Debug.Log($"Movement Action Second toward : {destination.worldPosition}");

                currentCharacterMovement.AskToMoveTo(destination.worldPosition, () => PrepareNextAction(timeBetweenActions));
                isDoneMoving = true;
            }
            else
            {
                //Debug.Log("Movement End");

                EndCharacterTurn(currentCharacter);
            }
        }
        else
        {
            //Debug.Log("No Action End");
            EndCharacterTurn(currentCharacter);
        }
    }

    /// <summary>
    /// Cherche la prochaine action que le caster doit effectuer.
    /// </summary>
    /// <param name="caster">Le personnage IA qui cherche à faire l'action.</param>
    /// <param name="forNextTurn">Si TRUE, le personnage cherche une action pour son prochain tour.</param>
    /// <returns>L'action que le personnage IA doit faire.</returns>
    private IEnumerator SearchForBestAction(CPN_Character caster, bool forNextTurn, Action<Ai_PlannedAction> callback)
    {
        Ai_PlannedAction toReturn = null;

        AI_CharacterScriptable currentAi = caster.Scriptable as AI_CharacterScriptable;

        List<CPN_Character> possibleTargets = new List<CPN_Character>();

        Node casterNode = currentCharacterMovement.CurrentNode;

        List<Ai_PlannedAction> possibleActions = new List<Ai_PlannedAction>();

        float maxScore = 0;

        int currentCheckDone = 0;

        foreach (AI_Consideration consideration in currentAi.Comportement) //Check de chacune des considérations de l'IA
        {
            possibleTargets = new List<CPN_Character>();

            switch(consideration.wantedAction.CastTarget) //Récupération des targets possibles
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

            if(!currentCharacterSpell.Spells[consideration.wantedActionIndex].IsUsable())
            {
                continue;
            }

            //Recherche des mouvements possibles
            List<Node> possibleMovements = GetPossibleMovements(casterNode, possibleTargets, forNextTurn);

            if(!currentCharacterMovement.CanMove)
            {
                possibleMovements = new List<Node>();
                possibleMovements.Add(casterNode);
            }

            float opportunityAttackScore = OpportunityAttackScore(currentCharacter, currentCharacterMovement.currentEvasiveAmount, currentCharacterHealth); // On fait le calcul ici puisque le résultat de changera pas pendant la recherche de l'attaque

            foreach (CPN_Character target in possibleTargets)
            {
                Ai_PlannedAction actionOnTarget = null;

                float minimumDistance = 99999;

                foreach (Node movementNode in possibleMovements)
                {
                    List<Node> spellUsableNodes = GetReachTargetNode(movementNode, target.CurrentNode, consideration.wantedAction);

                    foreach (Node spellNode in spellUsableNodes)
                    {
                        Ai_PlannedAction actionToCheck = new Ai_PlannedAction();
                        actionToCheck.caster = caster;
                        actionToCheck.spellNodeTarget = spellNode;
                        actionToCheck.actualTarget = target;
                        actionToCheck.movementTarget = movementNode;
                        actionToCheck.actionIndex = consideration.wantedActionIndex;
                        actionToCheck.hitedTargets = consideration.wantedAction.GetDisplayzone(movementNode, spellNode);

                        if (CanDoAction(caster, actionToCheck, consideration, forNextTurn))
                        {
                            float calculatedScore = CalculateActionScore(actionToCheck, consideration);

                            //Debug.Log($"AVANT : {currentCharacter} on {target} : {calculatedScore}");

                            if (movementNode != casterNode)
                            {
                                calculatedScore -= opportunityAttackScore;
                            }

                            //Debug.Log($"APRES : {currentCharacter} on {target} : {calculatedScore}");

                            if (forNextTurn)
                            {
                                actionToCheck.minimalDistance = Mathf.Abs(Pathfinding.GetDistance(actionToCheck.movementTarget, spellNode) - consideration.wantedAction.Range);
                            }
                            else
                            {
                                if (movementNode != casterNode)
                                {
                                    actionToCheck.minimalDistance = Pathfinding.GetDistance(actionToCheck.movementTarget, casterNode);
                                }
                                else
                                {
                                    actionToCheck.minimalDistance = -1f;
                                }
                            }

                            actionToCheck.score = calculatedScore;

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

                        currentCheckDone++;

                        if (currentCheckDone % checksByFrame == 0)
                        {
                            yield return new WaitForSeconds(Time.deltaTime);
                        }
                    }
                }

                if(actionOnTarget != null)
                {
                    possibleActions.Add(actionOnTarget);
                }
            }
        }

        if (possibleActions.Count > 0)
        {
            toReturn = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
        }

        yield return new WaitForSeconds(Time.deltaTime);

        callback?.Invoke(toReturn);
    }

    private Node SearchForBestMovement()
    {
        List<Node> possibleTargetNodes = (currentCharacter.Scriptable as AI_CharacterScriptable).MovementBehaviorUsed.GetBestMovementNodes(currentCharacter);

        Node toReturn = null;

        if (possibleTargetNodes.Count > 0 && !possibleTargetNodes.Contains(currentCharacter.CurrentNode))
        {
            toReturn = possibleTargetNodes[UnityEngine.Random.Range(0, possibleTargetNodes.Count)];
        }
        else
        {
            toReturn = currentCharacter.CurrentNode;
        }

        return toReturn;
    }

    /// <summary>
    /// Return 1 if the character can die. Return 0 if the character won't take any attack.
    /// </summary>
    /// <param name="characterToCheck"></param>
    /// <param name="evasiveAmount"></param>
    /// <param name="casterHealthHandler"></param>
    /// <returns></returns>
    public float OpportunityAttackScore(CPN_Character characterToCheck, int evasiveAmount, CPN_HealthHandler casterHealthHandler)
    {
        if(evasiveAmount > 0)
        {
            return 0;
        }

        float opportunityAttackDiceCounter = 0;

        List<Node> casterNeighbours = Grid.GetNeighbours(characterToCheck.CurrentNode);

        foreach(Node n in casterNeighbours)
        {
            List<CPN_SpellCaster> casters = n.GetNodeComponent<CPN_SpellCaster>();
            foreach(CPN_SpellCaster c in casters)
            {
                if (!RVN_BattleManager.AreCharacterAllies(casterHealthHandler.Handler as CPN_Character, c.Handler as CPN_Character))
                {
                    /*if (c.hasOpportunityAttack && c.OpportunitySpell.SpellData is SPL_AS_DamageSpell)
                    {
                        opportunityAttackDiceCounter += (c.OpportunitySpell.SpellData as SPL_AS_DamageSpell).DiceUsed; //TODO Spell Rework : A revoir
                    }*/
                }
            }
        }

        float toReturn = CalculateOpportunityScore(opportunityAttackDiceCounter, casterHealthHandler.CurrentArmor, casterHealthHandler.CurrentHealth, (characterToCheck.Scriptable as AI_CharacterScriptable).OppportunityHealthBonus); ;

        return toReturn;
    }

    public float OpportunityAttackScore(CPN_Character characterToCheck, CPN_HealthHandler casterHealthHandler, List<Node> pathNode)
    {
        float opportunityAttackDiceCounter = 0;

        List<CPN_SpellCaster> alreadyAttackedCaster = new List<CPN_SpellCaster>();

        for(int i = 0; i < pathNode.Count-1; i++)
        {
            List<Node> casterNeighbours = Grid.GetNeighbours(pathNode[i]);

            foreach (Node n in casterNeighbours)
            {
                List<CPN_SpellCaster> casters = n.GetNodeComponent<CPN_SpellCaster>();
                foreach (CPN_SpellCaster c in casters)
                {
                    if (!alreadyAttackedCaster.Contains(c) && !RVN_BattleManager.AreCharacterAllies(c.Handler as CPN_Character, characterToCheck) && c.hasOpportunityAttack && c.OpportunitySpell is RVN_SS_DamageSpellScriptable)
                    {
                        //opportunityAttackDiceCounter += (c.OpportunitySpell as RVN_SS_DamageSpellScriptable).DiceUsed; //TODO Spell Rework : A revoir

                        alreadyAttackedCaster.Add(c);
                    }
                }
            }
        }

        return CalculateOpportunityScore(opportunityAttackDiceCounter, casterHealthHandler.CurrentArmor, casterHealthHandler.CurrentHealth, (characterToCheck.Scriptable as AI_CharacterScriptable).OppportunityHealthBonus);
    }

    private float CalculateOpportunityScore(float damage, int armor, float health, int bonus)
    {
        return (Mathf.Clamp((damage - armor), 0, Mathf.Infinity)) / (health + bonus);
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

    private List<Node> GetReachTargetNode(Node casterNode, Node targetNode, SPL_SpellScriptable spellToCheck)
    {
        List<Node> toReturn = new List<Node>();

        foreach (Node n in spellToCheck.GetDisplayzone(casterNode, targetNode))
        {
            if (Pathfinding.GetAllNodeInDistance(casterNode, spellToCheck.Range, true).Contains(n))
            {
                toReturn.Add(n);
            }
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
        CPN_Character target = actionToCheck.actualTarget;

        SPL_SpellScriptable action = consideration.wantedAction;

        if (target != null)
        {
            switch (action.CastTarget)
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

        if (!CheckConsiderationCondition(actionToCheck, consideration))
        {
            return false;
        }

        if (!isForNextTurn)
        {
            if (!Grid.IsNodeVisible(actionToCheck.movementTarget, actionToCheck.spellNodeTarget))
            {
                return false;
            }

            if (!currentCharacterSpell.IsActionUsable(actionToCheck.movementTarget.worldPosition, actionToCheck.spellNodeTarget.worldPosition, consideration.wantedAction))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckConsiderationCondition(Ai_PlannedAction actionToCheck, AI_Consideration consideration)
    {
        float conditionResult = 0;

        foreach (AIC_Conditional condition in consideration.conditions)
        {
            conditionResult = condition.Calculate(actionToCheck);

            if(conditionResult <= 0)
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
    /// <returns>Le score de l'action.</returns>
    private float CalculateActionScore(Ai_PlannedAction plannedAction, AI_Consideration consideration)
    {
        float result = 0;
        float coef = 0;

        foreach(AI_Calcul calculValue in consideration.calculs)
        {
            result += CalculateConsideration(plannedAction, calculValue) * (calculValue.calculImportance + 1);
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
    /// <returns>Le score de la considération.</returns>
    private float CalculateConsideration(Ai_PlannedAction plannedAction, AI_Calcul calcul)
    {
        float calculResult = calcul.Calculate(plannedAction); ;

        return Mathf.Clamp(calculResult, 0, 1);
    }

    public static float CalculateDangerosityVulnerability(CPN_Character character, bool isDangerosity)
    {
        float toReturn = 0;
        float distanceScore = 0;
        float distance = 0;

        foreach (CPN_Character chara in RVN_BattleManager.GetEnnemyCharacters(character))
        {
            if (Grid.IsNodeVisible(character.CurrentNode, chara.CurrentNode))
            {
                distance = Pathfinding.GetDistance(character.CurrentNode, chara.CurrentNode);

                if (isDangerosity && (character.Scriptable.DangerosityMinimumDistance > 0 || character.Scriptable.DangerosityLerpDistance > 0))
                {
                    if (distance <= character.Scriptable.DangerosityMinimumDistance)
                    {
                        distanceScore = 1;
                    }
                    else if (character.Scriptable.DangerosityLerpDistance > 0)
                    {
                        distance -= character.Scriptable.DangerosityMinimumDistance;

                        distanceScore = (character.Scriptable.DangerosityLerpDistance - distance) / character.Scriptable.DangerosityLerpDistance;

                        if (distanceScore < 0)
                        {
                            distanceScore = 0;
                        }
                    }

                }
                else if(chara.Scriptable.DangerosityMinimumDistance > 0 || chara.Scriptable.DangerosityLerpDistance > 0)
                {
                    if (distance <= chara.Scriptable.DangerosityMinimumDistance)
                    {
                        distanceScore = 1;
                    }
                    else if(chara.Scriptable.DangerosityLerpDistance > 0)
                    {
                        distance -= chara.Scriptable.DangerosityMinimumDistance;

                        distanceScore = (chara.Scriptable.DangerosityLerpDistance - distance) / chara.Scriptable.DangerosityLerpDistance;

                        if (distanceScore < 0)
                        {
                            distanceScore = 0;
                        }
                    }
                }

                toReturn += distanceScore;
            }
        }

        return toReturn;
    }

    public CPN_Character GetClosestCharacter(Node nodeToCheck, bool isEnemy)
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


