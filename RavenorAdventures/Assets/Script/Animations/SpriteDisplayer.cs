using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDisplayer : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    protected virtual void OnEnable()
    {
        SetSortingOrder();
    }

    private void Update()
    {
        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        if (canvas != null)
        {
            //canvas.sortingOrder = rnd.sortingOrder;
        }
    }
}
