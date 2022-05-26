using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterCanvas : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;

    public void SetCanvas(CharacterScriptable character)
    {
        canvasTransform.localPosition = new Vector2(0, character.UIHeight);
    }
}
