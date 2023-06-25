using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node> {

	private List<NodeDataHanlder> datasOnNode = new List<NodeDataHanlder>();

	public string testName;
	public string parentName;
	public string childName;
	public bool testVisible = true;

	public bool walkable;
	public bool visible;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;
	public Node children;
	int heapIndex;

	public Node(bool _walkable, bool _visible, Vector3 _worldPos, int _gridX, int _gridY) {

		SetNode(_walkable, _visible, _worldPos, _gridX, _gridY);
	}

	public void SetNode(bool _walkable, bool _visible, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		visible = _visible;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;

		gCost = 0;
		hCost = 0;
		parent = null;
		children = null;
		heapIndex = 0;
	}

	public bool IsStaticObstacle => !walkable;

	public bool IsWalkable => walkable && CheckWalkableData();

	public bool IsVisible =>  visible && CheckVisibleData();

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
			for(int i = 0; i < datasOnNode.Count; i++)
            {
				datasOnNode[i].OnDataEnterCurrentNode(toAdd);
			}

			datasOnNode.Add(toAdd);
		}
	}

	public void RemoveDataOnNode(NodeDataHanlder toRemove)
    {
		if (datasOnNode.Contains(toRemove))
		{
			datasOnNode.Remove(toRemove);

			for (int i = 0; i < datasOnNode.Count; i++)
			{
				datasOnNode[i].OnDataExitCurrentNode(toRemove);
			}
		}
	}

	public List<T> GetNodeComponent<T>() where T : RVN_Component
    {
		List<T> toReturn = new List<T>();

		for (int i = 0; i < datasOnNode.Count; i++)
		{
			if (datasOnNode[i].Handler != null && datasOnNode[i].Handler.GetComponentOfType<T>(out T foundComponent))
			{
				toReturn.Add(foundComponent);
			}
		}
		return new List<T>(toReturn);
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
