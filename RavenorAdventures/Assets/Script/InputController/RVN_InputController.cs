using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RVN_InputController : RVN_Singleton<RVN_InputController>
{
    [SerializeField] private UnityEvent<Vector2> OnMouseLeftDown;

    [SerializeField] private Camera usedCamera;

    public static Vector2 MousePosition => instance.usedCamera.ScreenToWorldPoint(Input.mousePosition);

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftDown?.Invoke(MousePosition);
            }
        }
    }
}
