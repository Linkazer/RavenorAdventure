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
	[SerializeField] private UnityEvent<Vector2> OnChangeDirection;

	[SerializeField] private List<NodeDataHanlder> nodesDatas;

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
	/// The node on which the component is.
	/// </summary>
	private Node currentNode = null;

	private Coroutine currentMovement;

	/// <summary>
	/// The amount of movement left for the current turn.
	/// </summary>
	[SerializeField] private int currentMovementLeft;

	public Node CurrentNode => currentNode;

	public int MaxMovement => maxDistance;
	public int MovementLeft => currentMovementLeft;

	public bool CanMove => currentMovementLeft >= 10;

	private void Start()
	{
		currentNode = Grid.GetNodeFromWorldPoint(transform.position);
	}

    public override void OnEnterBattle()
    {
		Debug.Log("Enter");

		foreach (NodeDataHanlder ndh in nodesDatas)
		{
			ndh.SetWalkable(false);
		}
	}

    public override void OnExitBattle()
    {
		Debug.Log("Exit");

		foreach(NodeDataHanlder ndh in nodesDatas)
        {
			ndh.SetWalkable(true);
        }
	}

    public void AddMovement(int amount)
    {
		maxDistance += amount;
		currentMovementLeft += amount;
    }

	public void SetCurrentMovement(int amount)
    {
		currentMovementLeft = Mathf.Clamp(amount, 0, maxDistance);
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

			//if(!CheckForOpportunityAttack())
            {
				StartMovement();
            }
		}
        else
        {
			EndMovement();
		}
	}

	/// <summary>
	/// Get all the node the component can walk on during its turn.
	/// </summary>
	/// <returns>The node the component can walk on.</returns>
	public List<Node> GetPossibleMovementTarget()
    {
		if (currentMovementLeft >= 10)
		{
			return Pathfinding.CalculatePathfinding(currentNode, null, currentMovementLeft);
		}
        else
        {
			return new List<Node>();
        }
    }

	private bool CanMoveToDestination(Vector2 destination)
    {
		Node toCheck = Grid.GetNodeFromWorldPoint(destination);

		return !RVN_BattleManager.IsInBattle || GetPossibleMovementTarget().Contains(toCheck);
    }

	/// <summary>
	/// Ask the component to move to a destination.
	/// </summary>
	/// <param name="targetPosition">The destination the component has to reach.</param>
	/// <param name="callback">The callback to play once the component reach its destination.</param>
	public void AskToMoveTo(Vector2 targetPosition, Action callback)
    {
		OnEndMovementAction += callback;

		if (RVN_BattleManager.IsInBattle)
		{
			PathRequestManager.RequestPath(transform.position, targetPosition, currentMovementLeft, OnPathFound);
		}
        else
        {
			PathRequestManager.RequestPath(transform.position, targetPosition, -1, OnPathFound);
		}
	}

	public void ForceToMove(Vector2 targetPosition, Action callback)
    {
		OnEndMovementAction += callback;

		PathRequestManager.RequestPath(transform.position, targetPosition, -1, OnPathFound);
	}

	private void StartMovement()
	{
		if (enabled)
		{
			currentMovement = StartCoroutine(FollowPath());
		}
	}

	private void EndMovement()
    {
		handler.animationController?.EndAnimation();
		OnEndMovement?.Invoke();

		OnEndMovementAction?.Invoke();
		OnEndMovementAction = null;
	}

	/// <summary>
	/// Stop the current movement of the component.
	/// </summary>
	public void StopMovement()
    {
		if (currentMovement != null)
		{
			StopCoroutine(currentMovement);
		}

		transform.position = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);

		currentMovementLeft -= currentNode.gCost;

		OnEndMovementAction = null;
		handler.animationController?.EndAnimation();
		OnStopMovement?.Invoke();
	}

	/// <summary>
	/// Make the component follow the path.
	/// </summary>
	/// <returns></returns>
	private IEnumerator FollowPath()
	{
		if (!CheckForOpportunityAttack())
		{
			Node currentWaypoint = path[targetIndex];

			OnStartMovement?.Invoke();
			handler.animationController?.PlayAnimation("Character_Walk");

			float lerpValue = 0;
			float distance = 0;

			posUnit = new Vector2(transform.position.x, transform.position.y);
			posTarget = new Vector2(currentWaypoint.worldPosition.x, currentWaypoint.worldPosition.y);

			OnChangeDirection?.Invoke(posTarget - posUnit);

			distance = Vector2.Distance(posUnit, posTarget);


			while (true)
			{
				lerpValue += Time.deltaTime * speed / distance;

				if (currentNode != currentWaypoint && lerpValue >= 1)
				{
					ChangeNode(currentWaypoint);
				}

				if (lerpValue >= 1)// Vector2.Distance(posUnit, posTarget) < (Time.deltaTime * speed))
				{
					targetIndex++;

					if (targetIndex >= path.Length)
					{
						currentMovementLeft -= currentNode.gCost;

						EndMovement();

						transform.position = currentWaypoint.worldPosition;

						break;
					}

					currentWaypoint = path[targetIndex];

					lerpValue = 0;

					posUnit = new Vector2(transform.position.x, transform.position.y);
					posTarget = new Vector2(currentWaypoint.worldPosition.x, currentWaypoint.worldPosition.y);

					OnChangeDirection?.Invoke(posTarget - posUnit);

					distance = Vector2.Distance(posUnit, posTarget);

					if (CheckForOpportunityAttack())
					{
						currentMovementLeft -= currentNode.gCost;

						handler.animationController?.EndAnimation();
						OnStopMovement?.Invoke();
						StopCoroutine(currentMovement);
					}
				}


				transform.position = Vector3.Lerp(posUnit, posTarget, lerpValue);
				yield return null;
			}
		}
	}

	public void Teleport(Node teleportNode)
    {
		ChangeNode(teleportNode);

		transform.position = teleportNode.worldPosition;
    }

	private void ChangeNode(Node newNode)
    {
		OnExitNode?.Invoke(currentNode);

		currentNode = newNode;

		OnEnterNode?.Invoke(currentNode);
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

    public override void UnselectAction()
    {
        
    }

    public override void TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
		AskToMoveTo(actionTargetPosition, callback);
    }

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
		if(!RVN_BattleManager.IsInBattle)
        {
			return;
        }

		Color colorMovement = Color.green;
		colorMovement.a = 0.5f;
		RVN_GridDisplayer.SetGridFeedback(GetPossibleMovementTarget(), colorMovement);

		if (Grid.GetNodeFromWorldPoint(actionTargetPosition) != null)
		{
			List<Node> path = Pathfinding.CalculatePathfinding(currentNode, Grid.GetNodeFromWorldPoint(actionTargetPosition), currentMovementLeft);

			List<Node> validPath = new List<Node>();
			List<Node> opportunityPath = new List<Node>();

			bool foundOpportunityAttack = false;

			if(CheckForOpportunityAttack(currentNode))
            {
				foundOpportunityAttack = true;
            }

			foreach(Node n in path)
            {
				if (!foundOpportunityAttack)
				{
					if (CheckForOpportunityAttack(n))
					{
						foundOpportunityAttack = true;
					}

					validPath.Add(n);
				}
                else
                {
					opportunityPath.Add(n);
				}
            }

			RVN_GridDisplayer.SetGridFeedback(validPath, Color.green);
			RVN_GridDisplayer.SetGridFeedback(opportunityPath, Color.red);
		}
    }

    public override void UndisplayAction(Vector2 actionTargetPosition)
    {
		RVN_GridDisplayer.UnsetGridFeedback();
	}

    public override void ResetData()
	{
		currentMovementLeft = maxDistance;
	}

    public override void SetData(CPN_Data_Movement toSet)
    {
		speed = toSet.Speed();

		maxDistance = toSet.MaxDistance();

		ResetData();
    }

    public override bool CanSelectAction()
    {
        return true;
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
		return !RVN_BattleManager.IsInBattle || (CanMove && CanMoveToDestination(actionTargetPosition));
    }

	public bool CheckForOpportunityAttack()//CODE REVIEW : Voir si on peut pas le mettre autre part que dans le déplacement (Créer un lien entre le Déplacement et le SpellCaster qui est pas ouf)
	{
		bool hasBeenHited = false;

		List<Node> neighbours = Grid.GetNeighbours(CurrentNode);

		foreach (Node n in neighbours)
		{
			List<CPN_Character> characterOnMelee = n.GetNodeComponent<CPN_Character>();

			foreach (CPN_Character chara in characterOnMelee)
			{
				if (!RVN_BattleManager.AreCharacterAllies(chara, handler as CPN_Character))
				{
					if (chara.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster ennemyCaster))
					{
						if (ennemyCaster.hasOpportunityAttack)
						{
							ennemyCaster.SelectSpell(0, false);
							if (!hasBeenHited)
							{
								ennemyCaster.OpportunityAttack(CurrentNode.worldPosition, StartMovement);
								hasBeenHited = true;

								ennemyCaster.hasOpportunityAttack = false;

								return true;
							}
							else
							{
								ennemyCaster.OpportunityAttack(CurrentNode.worldPosition, null);
							}

							
						}
					}
				}
			}
		}

		return hasBeenHited;
	}

	private bool CheckForOpportunityAttack(Node nodeToCheck)
    {
		List<Node> neighbours = Grid.GetNeighbours(nodeToCheck);

		foreach (Node n in neighbours)
		{
			List<CPN_Character> characterOnMelee = n.GetNodeComponent<CPN_Character>();

			foreach (CPN_Character chara in characterOnMelee)
			{
				if (!RVN_BattleManager.AreCharacterAllies(chara, handler as CPN_Character))
				{
					if (chara.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster ennemyCaster))
					{
						if (ennemyCaster.hasOpportunityAttack)
						{
							return true;
						}
					}
				}
			}
		}

		return false;
	}
}
