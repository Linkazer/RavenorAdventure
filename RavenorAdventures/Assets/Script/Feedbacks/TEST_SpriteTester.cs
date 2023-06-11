using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SpriteTester : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rnd;

    [ContextMenu("Debug Order")]
    public void DebugOrder()
    {
        Debug.Log(rnd.sortingOrder);
    }
}
