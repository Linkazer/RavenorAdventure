using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Character_NumberDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float displayDuration;

    private void OnEnable()
    {
        TimerManager.CreateGameTimer(displayDuration, () => Destroy(gameObject));
    }

    public void Display(int damageAmount, Color textColor)
    {
        damageText.color = textColor;
        damageText.text = damageAmount.ToString();
    }
}
