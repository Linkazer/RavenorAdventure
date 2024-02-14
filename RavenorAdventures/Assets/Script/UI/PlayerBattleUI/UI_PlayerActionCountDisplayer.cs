using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerActionCountDisplayer : MonoBehaviour
{
    [SerializeField] private Image availableAction;

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetAvailable(bool available)
    {
        availableAction.gameObject.SetActive(available);
    }
}
