using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	private static Grid instance;

	public bool displayGridGizmos;
	public LayerMask unwalkableMask, visionMasks, characterMasks;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	private Node[,] grid;

	public GameObject caseSprite;
	public Transform caseParent;

	private float nodeDiameter;
	private int gridSizeX, gridSizeY;

	[SerializeField] private UnityEvent<Node[,], int, int> OnCreateGrid;

	void Awake() {
		instance = this;
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	private void CreateGrid() {
		if (grid == null)
		{
			grid = new Node[gridSizeX, gridSizeY];
		}
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius*0.2f,unwalkableMask));
				bool visible = !(Physics2D.OverlapCircle(worldPoint, nodeRadius * 0.2f, visionMasks));

				if (grid[x, y] != null)
				{
					grid[x, y].SetNode(walkable, visible, worldPoint, x, y);
				}
				else
                {
					grid[x, y] = new Node(walkable, visible, worldPoint, x, y); ;
				}
			}
		}

		OnCreateGrid?.Invoke(grid, gridSizeX, gridSizeY);
	}

	public static Node GetNode(int x, int y)
    {
		return instance.grid[x, y];
    }

	public static List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < instance.gridSizeX && checkY >= 0 && checkY < instance.gridSizeY) {
					neighbours.Add(instance.grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}

	public static Node GetRandomNeighbours(Node node)
    {
		List<Node> neighbours = GetNeighbours(node);
		return neighbours[Random.Range(0, neighbours.Count)];
    }
	
	public static Node GetNodeFromWorldPoint(Vector3 worldPoint)
    {
		return instance.NodeFromWorldPoint(worldPoint);
    }

	private Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + nodeRadius + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + nodeRadius + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX) * percentX)-1;
		int y = Mathf.RoundToInt((gridSizeY) * percentY)-1;

		if(x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY)
        {
			return null;
        }

		return grid[x,y];
	}

	public static bool IsNodeVisible(Node startNode, Node targetNode, float distanceMax = -1)
	{
		if(distanceMax > 0 && Pathfinding.GetDistance(startNode, targetNode) > distanceMax)
        {
			return false;
        }

		int x = targetNode.gridX;
		int y = targetNode.gridY;
		int j = y;

		float realJ = y;

		int diffX = targetNode.gridX - startNode.gridX;
		int diffY = targetNode.gridY - startNode.gridY;

		float absX = Mathf.Abs((float)diffX);
		float absY = Mathf.Abs((float)diffY);

		int xCoef = 1;
		int yCoef = 1;

		if (diffX < 0)
		{
			xCoef = -1;
		}
		else if (diffX == 0)
		{
			xCoef = 0;
		}

		if (diffY < 0)
		{
			yCoef = -1;
		}
		else if (diffY == 0)
		{
			yCoef = 0;
		}

		if (absX == absY)
		{
			for (int i = x; i != startNode.gridX; i -= xCoef)
			{
				if (!GetNode(i, j).IsVisible)
				{
					return false;
				}

				realJ -= yCoef;
				j = Mathf.RoundToInt(realJ);
			}
		}
		else if (absX > absY)
		{
			for (int i = x; i != startNode.gridX; i -= xCoef)
			{
				if (!GetNode(i, j).IsVisible)
				{
					return false;
				}

				if (yCoef != 0 && diffY != 0)
				{
					realJ -= (absY / absX) * (float)yCoef;
					j = Mathf.RoundToInt(realJ);
				}
			}
		}
		else if (absX < absY)
		{
			realJ = x;
			j = x;
			for (int i = y; i != startNode.gridY; i -= yCoef)
			{
				if (!GetNode(j, i).IsVisible)
				{
					return false;
				}

				if (xCoef != 0 && diffX != 0)
				{
					realJ -= (absX / absY) * (float)xCoef;
					j = Mathf.RoundToInt(realJ);
				}
			}
		}

		return true;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				bool redColor = n.IsWalkable;
				Gizmos.color = redColor?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter/2));
			}
		}
	}
}