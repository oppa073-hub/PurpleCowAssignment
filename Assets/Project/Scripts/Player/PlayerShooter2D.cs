using UnityEngine;

public class PlayerShooter2D : MonoBehaviour
{
    [SerializeField] private BallController2D ball;
    [SerializeField] private TrajectoryPreview trajectoryPreview;

    private Camera mainCamera;
    private Vector2 aimDirection = Vector2.up;
    private bool ballReady = true;

    private void OnEnable()
    {
        ball.OnRecovered += FireBall;
    }

    private void OnDisable()
    {
        ball.OnRecovered -= FireBall;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Start()
    {
        FireBall();
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
    private void FireBall()
    {
        ball.transform.position = transform.position;
        ball.Launch(aimDirection);
    }
}
