using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Movement : CPN_CharacterAction<CPN_Data_Movement>
{
	[SerializeField] private float speed = 1;
	[SerializeField] private int maxDistance = 100;

	[SerializeField] private UnityEvent<Node> OnEnterNode;
	[SerializeField] private UnityEvent<Node> OnExitNode;
	[SerializeField] private UnityEvent OnStartMovement;
	[SerializeField] private UnityEvent OnStopMovement;
	[SerializeField] private UnityEvent OnEndMovement;

	private Action OnEndMovementAction;

	Node[] path;
	int targetIndex;

	private Vector2 posUnit;
	private Vector2 posTarget;
	private Vector2 direction;
	private Node currentNode = null;

	private Coroutine currentMovement;

	[SerializeField] private int currentMovementLeft;

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
        else
        {
			OnEndMovementAction?.Invoke();
		}
	}

	public List<Node> GetPossibleMovementTarget()
    {
		return Pathfinding.GetNodesWithMaxDistance(currentNode, currentMovementLeft, true);
    }

	public void AskToMoveTest(Transform targetTest)
    {
		AskToMoveTo(targetTest.position, null);
    }

	public void AskToMoveTo(Vector2 targetPosition, Action callback)
    {
		OnEndMovementAction += callback;

		PathRequestManager.RequestPath(transform.position, targetPosition, currentMovementLeft, OnPathFound);
	}

	public void StopMovement()
    {
		StopCoroutine(currentMovement);

		transform.position = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);

		currentMovementLeft -= currentNode.gCost;

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
					currentMovementLeft -= currentNode.gCost;

					OnEndMovement?.Invoke();

					OnEndMovementAction?.Invoke();

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

    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
		AskToMoveTo(actionTargetPosition, callback);
    }

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
		Debug.Log("Display possible movement");
    }

	public override void ResetActionData()
	{
		currentMovementLeft = maxDistance;
	}

    public override void SetData(CPN_Data_Movement toSet)
    {
		speed = toSet.Speed();

		maxDistance = toSet.MaxDistance();
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
		return currentMovementLeft >= 10;
    }
}
