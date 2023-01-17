using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RVN_TooltipManager : RVN_Singleton<RVN_TooltipManager>
{
    [SerializeField] private RectTransform canvasUsed;
    [SerializeField] private CanvasGroup tooltipGroup;
    [SerializeField] private RectTransform tooltipTransform;
    [SerializeField] private RectTransform tooltipDisplayTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;

    private RVN_Tooltip currentTooltip;

    public void ShowTooltip(RVN_Tooltip toShow, Vector2 position)
    {
        if(currentTooltip != toShow)
        {
            currentTooltip = toShow;

            tooltipText.text = currentTooltip.Text;
            tooltipGroup.alpha = 1;

            transform.position = position;

            Vector2 anchoredPosition = tooltipTransform.anchoredPosition;

            if (anchoredPosition.x + tooltipDisplayTransform.rect.width / 2 > canvasUsed.rect.width)
            {
                anchoredPosition.x = canvasUsed.rect.width - tooltipDisplayTransform.rect.width / 2;
            }
            else if(anchoredPosition.x - tooltipDisplayTransform.rect.width / 2 < 0)
            {
                anchoredPosition.x = tooltipDisplayTransform.rect.width / 2;
            }

            if(anchoredPosition.y + tooltipDisplayTransform.rect.height > canvasUsed.rect.height)
            {
                anchoredPosition.y = canvasUsed.rect.height - tooltipDisplayTransform.rect.height;
            }
            else if(anchoredPosition.y < 0)
            {
                anchoredPosition.y = 0;
            }

            tooltipTransform.anchoredPosition = anchoredPosition;
        }
    }

    public void HideTooltip(RVN_Tooltip toHide)
    {
        if(toHide == currentTooltip)
        {
            tooltipGroup.alpha = 0;

            currentTooltip = null;
        }
    }
}
