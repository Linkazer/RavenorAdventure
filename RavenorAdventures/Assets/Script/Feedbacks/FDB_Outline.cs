using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDB_Outline : MonoBehaviour
{
    [SerializeField, Tooltip("The color that will be changed")] private Color targetColor;
    [SerializeField] private Color outlineColor;

    [SerializeField] private SpriteRenderer renderer;

    public void SetOutline()
    {
        renderer.material.SetColor("_Color1in", targetColor);
        renderer.material.SetColor("_Color1out", outlineColor);
    }

    public void UnsetOutline()
    {
        renderer.material.SetColor("_Color1in", Color.white);
        renderer.material.SetColor("_Color1out", Color.white);
    }
}
