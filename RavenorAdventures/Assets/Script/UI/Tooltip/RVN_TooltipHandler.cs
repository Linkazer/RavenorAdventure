using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RVN_TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RVN_Tooltip tooltip;
    [SerializeField] private Transform tooltipPosition;
    [SerializeField] private Vector2 offset = new Vector2(0,5);

    private void Start()
    {
        if(tooltipPosition == null)
        {
            tooltipPosition = transform;
        }
    }

    public void ShowTooltip()
    {
        RVN_TooltipManager.Instance.ShowTooltip(tooltip, tooltipPosition.position + new Vector3(offset.x, offset.y, 0));
    }

    public void HideTooltip()
    {
        RVN_TooltipManager.Instance.HideTooltip(tooltip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}
