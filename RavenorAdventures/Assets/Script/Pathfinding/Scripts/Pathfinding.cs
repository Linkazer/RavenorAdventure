using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour 
{
	private static Pathfinding instance;
	[SerializeField] private PathRequestManager requestManager;
	[SerializeField] private Grid grid;
	
	void Awake() {
		instance = this;
	}
	

	public static void StartFindPath(Vector3 startPos, Vector3 targetPos, int walkDistance)
	{
		instance.OnStartFindPath(startPos, targetPos, walkDistance);
	}
	
	private void OnStartFindPath(Vector3 startPos, Vector3 targetPos, int walkDistance) {
		StartCoroutine(FindPath(startPos,targetPos, walkDistance));
	}
	
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, int walkDistance) {

		Node[] waypoints = new Node[0];
		bool pathSuccess = false;
		
		Node startNode = Grid.GetNodeFromWorldPoint(startPos);
		Node targetNode = Grid.GetNodeFromWorldPoint(targetPos);

		if (targetNode.IsWalkable) {
			pathSuccess = SearchPath(startNode, targetNode);
		}

		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode, walkDistance);
		}
		if(waypoints.Length <= 0 || waypoints[0] == null)
        {
			pathSuccess = false;
        }
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
		
	}
	
	private Node[] RetracePath(Node startNode, Node endNode, int maxDistance) {

		Node[] path = GetPath(startNode, endNode, maxDistance).ToArray();
		Array.Reverse(path);
		return path;
	}
	
	private Vector3[] GetVectorPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		
		for (int i = 0; i < path.Count; i ++) {
				waypoints.Add(path[i].worldPosition);
		}
		return waypoints.ToArray();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="startNode"></param>
	/// <param name="targetNode">If null, will set all ode in the distance.</param>
	/// <param name="isForNextTurn"></param>
	/// <returns></returns>
	public static bool SearchPath(Node startNode, Node targetNode)
    {
		return instance.OnSearchPath(startNode, targetNode);
    }

	private bool OnSearchPath(Node startNode, Node targetNode)
	{
		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();

		startNode.gCost = 0;

		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);
			foreach (Node neighbour in Grid.GetNeighbours(currentNode))
			{
				if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				int newDistanceFromTargetCost = 0;

				if (targetNode != null)
				{
					newDistanceFromTargetCost = GetDistance(currentNode, targetNode);
				}

				if (newMovementCostToNeighbour == neighbour.gCost && newDistanceFromTargetCost < neighbour.hCost)
				{
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = newDistanceFromTargetCost;
					neighbour.parent = currentNode;
					currentNode.children = neighbour;

					if (!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}
				else if (newMovementCostToNeighbour <= neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = newDistanceFromTargetCost;
					neighbour.parent = currentNode;
					currentNode.children = neighbour;

					if (!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}

				if(targetNode == neighbour)
                {
					return true;
                }
			}
		}
		return false;
	}

	private List<Node> GetPath(Node startNode, Node endNode, int maxDistance)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		List<Node> checkedNodes = new List<Node>();

		while (currentNode.gCost > maxDistance && !checkedNodes.Contains(currentNode))
        {
			checkedNodes.Add(currentNode);
			if (currentNode.parent != null)
			{
				currentNode = currentNode.parent;
			}
			else
            {
				break;
            }
		}

		while (currentNode != startNode)
		{
			if(currentNode.parent == null)
            {
				path = new List<Node>();
				break;
			}
			else
			{
				path.Add(currentNode);
			}
			currentNode = currentNode.parent;
		}

		return path;
	}
	
	public static int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 15*dstY + 10* (dstX-dstY);
		return 15*dstX + 10 * (dstY-dstX);
	}

	public static List<Node> GetNodesWithMaxDistance(Node startNode, float distance, bool pathCalcul)
	{
		Heap<Node> openSet = new Heap<Node>(instance.grid.MaxSize);
		List<Node> usableNode = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);
		usableNode.Add(startNode);

		while (openSet.currentItemCount > 0)
		{
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			foreach (Node neighbour in Grid.GetNeighbours(currentNode))
			{
				if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour <= distance)
				{
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.parent = currentNode;
						currentNode.children = neighbour;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
							usableNode.Add(neighbour);
						}
					}
				}
			}
		}

		return usableNode;
	}
}
