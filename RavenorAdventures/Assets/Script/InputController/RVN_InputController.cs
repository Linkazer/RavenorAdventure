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
    [SerializeField] private UnityEvent<Vector2> OnMoveCameraInput;
    [SerializeField] private UnityEvent<Vector2> OnMouseMiddleClic;
    [SerializeField] private UnityEvent<Vector2> OnMouseMiddleHold;

    [Header("Inputs")]
    [SerializeField] private InputActionReference actionMouseMovementInput;
    [SerializeField] private InputActionReference actionMouseLeftClicInput;
    [SerializeField] private InputActionReference actionMoveCameraInput;
    [SerializeField] private InputActionReference actionMouseMiddle;

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

    protected override void OnAwake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();

        actionMouseMovementInput.action.performed += UpdateMousePosition;

        actionMouseLeftClicInput.action.started += LeftMouseInput;

        actionMouseMiddle.action.performed += MiddleMouseInput;
    }

    private void OnDisable()
    {
        playerControl.Disable();

        actionMouseMovementInput.action.performed -= UpdateMousePosition;

        actionMouseLeftClicInput.action.started -= LeftMouseInput;

        actionMouseMiddle.action.performed -= MiddleMouseInput;
    }

    private void Update()
    {
        mouseRaycast = GetMouseRaycast();

        OnMoveCameraInput?.Invoke(actionMoveCameraInput.action.ReadValue<Vector2>());
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
                currentClicHandlerTouched.MouseDown(0);
            }
        }
    }

    private void MiddleMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            if(context.started)
            {
                OnMouseMiddleClic?.Invoke(mouseWorldPosition);
            }

            Debug.Log("Hold input ?");

            OnMouseMiddleHold?.Invoke(mouseWorldPosition);
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
