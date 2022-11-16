using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CANIM_JumpOnTarget : CharacterAnimation
{
    [SerializeField] private Transform toMove;
    [SerializeField] protected Renderer rnd;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float animationTime;

    private Vector2 startPosition;
    private Vector2 direction;

    private float curveIndex;
    private float curveDirection;
    private int baseSortingOrder;

    /// <summary>
    /// Play a Jump animation on the target.
    /// </summary>
    /// <param name="_targetPosition"></param>
    public override void Play(Vector2 _targetPosition)
    {
        startPosition = toMove.localPosition;
        direction = _targetPosition - new Vector2(toMove.position.x, toMove.position.y);
        curveDirection = 1;

        if (direction.y < 0)
        {
            baseSortingOrder = Mathf.RoundToInt(-1);
        }
        else
        {
            baseSortingOrder = Mathf.RoundToInt(1);
        }

        enabled = true;
    }

    public override void Stop()
    {
        
    }

    private void Update()
    {
        curveIndex += (Time.deltaTime * curveDirection) / animationTime;

        rnd.sortingOrder = baseSortingOrder; //TO DO: Trouver une meilleure solution pour éviter d'avoir le personnage qui s'affiche derrière/devant la target ou les personnages qui l'entoure.

        if (curveIndex > 1)
        {
            curveIndex = 1;
            curveDirection = -1;
        }
        else if(curveIndex < 0)
        {
            curveIndex = 0;
            enabled = false;
        }

        toMove.localPosition = startPosition + new Vector2(direction.x * curveIndex, direction.y * curveIndex + jumpCurve.Evaluate(curveIndex));
    }
}
