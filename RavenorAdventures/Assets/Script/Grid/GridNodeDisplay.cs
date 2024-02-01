using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodeDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRenderer;

    private bool isDisplayed = false;

    private Color currentColor = new Color(0, 0, 0, 0);
    [SerializeField] private List<Color> colorsToSet = new List<Color>();
    [SerializeField] private List<Color> lastColors;

    public Node node;

    public void SetColor(Color colorToSet)
    {
        colorsToSet = new List<Color>() { colorToSet };// .Add(colorToSet);

        if(!enabled)
        {
            enabled = true;
        }
    }

    private void LateUpdate()
    {
        currentColor = GetColorToSet();

        if(currentColor.a != 0)
        {
            sprRenderer.enabled = true;
            sprRenderer.color = currentColor;
        }
        else
        {
            sprRenderer.enabled = false;
        }

        lastColors = new List<Color>(colorsToSet);
        colorsToSet.Clear();
        enabled = false;
    }

    private Color GetColorToSet()
    {
        Color toReturn = new Color(0, 0, 0, 0);

        for(int i = colorsToSet.Count - 1; i >= 0; i--)
        {
            if (colorsToSet[i].a != 0)
            {
                toReturn = colorsToSet[i];
                break;
            }
        }

        return toReturn;
    }
}
