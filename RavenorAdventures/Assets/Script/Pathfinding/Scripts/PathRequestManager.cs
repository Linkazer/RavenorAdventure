using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;

	bool isProcessingPath;

	void Awake() {
		instance = this;
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, int maxDistance, Action<Node[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart,pathEnd, maxDistance,callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	void TryProcessNext() 
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			Pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.maxDistance);
		}
	}

	public void FinishedProcessingPath(Node[] path, bool success) {
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}

	struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public int maxDistance;
		public Action<Node[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, int _maxDistance, Action<Node[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
			maxDistance = _maxDistance;
		}

	}
}
