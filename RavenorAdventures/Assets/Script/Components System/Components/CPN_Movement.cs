using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Movement : CPN_CharacterAction<CPN_Data_Movement>
{
	//Vitesse de d�placement
	[SerializeField] private float speed = 1;
	//Distance de d�placement possible pendant 1 tour.
	[SerializeField] private int maxDistance = 100;

	private bool isMovementCosting = true;

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

	public Action ActOnMvtFound;

	/// <summary>
	/// Played when the component reach its destination.
	/// </summary>
	private Action OnEndMovementAction;

	/// <summary>
	/// The path the component has to take.
	/// </summary>
	private Node[] path = new Node[0];
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

	public int currentEvasiveAmount;

	/// <summary>
	/// The amount of movement left for the current turn.
	/// </summary>
	private int currentMovementLeft;

	private List<CPN_InteractibleObject> targetInteractions = new List<CPN_InteractibleObject>();

	public Node CurrentNode => currentNode;

	public int MaxMovement => maxDistance;
	public int MovementLeft => currentMovementLeft;

	public bool CanMove => currentMovementLeft >= 10;
	public Node MovementTarget => path.Length > 0 ? path[path.Length - 1] : currentNode;

	public Node PreviousMovmentTarget => path.Length > 1 ? path[path.Length - 2] : currentNode;

    protected override CPN_Data_Movement GetDataFromHandler()
    {
        if (handler is CPN_Character)
        {
            return (handler as CPN_Character).Scriptable;
        }

        return null;
    }

    protected override void SetData(CPN_Data_Movement toSet)
    {
        speed = toSet.Speed();

        maxDistance = toSet.MaxDistance();

        ResetData();
    }

    public override void Activate()
    {
        currentNode = handler.CurrentNode;
    }

    public override void OnUpdateRoundMode(RVN_RoundManager.RoundMode settedMode)
    {
        base.OnUpdateRoundMode(settedMode);

		switch (settedMode)
		{
			case RVN_RoundManager.RoundMode.Round:
				doesBlockInput = true;
                isMovementCosting = true;
                break;
			case RVN_RoundManager.RoundMode.RealTime:
				doesBlockInput = false;
                isMovementCosting = false;
                break;
		}
    }

    public override void Disactivate()
    {

    }

    public override void ResetData()
    {
        currentMovementLeft = maxDistance;
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        if (CanMove && RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime)
        {
            return true;
        }

        Node toCheck = Grid.GetNodeFromWorldPoint(actionTargetPosition);

		if(toCheck.GetNodeComponent<CPN_InteractibleObject>().Count > 0)
		{
			if(Pathfinding.GetDistance(toCheck, CurrentNode) < 15)
			{
				return true;
			}
			else if(CanMove)
			{
				return CanInteractWithObject(toCheck.GetNodeComponent<CPN_InteractibleObject>()[0]);
			}
		}
		else
		{
            return CanMove && CanMoveToDestination(actionTargetPosition);
        }

		return false;
    }

    public override bool CanSelectAction()
    {
        return true;
    }

    public override void UnselectAction()
    {
		if (currentMovement != null)
		{
			StopMovement();
		}
    }

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        if (RVN_RoundManager.Instance.CurrentRoundMode == RVN_RoundManager.RoundMode.RealTime)
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

            if (CheckForOpportunityAttack(currentNode))
            {
                foundOpportunityAttack = true;
            }

            foreach (Node n in path)
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

    public override bool TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        AskToMoveTo(actionTargetPosition, callback);

		return true;
    }

    public void AddMovement(int amount)
    {
		maxDistance += amount;
		currentMovementLeft += amount;
        Debug.Log(this + " : " + currentMovementLeft);
    }

	public void SetCurrentMovement(int amount)
    {
		currentMovementLeft = Mathf.Clamp(amount, 0, maxDistance);
        Debug.Log(this + " : " + currentMovementLeft);
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
			if(currentMovement != null)
			{
				CancelMovement();
			}

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

	private bool CanInteractWithObject(CPN_InteractibleObject interactibleObject)
	{
		List<Node> possibleMovement = GetPossibleMovementTarget();

        foreach (Node interactibleNeighbourNode in Grid.GetNeighbours(interactibleObject.Handler.CurrentNode))
		{
			if (possibleMovement.Contains(interactibleNeighbourNode))
			{
				return true;
			}
		}

		return false;
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
        Node targetNode = Grid.GetNodeFromWorldPoint(targetPosition);

		targetInteractions = targetNode.GetNodeComponent<CPN_InteractibleObject>();

        OnEndMovementAction = callback;

        PathRequestManager.RequestPath(transform.position, targetPosition, -1, OnPathFound);
	}

	public void ForceToMove(Vector2 targetPosition, Action callback)
    {
        //isMovementCosting = false; => R�parer les d�placement dans les tutos

        Node targetNode = Grid.GetNodeFromWorldPoint(targetPosition);

        targetInteractions = targetNode.GetNodeComponent<CPN_InteractibleObject>();

        OnEndMovementAction = callback;

        PathRequestManager.RequestPath(transform.position, targetPosition, -1, OnPathFound);
    }

	private void StartMovement()
	{
		if (enabled)
		{
            currentMovement = StartCoroutine(FollowPath());
		}
	}

	private void CancelMovement()
    {
		if (handler.GetComponentOfType(out CPN_ANIM_Character animHandler))
        {
            animHandler.EndAnimation();
		}
        targetInteractions.Clear();

        OnEndMovement?.Invoke();

        OnEndMovementAction?.Invoke();
        OnEndMovementAction = null;

		StopMovement();
    }

	private void EndMovement()
    {
		if (handler.GetComponentOfType(out CPN_ANIM_Character animHandler))
        {
            animHandler.EndAnimation();
		}
		OnEndMovement?.Invoke();

		OnEndMovementAction?.Invoke();
		OnEndMovementAction = null;

		if (targetInteractions.Count > 0)
		{
            targetInteractions[0].TryInteract(handler);
			targetInteractions.Clear();
        }

		StopMovement();
	}

	/// <summary>
	/// Stop the current movement of the component.
	/// </summary>
	public void StopMovement()
    {
		if (currentMovement != null)
		{
			StopCoroutine(currentMovement);
			currentMovement = null;
        }

		transform.position = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);

		OnEndMovementAction = null;
		if (handler.GetComponentOfType(out CPN_ANIM_Character animHandler))
		{
            animHandler.EndAnimation();
		}
		OnStopMovement?.Invoke();
	}

	/// <summary>
	/// Make the component follow the path.
	/// </summary>
	/// <returns></returns>
	private IEnumerator FollowPath()
	{
		if (!isMovementCosting || !CheckForOpportunityAttack())
		{
			Node currentWaypoint = path[targetIndex];

			ActOnMvtFound?.Invoke();

            OnStartMovement?.Invoke();

			if (handler.GetComponentOfType(out CPN_ANIM_Character animHandler))
			{
                animHandler.PlayAnimation("Character_Walk");
			}

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
						if (isMovementCosting)
						{
							currentMovementLeft -= currentNode.gCost;
                        }

						EndMovement();

						transform.position = currentWaypoint.worldPosition;
						break;
					}

					currentWaypoint = path[targetIndex];

					lerpValue--;

					posUnit = new Vector2(transform.position.x, transform.position.y);
					posTarget = new Vector2(currentWaypoint.worldPosition.x, currentWaypoint.worldPosition.y);

					OnChangeDirection?.Invoke(posTarget - posUnit);

					distance = Vector2.Distance(posUnit, posTarget);

					if (isMovementCosting && (CheckForOpportunityAttack() || RVN_RoundManager.Instance.IsPaused))
					{
						//currentMovementLeft -= currentNode.gCost;
                        //Debug.Log(this + " : " + currentMovementLeft);

                        animHandler.EndAnimation();
						OnStopMovement?.Invoke();
						StopCoroutine(currentMovement);
						currentMovement = null;
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

    public bool CheckForOpportunityAttack()//CODE REVIEW : Voir si on peut pas le mettre autre part que dans le d�placement (Cr�er un lien entre le D�placement et le SpellCaster qui est pas ouf)
	{
		if(currentEvasiveAmount > 0)
        {
			return false;
        }

		bool hasBeenHited = false;

		List<Node> neighbours = Grid.GetNeighbours(CurrentNode);

		foreach (Node n in neighbours)
		{
			List<CPN_Character> characterOnMelee = n.GetNodeHandler<CPN_Character>();

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
			List<CPN_Character> characterOnMelee = n.GetNodeHandler<CPN_Character>();

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

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
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
