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
    [SerializeField] private UnityEvent<CPN_ClicHandler> OnMouseLeftDownOnObject;
    [SerializeField] private UnityEvent<Vector2> OnMouseRightDown;
    [SerializeField] private UnityEvent<CPN_ClicHandler> OnMouseRightDownOnObject;
    [SerializeField] private UnityEvent<Vector2> OnMoveCameraInput;
    [SerializeField] private UnityEvent<Vector2> OnMouseMiddleDown;
    [SerializeField] private UnityEvent<Vector2> OnMouseMiddleUp;
    [SerializeField] private UnityEvent<Vector2> OnMouseScroll;

    [Header("Inputs")]
    [SerializeField] private InputActionReference actionMouseMovementInput;
    [SerializeField] private InputActionReference actionMouseLeftClicInput;
    [SerializeField] private InputActionReference actionMouseRightClicInput;
    [SerializeField] private InputActionReference actionMoveCameraInput;
    [SerializeField] private InputActionReference actionMouseMiddle;
    [SerializeField] private InputActionReference actionMouseScroll;

    [Header("Datas")]
    [SerializeField] private Camera usedCamera;

    [SerializeField] private PlayerControl playerControl;

    [SerializeField] private EventSystem evtSyst;

    private RaycastHit2D mouseRaycast;

    private Vector2 mouseScreenPosition;
    public static Vector2 MouseScreenPosition => instance.mouseScreenPosition;

    private Vector2 mouseWorldPosition;
    public static Vector2 MousePosition => instance.mouseWorldPosition;

    private CPN_ClicHandler currentClicHandlerTouched;

    private Vector2 moveCameraDirection;

    private bool isMiddleMouseDown;

    public PlayerControl PlayerControl => playerControl;

    protected override void OnAwake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();

        actionMouseMovementInput.action.performed += UpdateMousePosition;

        actionMouseLeftClicInput.action.started += LeftMouseInput;

        actionMouseRightClicInput.action.started += RightMouseInput;

        actionMouseMiddle.action.started += MiddleMouseInputDown;

        actionMouseMiddle.action.canceled += MiddleMouseInputDown;

        actionMouseScroll.action.performed += ScrollMouseInput;
    }

    private void OnDisable()
    {
        playerControl.Disable();

        actionMouseMovementInput.action.performed -= UpdateMousePosition;

        actionMouseLeftClicInput.action.started -= LeftMouseInput;

        actionMouseRightClicInput.action.started -= RightMouseInput;

        actionMouseMiddle.action.performed -= MiddleMouseInputDown;

        actionMouseScroll.action.performed -= ScrollMouseInput;
    }

    private void Update()
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            mouseRaycast = GetMouseRaycast();

            OnMoveCameraInput?.Invoke(actionMoveCameraInput.action.ReadValue<Vector2>());

            OnMouseScroll?.Invoke(actionMouseScroll.action.ReadValue<Vector2>().normalized);
        }
    }

    private void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();

        mouseWorldPosition = usedCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void LeftMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnMouseLeftDown?.Invoke(mouseWorldPosition);

            if (currentClicHandlerTouched != null)
            {
                OnMouseLeftDownOnObject?.Invoke(currentClicHandlerTouched);
            }

            if (currentClicHandlerTouched != null)
            {
                currentClicHandlerTouched.MouseDown(0);
            }
        }
    }

    private void MiddleMouseInputDown(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            if(context.started)
            {
                isMiddleMouseDown = true;
                OnMouseMiddleDown?.Invoke(mouseWorldPosition);
            }
        }

        if (context.canceled && isMiddleMouseDown)
        {
            isMiddleMouseDown = false;
            OnMouseMiddleUp?.Invoke(mouseWorldPosition);
        }
    }

    private void RightMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            if (currentClicHandlerTouched != null)
            {
                OnMouseRightDownOnObject?.Invoke(currentClicHandlerTouched);
            }
            else
            {
                OnMouseRightDown?.Invoke(mouseWorldPosition);
            }
        }
    }

    private void ScrollMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnMouseScroll?.Invoke(context.ReadValue<Vector2>());
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
