using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float heightByDistance;
    [SerializeField] private AnimationCurve heightCurve;

    [SerializeField] private bool faceDirection = false;

    private Vector2 direction;
    private Node targetNode;

    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private Vector2 nextPosition;

    private Action toPlayAtReachTarget;

    private float maxDistance;
    private float currentTraveledDistance;

    private Vector2 RealPosition => transform.position;

    public void SetProjectile(Node nStartNode, Node nTargetNode, Action callback)
    {
        transform.position = nStartNode.worldPosition;

        currentPosition = nStartNode.worldPosition;

        direction = (nTargetNode.worldPosition - nStartNode.worldPosition).normalized;
        targetNode = nTargetNode;

        maxDistance = Vector2.Distance(nTargetNode.worldPosition, nStartNode.worldPosition);
        currentTraveledDistance = 0;

        toPlayAtReachTarget = callback;
    }

    private void Update()
    {
        nextPosition = currentPosition + (direction * speed * Time.deltaTime);

        currentTraveledDistance += direction.magnitude * speed * Time.deltaTime;

        if(Vector2.Distance(nextPosition, targetNode.worldPosition) > Vector2.Distance(currentPosition, targetNode.worldPosition))
        {
            toPlayAtReachTarget?.Invoke();

            Destroy(gameObject);

            return;
        }

        currentPosition = nextPosition;

        transform.position = nextPosition + (new Vector2(0, heightCurve.Evaluate(currentTraveledDistance / maxDistance) * (heightByDistance * maxDistance)));

        if (faceDirection)
        {
            transform.up = RealPosition - lastPosition;
        }

        lastPosition = transform.position;
    }
}
