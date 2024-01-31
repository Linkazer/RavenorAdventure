using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CANIM_JumpOnTarget : CharacterAnimation<LaunchedSpellData>
{
    [SerializeField] private Transform toMove;
    [SerializeField] protected SortingGroup sortingGroup;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float animationTime;

    [SerializeField] private int sortingOrderOffset;


    private Vector2 startPosition;
    private Vector2 direction;

    private float curveIndex;
    private float curveDirection;

    private int baseSortingOrder;
    private int startSortingOrder;

    /// <summary>
    /// Play a Jump animation on the target.
    /// </summary>
    /// <param name="_targetPosition"></param>
    public override void Play(LaunchedSpellData launchedSpell)
    {
        startPosition = toMove.localPosition;
        direction = launchedSpell.targetNode.worldPosition - toMove.position;
        curveDirection = 1;

        startSortingOrder = sortingGroup.sortingOrder;

        if(direction.y >= 0)
        {
            baseSortingOrder = startSortingOrder;
        }
        else
        {
            baseSortingOrder = startSortingOrder += sortingOrderOffset;
        }

        enabled = true;
    }

    public override void Stop()
    {
        sortingGroup.sortingOrder = startSortingOrder;

        curveIndex = 0;
        enabled = false;
    }

    public override void End()
    {
        Stop();

        toMove.localPosition = startPosition;

        if (currentAnimation.linkedAnimation != "")
        {
            currentAnimation.endCallback?.Invoke();
        }
    }

    private void Update()
    {
        curveIndex += (Time.deltaTime * curveDirection) / animationTime;

        sortingGroup.sortingOrder = baseSortingOrder;

        if (curveIndex > 1)
        {
            curveIndex = 1;
            curveDirection = -1;

            animationHandler.UE_PlayAnimationSound();
        }
        else if(curveIndex < 0)
        {
            End();
        }

        toMove.localPosition = startPosition + new Vector2(direction.x * curveIndex, direction.y * curveIndex + jumpCurve.Evaluate(curveIndex));
    }
}
