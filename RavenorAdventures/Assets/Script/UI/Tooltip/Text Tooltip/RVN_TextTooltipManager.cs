using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_TextTooltipManager : RVN_Singleton<RVN_TextTooltipManager>
{
    [Serializable]
    private struct TooltipData
    {
        public string ID;
        public RVN_Tooltip tooltip;
    }

    [SerializeField] private TooltipData[] tooltips;

    private Dictionary<string, RVN_Tooltip> tooltipByID = new Dictionary<string, RVN_Tooltip>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(TooltipData data in tooltips)
        {
            if(!tooltipByID.ContainsKey(data.ID))
            {
                tooltipByID.Add(data.ID, data.tooltip);
            }
            else
            {
                Debug.LogError($"{data.ID} Tooltip exist multiple times.");
            }
        }
    }

    public static RVN_Tooltip GetTooltipOfID(string id)
    {
        if(instance.tooltipByID.ContainsKey(id))
        {
            return instance.tooltipByID[id];
        }

        return null;
    }
}
