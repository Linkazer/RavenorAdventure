using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DiceResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image missImage;

    public void Display(int result, bool doesHit)
    {
        gameObject.SetActive(true);

        resultText.text = result.ToString();

        missImage.enabled = !doesHit;
    }
}
