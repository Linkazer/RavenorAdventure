using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RVN_TextTooltipHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Canvas canvasTocheck;
    [SerializeField] private RectTransform textBoxRectTransform;

    private int activeLinkedElement = -1;
    private string activeTooltipID = "";

    // Update is called once per frame
    void Update()
    {
        CheckForTooltip();
    }

    private void CheckForTooltip()
    {
        int interesectingLink = TMP_TextUtilities.FindIntersectingLink(textBox, RVN_InputController.MouseScreenPosition, null);

        if(interesectingLink != activeLinkedElement)
        {
            HideTooltip(activeTooltipID);
            activeTooltipID = "";
        }

        if(interesectingLink == -1)
        {
            return;
        }

        TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[interesectingLink];

        activeTooltipID = linkInfo.GetLinkID();

        ShowTooltip(activeTooltipID, RVN_InputController.MouseScreenPosition);

        activeLinkedElement = interesectingLink;
    }

    private void ShowTooltip(string tooltipId, Vector2 position)
    {
        RVN_TooltipManager.Instance.ShowTooltip(RVN_TextTooltipManager.GetTooltipOfID(tooltipId), position);
    }

    private void HideTooltip(string tooltipId)
    {
        RVN_TooltipManager.Instance.HideTooltip(RVN_TextTooltipManager.GetTooltipOfID(tooltipId));
    }
}
