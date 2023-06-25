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

	[SerializeField] private RVN_GridDisplayer gridDisplayer;

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

	public void CreateGrid() {
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

		gridDisplayer.OnSetGrid(grid, gridSizeX, gridSizeY);
	}

	public static Node GetNode(int x, int y)
    {
		if(x >= 0 && x < instance.gridSizeX && y >= 0 && y < instance.gridSizeY)
        {
			return instance.grid[x, y];
		}

		return null;
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
		if (distanceMax > 0 && Pathfinding.GetDistance(startNode, targetNode) > distanceMax)
		{
			return false;
		}
		else
		{
			return instance.CalculateVision(startNode.gridX, startNode.gridY, targetNode.gridX, targetNode.gridY);
		}
	}

	public bool CalculateVision(int x1, int y1, int x2, int y2)
	{
		int startX = x1;
		int startY = y1;

		int dx = Mathf.Abs(x1 - x2);
		int dy = Mathf.Abs(y1 - y2);

		int sx = (x1 < x2) ? 1 : -1;
		int sy = (y1 < y2) ? 1 : -1;

		int lastX1 = x1;
		int lastY1 = y1;

		int err = dx - dy;

		int e2 = 2 * err;

		while (true)
		{
			if (lastY1 != y1 && lastX1 != x1)
			{
				if (Mathf.Abs(startX - x1) == Mathf.Abs(startY - y1))
				{
					if (!GetNode(x1, lastY1).IsVisible && !GetNode(lastX1, y1).IsVisible)
					{
						return false;
					}
				}
				else if (Mathf.Abs(startX - x1) > Mathf.Abs(startY - y1))
				{
					if (lastY1 != y1 && !GetNode(lastX1, y1).IsVisible)
					{
						return false;
					}
				}
				else if (Mathf.Abs(startY - y1) > Mathf.Abs(startX - x1))
				{
					if (lastX1 != x1 && !GetNode(x1, lastY1).IsVisible)
					{
						return false;
					}
				}
			}

			if (!GetNode(x1, y1).IsVisible)
			{
				return false;
			}

			if (x1 == x2 && y1 == y2)
			{
				break;
			}

			lastX1 = x1;
			lastY1 = y1;

			e2 = 2 * err;

			if (e2 > -dy)
			{
				err = err - dy;
				x1 = x1 + sx;
			}

			if (e2 < dx)
			{
				err = err + dx;
				y1 = y1 + sy;
			}

			
		}

		return true;
	}


#if UNITY_EDITOR
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
#endif
}
