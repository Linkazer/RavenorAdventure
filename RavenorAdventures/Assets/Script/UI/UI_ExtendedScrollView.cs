using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_ExtendedScrollView : ScrollRect, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InputActionReference actionMouseScroll;

    private bool swallowMouseWheelScrolls = true;
    private bool isMouseOver = false;

    private static Vector2 mouseScrollValue = new Vector2();

    protected override void Start()
    {
        actionMouseScroll = RVN_InputController.Instance.ScrollAction;

        base.Start(); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

        actionMouseScroll.action.performed += ScrollMouseInput;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;

        actionMouseScroll.action.performed -= ScrollMouseInput;
    }

    private void Update()
    {
        // Detect the mouse wheel and generate a scroll. This fixes the issue where Unity will prevent our ScrollRect
        // from receiving any mouse wheel messages if the mouse is over a raycast target (such as a button).
        if (isMouseOver && IsMouseWheelRolling())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.scrollDelta = new Vector2(0f, mouseScrollValue.y);

            swallowMouseWheelScrolls = false;
            OnScroll(pointerData);
            swallowMouseWheelScrolls = true;
        }
    }

    public override void OnScroll(PointerEventData data)
    {
        if (IsMouseWheelRolling() && swallowMouseWheelScrolls)
        {
            // Eat the scroll so that we don't get a double scroll when the mouse is over an image
        }
        else
        {
            // Amplify the mousewheel so that it matches the scroll sensitivity.
            if (data.scrollDelta.y < -Mathf.Epsilon)
                data.scrollDelta = new Vector2(0f, -scrollSensitivity);
            else if (data.scrollDelta.y > Mathf.Epsilon)
                data.scrollDelta = new Vector2(0f, scrollSensitivity);

            mouseScrollValue = Vector2.zero;

            base.OnScroll(data);
        }
    }

    private void ScrollMouseInput(InputAction.CallbackContext context)
    {
        mouseScrollValue = context.ReadValue<Vector2>();
    }

    private static bool IsMouseWheelRolling()
    {
        return mouseScrollValue != Vector2.zero;
    }
}
