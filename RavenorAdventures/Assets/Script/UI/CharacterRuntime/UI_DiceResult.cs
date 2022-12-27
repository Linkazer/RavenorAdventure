using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DiceResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image succeedImageReroll;
    [SerializeField] private Image missImage;
    [SerializeField] private Image missImageReroll;
    [SerializeField] private Image bothImageReroll;

    public void Display(int result, bool doesHit, int rerolls)
    {
        succeedImageReroll.enabled = false;
        missImage.enabled = false;
        missImageReroll.enabled = false;
        bothImageReroll.enabled = false;

        gameObject.SetActive(true);

        resultText.text = result.ToString();

        missImage.enabled = !doesHit;
        
        switch(rerolls)
        {
            case 1:
                succeedImageReroll.enabled = true;
                break;
            case 2:
                missImageReroll.enabled = true;
                break;
            case 3:
                bothImageReroll.enabled = true;
                break;
        }
    }
}
