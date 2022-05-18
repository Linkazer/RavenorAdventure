using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RVN_InputController : RVN_Singleton<RVN_InputController>
{

    [Header("Events")]
    [SerializeField] private UnityEvent<Vector2> OnMouseLeftDown;

    [Header("Inputs")]
    [SerializeField] private InputActionReference mouseMovementInput;
    [SerializeField] private InputActionReference mouseLeftClicInput;

    [Header("Datas")]
    [SerializeField] private Camera usedCamera;

    [SerializeField] private PlayerControl playerControl;

    private RaycastHit2D mouseRaycast;

    private Vector2 mouseWorldPosition;
    public static Vector2 MousePosition => instance.mouseWorldPosition;

    private CPN_ClicHandler currentClicHandlerTouched;

    protected override void OnAwake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();

        mouseMovementInput.action.performed += UpdateMousePosition;

        mouseLeftClicInput.action.started += LeftMouseInput;
    }

    private void OnDisable()
    {
        playerControl.Disable();

        mouseMovementInput.action.performed -= UpdateMousePosition;

        mouseLeftClicInput.action.started -= LeftMouseInput;
    }

    private void Update()
    {
        mouseRaycast = GetMouseRaycast();
    }

    private void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mouseWorldPosition = usedCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    private void LeftMouseInput(InputAction.CallbackContext context)
    {
        OnMouseLeftDown?.Invoke(mouseWorldPosition);

        if(currentClicHandlerTouched != null)
        {
            currentClicHandlerTouched.MouseDown(0);
        }
    }

    private RaycastHit2D GetMouseRaycast() //CODE REVIEW : Voir si on peut améliorer ça (au niveau du ClicHandler)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        CPN_ClicHandler lastClicHandler = currentClicHandlerTouched;

        if (hit.transform != null && hit.transform.gameObject.GetComponent<CPN_ClicHandler>() != null)
        {
            currentClicHandlerTouched = hit.transform.gameObject.GetComponent<CPN_ClicHandler>();
        }
        else if(currentClicHandlerTouched != null)
        {
            currentClicHandlerTouched = null;
        }

        if(currentClicHandlerTouched != lastClicHandler)
        {
            if (lastClicHandler != null)
            {
                lastClicHandler.MouseExit();
            }

            if(currentClicHandlerTouched != null)
            {
                currentClicHandlerTouched.MouseEnter();
            }
        }

        return hit;
    }
}
