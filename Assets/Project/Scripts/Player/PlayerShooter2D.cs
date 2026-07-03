using UnityEngine;

public class PlayerShooter2D : MonoBehaviour
{
    [SerializeField] private TrajectoryPreview trajectoryPreview;

    private Camera mainCamera;
    private Vector2 aimDirection = Vector2.up;

    public Vector2 AimDirection => aimDirection;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector2 inputPosition;
        bool isPressed = false;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            isPressed = true;
        }
        else
        {
            inputPosition = Vector2.zero;
        }
#else
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        inputPosition = touch.position;
        isPressed = true;
    }
    else
    {
        inputPosition = Vector2.zero;
    }
#endif

        if (isPressed)
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(inputPosition);
            Vector2 dir = worldPos - (Vector2)transform.position;

            if (dir.y > 0f)
                aimDirection = dir.normalized;
        }

        trajectoryPreview.DrawTrajectory(transform.position, aimDirection);
    }
}
