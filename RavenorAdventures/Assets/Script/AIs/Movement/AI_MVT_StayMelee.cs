using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_MVT_StayMelee : AI_MovementBehavior
{
    private const float MaxDistanceForMelee = 15;
    private const float OpportunityAttackMalus = 50;

    public override List<Node> GetBestMovementNodes(CPN_Character currentCharacter)
    {
        List<Node> toReturn = new List<Node>();
        Node casterNode = currentCharacter.CurrentNode;

        Node targetCharacterNode = RVN_AiBattleManager.Instance.GetClosestCharacter(casterNode, true).CurrentNode;

        if (Grid.IsNodeVisible(targetCharacterNode, casterNode, MaxDistanceForMelee))
        {
            toReturn.Add(casterNode);
        }
        else
        {
            CPN_HealthHandler currentCharacterHealth = null;
            if (currentCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler nHealth))
            {
                currentCharacterHealth = nHealth;
            }

            CPN_Movement currentCharacterMovement = null;
            if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement nMovement))
            {
                currentCharacterMovement = nMovement;
            }

            float maxScore = -1;
            PathType currentPathType = PathType.PathBlockByMovingObstacle;

            List<Node> possibleMovements = Pathfinding.CalculatePathfinding(casterNode, null, currentCharacterMovement.MovementLeft);

            foreach (Node n in possibleMovements)
            {
                float distanceMovementPosTargetPos = -1;

                targetCharacterNode = RVN_AiBattleManager.Instance.GetClosestCharacter(n, true).CurrentNode; //Prend le personnage le plus proche de la position

                List<Node> pathFromNToTarget = Pathfinding.CalculatePathfinding(n, targetCharacterNode, -1, true, true);

                if (pathFromNToTarget.Count > 0)
                {
                    distanceMovementPosTargetPos = Pathfinding.GetPathLength(n, pathFromNToTarget);

                    if (currentPathType != PathType.PathClear)
                    {
                        currentPathType = PathType.PathClear;
                        maxScore = -1;
                    }
                }
                else if (currentPathType != PathType.PathClear)
                {
                    pathFromNToTarget = Pathfinding.CalculatePathfinding(n, targetCharacterNode, -1, false, true);

                    if (pathFromNToTarget.Count > 0)
                    {
                        distanceMovementPosTargetPos = Pathfinding.GetPathLength(n, pathFromNToTarget);
                    }
                }

                //Pourcentage par rapport � la distance voulue
                if (distanceMovementPosTargetPos > MaxDistanceForMelee)
                {
                    distanceMovementPosTargetPos += Mathf.Abs(distanceMovementPosTargetPos - MaxDistanceForMelee);
                }
                else if (distanceMovementPosTargetPos >= 0)//La position est � la bonne distance voulue
                {
                    distanceMovementPosTargetPos = 0;
                }

                if (distanceMovementPosTargetPos >= 0)
                {
                    if (n != casterNode)
                    {
                        List<Node> pathFromCasterToN = Pathfinding.CalculatePathfinding(casterNode, n, -1, true, true);
                        pathFromCasterToN.Insert(0, casterNode);

                        if (RVN_AiBattleManager.Instance.OpportunityAttackScore(currentCharacter, currentCharacterHealth, pathFromCasterToN) > 0)
                        {
                            distanceMovementPosTargetPos += OpportunityAttackMalus;
                        }
                    }

                    if (maxScore < 0 || distanceMovementPosTargetPos < maxScore)
                    {
                        toReturn = new List<Node>();

                        toReturn.Add(n);

                        maxScore = distanceMovementPosTargetPos;
                    }
                    else if (distanceMovementPosTargetPos == maxScore)
                    {
                        toReturn.Add(n);
                    }
                }
            }
        }

        return toReturn;
    }
}
