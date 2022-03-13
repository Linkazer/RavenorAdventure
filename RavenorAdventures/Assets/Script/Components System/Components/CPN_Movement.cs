using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Movement : RVN_Component
{
	[SerializeField] private float speed = 1;
	[SerializeField] private int maxDistance = 100;

	[SerializeField] private UnityEvent<Node> OnEnterNode;
	[SerializeField] private UnityEvent<Node> OnExitNode;
	[SerializeField] private UnityEvent OnStartMovement;
	[SerializeField] private UnityEvent OnStopMovement;
	[SerializeField] private UnityEvent OnEndMovement;

	Node[] path;
	int targetIndex;

	private Vector2 posUnit;
	private Vector2 posTarget;
	private Vector2 direction;
	private Node currentNode = null;

	private Coroutine currentMovement;

	void Start()
	{
		currentNode = Grid.GetNodeFromWorldPoint(transform.position);
	}

	private void OnPathFound(Node[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			if (currentMovement != null)
			{
				StopCoroutine(currentMovement);
			}
			currentMovement = StartCoroutine(FollowPath());
		}
	}

	public List<Node> GetPossibleMovementTarget()
    {
		return Pathfinding.GetNodesWithMaxDistance(currentNode, maxDistance, true);
    }

	public void AskToMoveTest(Transform targetTest)
    {
		AskToMoveTo(targetTest.position);
    }

	public void AskToMoveTo(Vector2 targetPosition)
    {
		PathRequestManager.RequestPath(transform.position, targetPosition, maxDistance, OnPathFound);
	}

	public void StopMovement()
    {
		StopCoroutine(currentMovement);

		transform.position = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);

		OnStopMovement?.Invoke();
	}

	private IEnumerator FollowPath()
	{
		Node currentWaypoint = path[0];

		OnStartMovement?.Invoke();

		while (true)
		{
			posUnit = new Vector2(transform.position.x, transform.position.y);
			posTarget = new Vector2(currentWaypoint.worldPosition.x, currentWaypoint.worldPosition.y);
			if(currentNode != currentWaypoint && Vector2.Distance(currentWaypoint.worldPosition, posUnit) < Vector2.Distance(currentNode.worldPosition, posUnit))
            {
				OnExitNode?.Invoke(currentWaypoint);

				currentNode = currentWaypoint;

				OnEnterNode?.Invoke(currentWaypoint);
			}

			if (Vector2.Distance(posUnit, posTarget) < (Time.deltaTime * speed))
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					OnEndMovement?.Invoke();

					transform.position = currentWaypoint.worldPosition;

					break;
				}
				currentWaypoint = path[targetIndex];
			}
			direction = new Vector2(currentWaypoint.worldPosition.x - transform.position.x, currentWaypoint.worldPosition.y - transform.position.y).normalized;

			transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i].worldPosition, Vector3.one * 0.1f);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i].worldPosition);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
				}
			}
		}
	}
}
