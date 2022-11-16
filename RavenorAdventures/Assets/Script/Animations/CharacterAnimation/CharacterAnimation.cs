using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    public abstract void Play(Vector2 _targetPosition);

    public abstract void Stop();

    public virtual void SetCharacter(CharacterScriptable_Battle character)
    {

    }
}
