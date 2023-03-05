using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Character_NumberDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float displayDuration;

    private TimerManager.Timer displayTimer = null;

    private void OnEnable()
    {
        TimerManager.CreateGameTimer(displayDuration, () => Destroy(gameObject));
    }

    private void OnDisable()
    {
        if(displayTimer != null)
        {
            displayTimer.Stop();
        }
    }

    public void Display(int damageAmount, Color textColor)
    {
        damageText.color = textColor;
        damageText.text = damageAmount.ToString();
    }

    public void Display(string toDisplay, Color textColor)
    {
        damageText.color = textColor;
        damageText.text = toDisplay;
    }
}
