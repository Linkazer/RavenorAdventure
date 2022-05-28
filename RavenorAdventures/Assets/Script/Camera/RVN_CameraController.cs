using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_CameraController : RVN_Singleton<RVN_CameraController>
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraHandler;
    [SerializeField] private float speed;

    [SerializeField] private bool enableEdgeCamera;

    public void MoveCamera(Vector2 direction)
    {
        cameraHandler.transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.unscaledDeltaTime;
    }

    private void Update()
    {
        if (enableEdgeCamera)
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
