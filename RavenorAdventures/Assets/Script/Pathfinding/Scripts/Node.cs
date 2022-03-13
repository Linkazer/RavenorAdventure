using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node> {

	private List<NodeDataHanlder> datasOnNode = new List<NodeDataHanlder>();

	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;
	public Node children;
	int heapIndex;

	public Node(bool _walkable, bool _blockVision, Vector3 _worldPos, int _gridX, int _gridY) {

		SetNode(_walkable, _blockVision, _worldPos, _gridX, _gridY);
	}

	public void SetNode(bool _walkable, bool _blockVision, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;

		gCost = 0;
		hCost = 0;
		parent = null;
		children = null;
		heapIndex = 0;
	}

	public bool IsWalkable => walkable && CheckWalkableData();

	public bool IsVisible => CheckVisibleData();

	public List<NodeDataHanlder> DatasOnNode => datasOnNode;

	private bool CheckWalkableData()
    {
		bool toReturn = true;
		
		for(int i = 0; i < datasOnNode.Count; i++)
        {
			if(!datasOnNode[i].Walkable)
            {
				toReturn = false;
            }
        }
		return toReturn;
    }

	private bool CheckVisibleData()
    {
		bool toReturn = true;

		for (int i = 0; i < datasOnNode.Count; i++)
		{
			if (datasOnNode[i].BlockVision)
			{
				toReturn = false;
			}
		}
		return toReturn;
	}

	public void AddDataOnNode(NodeDataHanlder toAdd)
	{
		if (!datasOnNode.Contains(toAdd))
		{
			Debug.Log("Add" + worldPosition);
			datasOnNode.Add(toAdd);
		}
	}

	public void RemoveDataOnNode(NodeDataHanlder toRemove)
    {
		if (datasOnNode.Contains(toRemove))
		{
			Debug.Log("Remove" + worldPosition);
			datasOnNode.Remove(toRemove);
		}
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
