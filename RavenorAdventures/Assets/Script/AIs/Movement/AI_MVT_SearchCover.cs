using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class AI_MVT_SearchCover : AI_MovementBehavior
{
    private enum VisibilityType
    {
        Cover,
        Visible,
        Lost,
    }

    private const float IsVisibleScoreMalus = 30f;
    private const float NotCoverScoreMalus = 15f;
    private const float OpportunityAttackScoreMalus = 150f;
    private const float NoClearPathScoreMalus = 50f;
    private const float NoPathScoreMalus = 1000f;
    private const float TooCloseMultipler = 5f;
    private const float DistanceViewOffset = 25f;

    [SerializeField] private Vector2 distanceFromTargetWanted = new Vector2(40, 50);

    private float VisibleDistance => distanceFromTargetWanted.y + DistanceViewOffset;

    public override List<Node> GetBestMovementNodes(CPN_Character currentCharacter)
    {
        CPN_HealthHandler currentCharacterHealth = null;
        CPN_Movement currentCharacterMovement = null;
        if (currentCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler nHealth))
        {
            currentCharacterHealth = nHealth;
        }

        if (currentCharacter.GetComponentOfType<CPN_Movement>(out CPN_Movement nMovement))
        {
            currentCharacterMovement = nMovement;
        }

        Node casterNode = currentCharacterMovement.CurrentNode;

        float maxScore = -1;
        float nodeScore = 0;
        float characterScore = -1;

        List<Node> possibleMovements = Pathfinding.CalculatePathfinding(casterNode, null, currentCharacterMovement.MovementLeft);

        List<Node> toReturn = new List<Node>();

        List<CPN_Character> charactersToCheck = RVN_BattleManager.GetEnnemyCharacters(currentCharacter);

        Dictionary<VisibilityType, List<CPN_Character>> characterByVisibility = new Dictionary<VisibilityType, List<CPN_Character>>();

        Dictionary<CPN_Character, List<Node>> pathByCharacterClear = new Dictionary<CPN_Character, List<Node>>();
        Dictionary<CPN_Character, List<Node>> pathByCharacterBlocked = new Dictionary<CPN_Character, List<Node>>();

        foreach (CPN_Character chara in charactersToCheck)
        {
            pathByCharacterClear.Add(chara, Pathfinding.CalculatePathfinding(casterNode, chara.CurrentNode, -1, true, true));
            pathByCharacterBlocked.Add(chara, Pathfinding.CalculatePathfinding(casterNode, chara.CurrentNode, -1, false, true));
        }

        foreach (Node n in possibleMovements)
        {
            characterByVisibility = new Dictionary<VisibilityType, List<CPN_Character>>();
            nodeScore = 0;

            //Debug.Log(n.worldPosition);

            foreach (CPN_Character chara in charactersToCheck)
            {
                if (!Grid.IsNodeVisible(n, chara.CurrentNode, VisibleDistance))
                {
                    bool neighbourFound = false;
                    foreach (Node nNeighbour in Grid.GetNeighbours(n))
                    {
                        if ((nNeighbour == casterNode || WillNodeBeWalkableNextTurn(n, nNeighbour)) && Grid.IsNodeVisible(nNeighbour, chara.CurrentNode, VisibleDistance))
                        {
                            if(!characterByVisibility.ContainsKey(VisibilityType.Cover))
                            {
                                characterByVisibility.Add(VisibilityType.Cover, new List<CPN_Character>());
                            }
                            characterByVisibility[VisibilityType.Cover].Add(chara);

                            //Debug.Log("Cover");

                            neighbourFound = true;
                            break;
                        }
                    }

                    if (!neighbourFound)
                    {
                        if (!characterByVisibility.ContainsKey(VisibilityType.Lost))
                        {
                            characterByVisibility.Add(VisibilityType.Lost, new List<CPN_Character>());
                        }
                        characterByVisibility[VisibilityType.Lost].Add(chara);

                        //Debug.Log("Lost");
                    }
                }
                else
                {
                    if (!characterByVisibility.ContainsKey(VisibilityType.Visible))
                    {
                        characterByVisibility.Add(VisibilityType.Visible, new List<CPN_Character>());
                    }
                    characterByVisibility[VisibilityType.Visible].Add(chara);

                    //Debug.Log("Visible");
                }
            }

            if (!characterByVisibility.ContainsKey(VisibilityType.Cover) && !characterByVisibility.ContainsKey(VisibilityType.Visible))
            {
                Node targetCharacterNode = RVN_AiBattleManager.Instance.GetClosestCharacter(casterNode, true).CurrentNode;

                foreach (CPN_Character chara in characterByVisibility[VisibilityType.Lost])
                {
                    if (pathByCharacterClear[chara].Contains(n))
                    {
                        characterScore = Pathfinding.GetDistance(n, chara.CurrentNode) - Pathfinding.GetDistance(n, casterNode);
                        //Debug.Log($"{characterScore} = {Pathfinding.GetDistance(n, chara.CurrentNode)} - {Pathfinding.GetDistance(n, casterNode)}");
                    }
                    else if(pathByCharacterBlocked[chara].Contains(n))
                    {
                        characterScore = Pathfinding.GetDistance(n, chara.CurrentNode) - Pathfinding.GetDistance(n, casterNode) + NoClearPathScoreMalus;
                        //Debug.Log($"{characterScore} = {Pathfinding.GetDistance(n, chara.CurrentNode)} - {Pathfinding.GetDistance(n, casterNode)} + NoClearPathScoreMalus");
                    }
                    else
                    {
                        characterScore = NoPathScoreMalus;
                    }

                    nodeScore += characterScore;
                }
            }
            else
            {
                foreach(KeyValuePair<VisibilityType, List<CPN_Character>> visibilityCharacterPair in characterByVisibility)
                {
                    foreach(CPN_Character chara in visibilityCharacterPair.Value)
                    {
                        switch(visibilityCharacterPair.Key)
                        {
                            case VisibilityType.Cover:
                                characterScore = 0;
                                break;
                            case VisibilityType.Visible:
                                characterScore = IsVisibleScoreMalus;
                                break;
                            case VisibilityType.Lost:
                                if (characterByVisibility.ContainsKey(VisibilityType.Cover))
                                {
                                    characterScore = 0;
                                }
                                else
                                {
                                    characterScore = NotCoverScoreMalus;
                                }
                                break;
                        }

                        if (visibilityCharacterPair.Key != VisibilityType.Lost || !characterByVisibility.ContainsKey(VisibilityType.Cover))
                        {
                            float distanceNToCharacterFlat = Pathfinding.GetDistance(n, chara.CurrentNode);

                            //Vérification de la distance voulue
                            if (distanceNToCharacterFlat > distanceFromTargetWanted.y)
                            {
                                characterScore += Mathf.Abs(distanceNToCharacterFlat - distanceFromTargetWanted.y);
                            }
                            else if (distanceNToCharacterFlat < distanceFromTargetWanted.x)
                            {
                                characterScore += Mathf.Abs(distanceNToCharacterFlat - distanceFromTargetWanted.x) * TooCloseMultipler;
                            }
                        }

                        //Debug.Log("Character (" + visibilityCharacterPair.Key + ") score : " + characterScore);

                        nodeScore += characterScore;
                    }
                }
            }

            //Debug.Log("Pos : " + n.worldPosition + " | Score : " + nodeScore);

            if (nodeScore >= 0)
            {
                if (n != casterNode)
                {
                    List<Node> pathFromCasterToN = Pathfinding.CalculatePathfinding(casterNode, n, -1, true, true);

                    pathFromCasterToN.Insert(0, casterNode);

                    float opportunityScore = RVN_AiBattleManager.Instance.OpportunityAttackScore(currentCharacter, currentCharacterHealth, pathFromCasterToN);

                    if(opportunityScore >= 1)
                    {
                        continue;
                    }
                    else if (opportunityScore > 0)
                    {
                        nodeScore += OpportunityAttackScoreMalus;
                    }
                }

                if (maxScore < 0 || nodeScore < maxScore)
                {
                    toReturn = new List<Node>();

                    toReturn.Add(n);

                    maxScore = nodeScore;
                }
                else if (nodeScore == maxScore)
                {
                    toReturn.Add(n);
                }
            }
        }

        return toReturn;
    }

    private bool WillNodeBeWalkableNextTurn(Node movementNode, Node nodeToCheck)
    {
        return nodeToCheck.IsWalkable && (Grid.GetNode(movementNode.gridX, nodeToCheck.gridY).IsWalkable || Grid.GetNode(nodeToCheck.gridX, movementNode.gridY).IsWalkable);
    }
}
