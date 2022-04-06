using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_GridDisplayer : RVN_Singleton<RVN_GridDisplayer>
{
    public class DisplayNode
    {
        public SpriteRenderer renderer;
    }

    [SerializeField] private SpriteRenderer prefab;

    private DisplayNode[,] displayedGrid;

    private List<DisplayNode> currentDisplayedNodes = new List<DisplayNode>();

    public static void SetGridFeedback(List<Node> toFeedbacks, Color color)
    {
        instance.OnSetGridFeedback(toFeedbacks, color);
    }

    public static void UnsetGridFeedback()
    {
        instance.OnUnsetGridFeedback();
    }

    public void OnSetGrid(Node[,] grid, int xSize, int ySize)
    {
        displayedGrid = new DisplayNode[xSize, ySize];

        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                displayedGrid[i, j] = new DisplayNode();
                displayedGrid[i, j].renderer = Instantiate(prefab, gameObject.transform);
                displayedGrid[i, j].renderer.transform.position = grid[i, j].worldPosition;
            }
        }
    }

    public void OnSetGridFeedback(List<Node> toFeedbacks, Color color) // CODE REVIEW : Voir pour clean le script
    {
        OnUnsetGridFeedback(currentDisplayedNodes);

        currentDisplayedNodes = new List<DisplayNode>();

        for(int i = 0; i < toFeedbacks.Count; i++)
        {
            Node currentFeedbacked = toFeedbacks[i];

            displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.color = color;
            if (displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled && color.a == 0)
            {
                displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled = false;
            }
            else if (!displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled && color.a != 0)
            {
                displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled = true;
            }

            currentDisplayedNodes.Add(displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY]);
        }
    }

    public void OnUnsetGridFeedback()
    {
        OnUnsetGridFeedback(currentDisplayedNodes);
    }
    
    public void OnUnsetGridFeedback(List<DisplayNode> toFeedbacks) //CODE REVIEW : Voir pour le faire plus proprement
    {
        Color nullColor = Color.white;
        nullColor.a = 0;

        for(int i = 0; i < toFeedbacks.Count; i++)
        {
            toFeedbacks[i].renderer.color = nullColor;
            toFeedbacks[i].renderer.enabled = false;
        }
    }
}
