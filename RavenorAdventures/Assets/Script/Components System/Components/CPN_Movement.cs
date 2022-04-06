using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Movement : CPN_CharacterAction<CPN_Data_Movement>
{
	//Vitesse de déplacement
	[SerializeField] private float speed = 1;
	//Distance de déplacement possible pendant 1 tour.
	[SerializeField] private int maxDistance = 100;

	/// <summary>
	/// Played when the component enter a new Node.
	/// </summary>
	[SerializeField] private UnityEvent<Node> OnEnterNode;
	/// <summary>
	/// Played when the component exit the Node.
	/// </summary>
	[SerializeField] private UnityEvent<Node> OnExitNode;
	/// <summary>
	/// Played when the component start to move.
	/// </summary>
	[SerializeField] private UnityEvent OnStartMovement;
	/// <summary>
	/// Played when the component stop its movement.
	/// </summary>
	[SerializeField] private UnityEvent OnStopMovement;
	/// <summary>
	/// Played when the component reach its destination.
	/// </summary>
	[SerializeField] private UnityEvent OnEndMovement;

	/// <summary>
	/// Played when the component reach its destination.
	/// </summary>
	private Action OnEndMovementAction;

	/// <summary>
	/// The path the component has to take.
	/// </summary>
	private Node[] path;
	/// <summary>
	/// The current index of the path the component is taking.
	/// </summary>
	private int targetIndex;

	/// <summary>
	/// Position of the component in the scene.
	/// </summary>
	private Vector2 posUnit;
	/// <summary>
	/// Position of the destination in the scene.
	/// </summary>
	private Vector2 posTarget;
	/// <summary>
	/// The current direction of the component.
	/// </summary>
	private Vector2 direction;
	/// <summary>
	/// The node on which the component is.
	/// </summary>
	private Node currentNode = null;

	private Coroutine currentMovement;

	/// <summary>
	/// The amount of movement left for the current turn.
	/// </summary>
	[SerializeField] private int currentMovementLeft;

	void Start()
	{
		currentNode = Grid.GetNodeFromWorldPoint(transform.position);
	}

	/// <summary>
	/// Called when the component find a path to follow.
	/// </summary>
	/// <param name="newPath">The path to follow.</param>
	/// <param name="pathSuccessful">TRUE if there is a path to follow.</param>
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

	/// <summary>
	/// Get all the node the component can walk on during its turn.
	/// </summary>
	/// <returns>The node the component can walk on.</returns>
	public List<Node> GetPossibleMovementTarget()
    {
		return Pathfinding.CalculatePathfinding(currentNode, null, currentMovementLeft);
    }

	private bool CanMoveToDestination(Vector2 destination)
    {
		Node toCheck = Grid.GetNodeFromWorldPoint(destination);

		return GetPossibleMovementTarget().Contains(toCheck);
    }

	/// <summary>
	/// Ask the component to move to a destination.
	/// </summary>
	/// <param name="targetPosition">The destination the component has to reach.</param>
	/// <param name="callback">The callback to play once the component reach its destination.</param>
	public void AskToMoveTo(Vector2 targetPosition, Action callback)
    {
		OnEndMovementAction += callback;

		PathRequestManager.RequestPath(transform.position, targetPosition, currentMovementLeft, OnPathFound);
	}

	/// <summary>
	/// Stop the current movement of the component.
	/// </summary>
	public void StopMovement()
    {
		StopCoroutine(currentMovement);

		transform.position = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);

		currentMovementLeft -= currentNode.gCost;

		OnEndMovementAction = null;
		OnStopMovement?.Invoke();
	}

	/// <summary>
	/// Make the component follow the path.
	/// </summary>
	/// <returns></returns>
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
		return currentMovementLeft >= 10 && CanMoveToDestination(actionTargetPosition);
    }
}
