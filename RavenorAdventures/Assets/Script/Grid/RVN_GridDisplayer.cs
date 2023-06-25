using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_GridDisplayer : RVN_Singleton<RVN_GridDisplayer>
{
    [SerializeField] private GridNodeDisplay prefab;

    private GridNodeDisplay[,] displayedGrid = null;

    private List<GridNodeDisplay> currentDisplayedNodes = new List<GridNodeDisplay>();

    /// <summary>
    /// Set every wanted nodes at the color wanted.
    /// </summary>
    /// <param name="toFeedbacks">The nodes to set.</param>
    /// <param name="color">The color to set.</param>
    public static void SetGridFeedback(List<Node> toFeedbacks, Color color)
    {
        instance.OnSetGridFeedback(toFeedbacks, color);
    }

    /// <summary>
    /// Unset the color of every nodes.
    /// </summary>
    public static void UnsetGridFeedback()
    {
        instance.OnUnsetGridFeedback();
    }

    /// <summary>
    /// Create every node renderer.
    /// </summary>
    /// <param name="grid">An array of all the nodes</param>
    /// <param name="xSize">The size of the grid on the X axis.</param>
    /// <param name="ySize">The size of the grid on the Y axis</param>
    public void OnSetGrid(Node[,] grid, int xSize, int ySize)
    {
        if (displayedGrid == null)
        {
            displayedGrid = new GridNodeDisplay[xSize, ySize];

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    if (displayedGrid[i, j] == null)
                    {
                        displayedGrid[i, j] = Instantiate(prefab, gameObject.transform);
                        displayedGrid[i, j].transform.position = grid[i, j].worldPosition;
                        displayedGrid[i, j].node = grid[i, j];
                        displayedGrid[i, j].node.testName = $"Grid [{i},{j}]";
                        displayedGrid[i, j].gameObject.name = $"Grid [{i},{j}]";
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set every wanted nodes at the color wanted.
    /// </summary>
    /// <param name="toFeedbacks">The nodes to set.</param>
    /// <param name="color">The color to set.</param>
    public void OnSetGridFeedback(List<Node> toFeedbacks, Color color) // CODE REVIEW : Voir pour clean le script
    {
        //OnUnsetGridFeedback(currentDisplayedNodes);

        for(int i = 0; i < toFeedbacks.Count; i++)
        {
            Node currentFeedbacked = toFeedbacks[i];

            displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].SetColor(color);

            /*displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.color = color;
            if (displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled && color.a == 0)
            {
                displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled = false;
            }
            else if (!displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled && color.a != 0)
            {
                displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY].renderer.enabled = true;
            }*/

            currentDisplayedNodes.Add(displayedGrid[currentFeedbacked.gridX, currentFeedbacked.gridY]);
        }
    }

    /// <summary>
    /// Unset the color of every nodes.
    /// </summary>
    public void OnUnsetGridFeedback()
    {
        OnUnsetGridFeedback(new List<GridNodeDisplay>(currentDisplayedNodes));

        currentDisplayedNodes = new List<GridNodeDisplay>();
    }
    
    /// <summary>
    /// Unset the color of a list of Node.
    /// </summary>
    /// <param name="toFeedbacks">The list of node to reset.</param>
    public void OnUnsetGridFeedback(List<GridNodeDisplay> toFeedbacks) //CODE REVIEW : Voir pour le faire plus proprement
    {
        Color nullColor = new Color(0, 0, 0, 0);

        for(int i = 0; i < toFeedbacks.Count; i++)
        {
            toFeedbacks[i].SetColor(nullColor);

            currentDisplayedNodes.Remove(toFeedbacks[i]);
        }
    }
}
