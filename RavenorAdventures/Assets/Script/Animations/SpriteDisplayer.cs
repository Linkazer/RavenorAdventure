using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDisplayer : MonoBehaviour
{
    [SerializeField]
    protected Renderer rnd;
    public int offset = 0;
    [SerializeField]
    protected Renderer originRnd;
    [SerializeField] private Canvas canvas;

    protected Material mat;

    protected virtual void OnEnable()
    {
        if (rnd == null)
        {
            rnd = GetComponent<Renderer>();
            if (rnd == null)
            {
                enabled = false;
                return;
            }
        }

        mat = rnd.material;
        rnd.material = Instantiate(mat);

        SetSortingOrder();
    }

    private void Update()
    {
        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        if (originRnd == null)
        {
            rnd.sortingOrder = -Mathf.RoundToInt(transform.position.y * 5) + offset;
        }
        else
        {
            rnd.sortingOrder = originRnd.sortingOrder + offset;
        }

        if (canvas != null)
        {
            canvas.sortingOrder = rnd.sortingOrder;
        }
    }
}
