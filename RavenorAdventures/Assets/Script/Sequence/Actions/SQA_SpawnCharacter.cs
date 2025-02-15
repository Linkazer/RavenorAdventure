using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_SpawnCharacter : SequenceAction
{
    [SerializeField] private bool spawnAsInstance = false;
    [SerializeField] private SpawnableCharacterTeam charactersToSpawn;
    [SerializeField] private Transform[] spawnPositions;

    [SerializeField] private int maxSpawnDistance = 35;

    protected override void OnStartAction()
    {
        for(int i = 0; i < charactersToSpawn.charaToSpawns.Count; i++)
        {
            Node spawnNode = GetSpawnNode(charactersToSpawn.charaToSpawns[i].CurrentNode.worldPosition);

            if (spawnPositions.Length != 0)
            {
                spawnNode = GetSpawnNode(spawnPositions[i].position);
            }

            if (spawnAsInstance)
            {
                RVN_BattleManager.SpawnCharacter(charactersToSpawn.charaToSpawns[i], charactersToSpawn.teamIndex, spawnNode.worldPosition);
            }
            else
            {
                RVN_BattleManager.ActivateCharacter(charactersToSpawn.charaToSpawns[i], charactersToSpawn.teamIndex, spawnNode.worldPosition);
            }
        }

        EndAction();
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }

    protected Node GetSpawnNode(Vector2 positionTarget)
    {
        Node targetNode = Grid.GetNodeFromWorldPoint(positionTarget);
        List<Node> spawnableNodes = new List<Node>();
        float minDistance = maxSpawnDistance;
        float calculatedDistance = 0;

        if (targetNode != null)
        {
            if (!targetNode.IsWalkable)
            {
                List<Node> nodesToCheck = Pathfinding.GetAllNodeInDistance(targetNode, maxSpawnDistance, false);

                foreach (Node node in nodesToCheck)
                {
                    if (node.IsWalkable)
                    {
                        calculatedDistance = Pathfinding.GetDistance(node, targetNode);
                        if (calculatedDistance <= minDistance)
                        {
                            if(calculatedDistance < minDistance)
                            {
                                minDistance = calculatedDistance;
                                spawnableNodes = new List<Node>();
                            }

                            spawnableNodes.Add(node);
                        }
                    }
                }
            }
            else
            {
                spawnableNodes.Add(targetNode);
            }
        }

        return spawnableNodes[Random.Range(0, spawnableNodes.Count)];
    }
}
