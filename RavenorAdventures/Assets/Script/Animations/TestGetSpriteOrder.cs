using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetSpriteOrder : MonoBehaviour
{
    [SerializeField] private Renderer rnd;

    private void Update()
    {
        Debug.Log($"{gameObject} : {rnd.sortingOrder}");
    }
}
