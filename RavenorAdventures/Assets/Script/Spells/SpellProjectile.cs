using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 direction;
    private Node targetNode;

    private Vector2 nextPosition;

    private Action toPlayAtReachTarget;

    private Vector2 Position => transform.position;

    public void SetProjectile(Node nStartNode, Node nTargetNode, Action callback)
    {
        transform.position = nStartNode.worldPosition;

        direction = (nTargetNode.worldPosition - nStartNode.worldPosition).normalized;
        targetNode = nTargetNode;

        toPlayAtReachTarget = callback;
    }

    private void Update()
    {
        nextPosition = Position + (direction * speed * Time.deltaTime);

        if(Vector2.Distance(nextPosition, targetNode.worldPosition) > Vector2.Distance(Position, targetNode.worldPosition))
        {
            toPlayAtReachTarget?.Invoke();

            Destroy(gameObject);

            return;
        }

        transform.position = nextPosition;
    }
}
