using UnityEngine;

public class TrajectoryPreview : MonoBehaviour
{
    [SerializeField] private float previewLength = 15f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector2 startPos, Vector2 direction)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos + direction.normalized * previewLength);
    }
}
