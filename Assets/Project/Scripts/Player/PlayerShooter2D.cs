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
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mouseWorldPos - (Vector2)transform.position;

            if (dir.y > 0f) aimDirection = dir.normalized;
        }
        trajectoryPreview.DrawTrajectory(transform.position, aimDirection);
    }
}
