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

    [SerializeField] private Vector4 cameraLimit;

    private Vector2 mouseDirection;

    private Vector2 mouseStartWorldPosition;
    private Vector2 mouseStartScreenPosition;
    private bool isMouseMoving;

    /// <summary>
    /// Calcul la position de la camera selon le Clic Molette.
    /// </summary>
    /// <param name="mousePosition">The mouse position.</param>
    public void MoveFromMiddleClic(Vector2 mousePosition)
    {
        mouseDirection = (mouseStartScreenPosition - RVN_InputController.MouseScreenPosition) * mouseSpeed * 0.001f;

        SetCameraPosition(mouseDirection + mouseStartWorldPosition);
    }

    /// <summary>
    /// D�finit le point de d�part du Clic Molette.
    /// </summary>
    /// <param name="mouseWorldPosition"></param>
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

    /// <summary>
    /// D�placement la cam�ra dans la direction voulue.
    /// </summary>
    /// <param name="direction">La direction que la camera.</param>
    public void MoveCamera(Vector2 direction)
    {
        if(direction != Vector2.zero && currentFocus != null)
        {
            currentFocus = null;
        }

        SetCameraPosition(cameraHandler.transform.position + new Vector3(direction.x, direction.y, 0) * speed * Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Focus la camera sur un personnage.
    /// </summary>
    /// <param name="character">Le personnage � focus.</param>
    public void SetCameraFocus(CPN_Character character)
    {
        currentFocus = character.transform;
    }

    /// <summary>
    /// Met la camera � la position voulue. (Prend en compte les limites de la map)
    /// </summary>
    /// <param name="position">La position voulue pour la camera.</param>
    public void SetCameraPosition(Vector2 position)
    {
        if(position.x < cameraLimit.x)
        {
            position.x = cameraLimit.x;
        }
        else if (position.x > cameraLimit.y)
        {
            position.x = cameraLimit.y;
        }

        if (position.y < cameraLimit.z)
        {
            position.y = cameraLimit.z;
        }
        else if (position.y > cameraLimit.w)
        {
            position.y = cameraLimit.w;
        }

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
