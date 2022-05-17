using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of all action usable by characters during their turn.
/// 
/// Inherit of RVN_Component because we can have different type T of data that the CharacterAction will need.
/// </summary>
public abstract class CPN_CharacterAction : RVN_Component
{
    /// <summary>
    /// Check if the action can be used at the position.
    /// </summary>
    /// <param name="actionTargetPosition">The position where the action need to be used.</param>
    /// <returns>TRUE if the action can be made.</returns>
    public abstract bool IsActionUsable(Vector2 actionTargetPosition);
    /// <summary>
    /// Try to do the action.
    /// </summary>
    /// <param name="actionTargetPosition">The position where the action will occur.</param>
    /// <param name="callback">The callback to play at the end of the action.</param>
    public abstract void TryDoAction(Vector2 actionTargetPosition, Action callback);
    /// <summary>
    /// Display the action.
    /// </summary>
    /// <param name="actionTargetPosition">The position where the action will occur.</param>
    public abstract void DisplayAction(Vector2 actionTargetPosition);
    /// <summary>
    /// Hide the action.
    /// </summary>
    /// <param name="actionTargetPosition">The position where the action will occur.</param>
    public abstract void UndisplayAction(Vector2 actionTargetPosition);
    /// <summary>
    /// Reset all datat of the action.
    /// </summary>
    public abstract void ResetActionData();
}

public abstract class CPN_CharacterAction<T> : CPN_CharacterAction
{
    /// <summary>
    /// Set the data. Need to be override for every Component.
    /// </summary>
    /// <param name="toSet">The values of the data to set.</param>
    public abstract void SetData(T toSet);
}
