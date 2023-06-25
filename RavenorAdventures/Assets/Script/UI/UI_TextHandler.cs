using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TextHandler : MonoBehaviour
{
    [SerializeField] private RVN_Text text;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Awake()
    {
        string toDisplay = text.GetText();

        if(toDisplay != "")
        {
            textMesh.text = toDisplay;
        }
    }
}
