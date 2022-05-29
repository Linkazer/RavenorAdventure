using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Character_DamageDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float displayDuration;

    public void Display(int damageAmount)
    {
        damageText.text = damageAmount.ToString();

        TimerManager.CreateGameTimer(displayDuration, () => Destroy(gameObject));
    }
}
