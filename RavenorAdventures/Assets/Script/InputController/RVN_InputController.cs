using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RVN_InputController : RVN_Singleton<RVN_InputController>
{
    [SerializeField] private UnityEvent<Vector2> OnMouseLeftDown;

    [SerializeField] private Camera usedCamera;

    [SerializeField] private PlayerControl playerControl;

    public static Vector2 MousePosition => Vector2.zero;// instance.usedCamera.ScreenToWorldPoint(Input.mousePosition);

    private PlayerInput playerInput;

    [SerializeField] private InputActionReference mouseAction;

     private void Awake()
     {
         playerControl = new PlayerControl();
     }

     private void OnEnable()
     {
         playerControl.Enable();
     }

     private void OnDisable()
     {
         playerControl.Disable();
     }

     private void Start()
     {
        mouseAction.action.started += MouseClic;
     }

     private void MouseClic(InputAction.CallbackContext context)
     {
         Debug.Log(context.ReadValue<Vector2>());
     }

     // Update is called once per frame
     void Update()
     {
         if (!EventSystem.current.IsPointerOverGameObject())
         {
             if (playerControl.BattleActionMap.MouseClic.triggered)
             {
                 OnMouseLeftDown?.Invoke(playerControl.BattleActionMap.MouseClic.ReadValue<Vector2>());
                 Debug.Log(playerControl.BattleActionMap.MouseClic.ReadValue<Vector2>());
             }
         }
     }
}
