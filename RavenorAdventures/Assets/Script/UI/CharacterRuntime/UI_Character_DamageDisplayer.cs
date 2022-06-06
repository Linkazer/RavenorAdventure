using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Character_DamageDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float displayDuration;

    [SerializeField] private List<UI_DiceResult> dicesResults = new List<UI_DiceResult>();

    private void OnEnable()
    {
        TimerManager.CreateGameTimer(displayDuration, () => Destroy(gameObject));
    }

    public void Display(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
    }

    public void DisplayDices(List<Dice> dices)
    {
        for(int i = 0; i < dicesResults.Count; i++)
        {
            if(i < dices.Count)
            {
                dicesResults[i].Display((int)dices[i].Result, dices[i].succeed);
            }
        }
    }
}
