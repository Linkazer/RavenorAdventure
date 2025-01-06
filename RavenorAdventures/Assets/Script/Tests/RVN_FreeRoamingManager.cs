using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_FreeRoamingManager : RVN_Singleton<RVN_FreeRoamingManager>
{
    private List<CPN_Character> characterMovements = new List<CPN_Character>();

    private CPN_Character selectedChara = null;

    private bool areCharacterGrouped = true;

    public Action<bool> actOnEnableRoaming;

    public bool AreCharacterGrouped => areCharacterGrouped && selectedChara != null;

    // Start is called before the first frame update
    private void OnEnable()
    {
        RVN_BattleManager.ActOnExitBattle += OnExitBattle;
        RVN_BattleManager.ActOnEnterBattle += OnEnterBattle;
        RVN_BattleManager.ActOnSpawnAlly += AddCharacter;
        RVN_BattleManager.Instance.ActOnStartCharacterTurn += SetSelectedChara;

        foreach (CPN_Character chara in RVN_BattleManager.GetPlayerTeam)
        {
            characterMovements.Add(chara);
        }

        if(RVN_BattleManager.GetEnemyTeam.Count == 0)
        {
            OnExitBattle();
        }
    }

    private void OnDisable()
    {
        RVN_BattleManager.ActOnExitBattle -= OnExitBattle;
        RVN_BattleManager.ActOnEnterBattle -= OnEnterBattle;
        RVN_BattleManager.ActOnSpawnAlly -= AddCharacter;
        RVN_BattleManager.Instance.ActOnStartCharacterTurn -= SetSelectedChara;
    }

    public void SetGrouped(bool grouped)
    {
        if (areCharacterGrouped != grouped)
        {
            areCharacterGrouped = grouped;

            OnSelectedCharaMove();
        }
    }

    private void OnEnterBattle()
    {
        foreach (CPN_Character chara in characterMovements)
        {
            if (chara.TryGetComponent<CPN_Movement>(out CPN_Movement charaMvt))
            {
                charaMvt.StopMovement();
            }
            chara.NodeData.SetWalkable(false);
        }

        SetSelectedChara(null);

        actOnEnableRoaming?.Invoke(false);
    }

    private void OnExitBattle()
    {
        foreach (CPN_Character chara in characterMovements)
        {
            chara.NodeData.SetWalkable(true);
        }

        SetSelectedChara(RVN_BattleManager.CurrentCharacter);

        actOnEnableRoaming?.Invoke(true);
    }

    private void OnSelectedCharaMove()
    {
        if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime && !RVN_RoundManager.instance.IsPaused)
        {
            if (areCharacterGrouped && selectedChara != null)
            {
                int posOffsetIndex = 0;

                if (selectedChara.TryGetComponent<CPN_Movement>(out CPN_Movement selectedMvt))
                {
                    Node[] moveNodes = GetFollowerPositions(selectedMvt.MovementTarget, selectedMvt.PreviousMovmentTarget, characterMovements.Count - 1);

                    foreach (CPN_Character chara in characterMovements)
                    {
                        if (chara != selectedChara && chara.TryGetComponent<CPN_Movement>(out CPN_Movement charaMvt))
                        {
                            charaMvt.AskToMoveTo(moveNodes[posOffsetIndex].worldPosition, null);
                            posOffsetIndex++;
                        }
                    }
                }
            }
        }
    }

    private void SetSelectedChara(CPN_Character characterToPlay)
    {
        if(selectedChara != null && selectedChara.TryGetComponent<CPN_Movement>(out CPN_Movement selectedMvt))
        {
            selectedMvt.ActOnMvtFound -= OnSelectedCharaMove;
        }
        selectedChara = characterToPlay;
        if (selectedChara != null && selectedChara.TryGetComponent<CPN_Movement>(out CPN_Movement selectedMvt2))
        {
            selectedMvt2.ActOnMvtFound += OnSelectedCharaMove;
        }
    }

    private void AddCharacter(CPN_Character toAdd)
    {
        characterMovements.Add(toAdd);
    }

    private Node[] GetFollowerPositions(Node targetNode, Node beforeTargetNode, int numberPositionWanted)
    {
        Vector2 direction = new Vector2(beforeTargetNode.gridX - targetNode.gridX, beforeTargetNode.gridY - targetNode.gridY);

        Node[] toReturn = new Node[numberPositionWanted];
        List<Node> usedNodes = new List<Node>();
        usedNodes.Add(targetNode);

        List<Node> usableNodes = GetFollowerNodes(direction, targetNode, numberPositionWanted);
        for(int i = 0; i < numberPositionWanted; i++)
        {
            toReturn[i] = usableNodes[i];
        }

        return toReturn;
    }

    private List<Node> GetFollowerNodes(Vector2 direction, Node startNode, int numberPositionWanted)
    {
        List<Node> allNodeInDistance = Pathfinding.GetAllNodeInDistance(startNode, numberPositionWanted * 10, false);

        allNodeInDistance.Remove(startNode);

        for(int i = 0; i < allNodeInDistance.Count; i++)
        {
            if(!allNodeInDistance[i].IsWalkable)// && direction.x != 0 && ((allNodeInDistance[i].gridX - startNode.gridX) * direction.x > 0) && ((allNodeInDistance[i].gridY - startNode.gridY) *direction.y > 0))
            {
                allNodeInDistance.RemoveAt(i);
                i--;
            }
        }

        allNodeInDistance.Sort((n1, n2) => CompareNodes(n1, n2, startNode, direction));

        return allNodeInDistance;
    }

    private int CompareNodes(Node n1, Node n2, Node startNode, Vector2 direction)
    {
        int n1Score = 0, n2Score = 0;

        n1Score = Pathfinding.GetDistance(startNode, n1);
        if(direction.x != 0 && ((n1.gridX - startNode.gridX) * direction.x < 0))
        {
            n1Score += 50;
        }
        else if(direction.y == 0 && ((n1.gridX - startNode.gridX) * direction.x > 0))
        {
            n1Score -= 25;
        }
        if (direction.y != 0 && ((n1.gridY - startNode.gridY) * direction.y < 0))
        {
            n1Score += 50;
        }
        else if (direction.x == 0 && ((n1.gridY - startNode.gridY) * direction.y > 0))
        {
            n1Score -= 25;
        }
        if (!Grid.IsNodeVisible(startNode, n1))
        {
            n1Score += 100;
        }

        n2Score = Pathfinding.GetDistance(startNode, n2);
        if (direction.x != 0 && ((n2.gridX - startNode.gridX) * direction.x < 0))
        {
            n2Score += 50;
        }
        else if (direction.y == 0 && ((n2.gridX - startNode.gridX) * direction.x > 0))
        {
            n2Score -= 25;
        }
        if (direction.y != 0 && ((n2.gridY - startNode.gridY) * direction.y < 0))
        {
            n2Score += 50;
        }
        else if (direction.x == 0 && ((n2.gridY - startNode.gridY) * direction.y > 0))
        {
            n2Score -= 25;
        }
        if (!Grid.IsNodeVisible(startNode, n2))
        {
            n2Score += 100;
        }

        if (n1Score > n2Score)
        {
            return 1;
        }
        else if(n1Score < n2Score)
        {
            return -1;
        }

        return 0;
    }
}
