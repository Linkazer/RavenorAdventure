using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueResponseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetResponse(DialogueResponse toSet)
    {
        text.text = toSet.Text.GetText();

        gameObject.SetActive(true);
    }

    public void UnsetResponse()
    {
        gameObject.SetActive(false);
    }
}
