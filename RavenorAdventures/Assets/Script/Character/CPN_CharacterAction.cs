using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPN_CharacterAction : RVN_Component
{
    public abstract bool IsActionUsable(Vector2 actionTargetPosition);

    public abstract void TryDoAction(Vector2 actionTargetPosition, Action callback);

    public abstract void DisplayAction(Vector2 actionTargetPosition);

    public abstract void ResetActionData();
}

public abstract class CPN_CharacterAction<T> : CPN_CharacterAction
{
    public abstract void SetData(T toSet);
}
