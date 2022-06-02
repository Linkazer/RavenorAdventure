using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_CameraController : RVN_Singleton<RVN_CameraController>
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraHandler;
    [SerializeField] private float speed;
    [SerializeField] private float mouseSpeed;

    [SerializeField] private bool enableEdgeCamera;

    [SerializeField] private Transform currentFocus;

    private Vector2 mouseDirection;

    private Vector2 mouseStartWorldPosition;
    private Vector2 mouseStartScreenPosition;
    private bool isMouseMoving;

    public void MoveFromMiddleClic(Vector2 mousePosition)
    {
        Vector2 wantedPos = (mouseStartScreenPosition - RVN_InputController.MouseScreenPosition) * mouseSpeed * 0.001f;

        SetCameraPosition(wantedPos + mouseStartWorldPosition);
    }

    public void StartMoveFromMiddleClic(Vector2 mouseWorldPosition)
    {
        if(currentFocus != null)
        {
            currentFocus = null;
        }

        isMouseMoving = true;
        mouseStartScreenPosition = RVN_InputController.MouseScreenPosition;
        mouseStartWorldPosition = cameraHandler.transform.position;
    }

    public void EndMoveFromMiddleClic()
    {
        isMouseMoving = false;
    }

    public void MoveCamera(Vector2 direction)
    {
        if(direction != Vector2.zero && currentFocus != null)
        {
            currentFocus = null;
        }

        cameraHandler.transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.unscaledDeltaTime;
    }

    public void SetCameraFocus(CPN_Character character)
    {
        currentFocus = character.transform;
    }

    public void SetCameraPosition(Vector2 position)
    {
        cameraHandler.transform.position = position;
    }

    private void Update()
    {
        if(currentFocus != null)
        {
            SetCameraPosition(currentFocus.position);
        }

        if(isMouseMoving)
        {
            MoveFromMiddleClic(RVN_InputController.MousePosition);
        }
        else if (enableEdgeCamera)
        {
            if (RVN_InputController.MouseScreenPosition.x < 50)
            {
                MoveCamera(new Vector2(-1, 0));
            }
            else if (RVN_InputController.MouseScreenPosition.x > Screen.width - 50)
            {
                MoveCamera(new Vector2(1, 0));
            }

            if (RVN_InputController.MouseScreenPosition.y < 50)
            {
                MoveCamera(new Vector2(0, -1));
            }
            else if (RVN_InputController.MouseScreenPosition.y > Screen.height - 50)
            {
                MoveCamera(new Vector2(0, 1));
            }
        }
    }
}
