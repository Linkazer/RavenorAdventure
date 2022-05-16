using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    public abstract void Play(Vector2 _targetPosition);
}
