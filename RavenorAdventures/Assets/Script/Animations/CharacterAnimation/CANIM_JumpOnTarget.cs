using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CANIM_JumpOnTarget : CharacterAnimation
{
    [SerializeField] private Transform toMove;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpSpeed;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 direction;

    private float curveIndex;
    private float curveDirection;

    public override void Play(Vector2 _targetPosition)
    {
        startPosition = toMove.localPosition;
        targetPosition = _targetPosition;
        direction = targetPosition - startPosition;
        curveDirection = 1;

        enabled = true;
    }

    private void Update()
    {
        curveIndex += Time.deltaTime * curveDirection;

        if(curveIndex > 1)
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
