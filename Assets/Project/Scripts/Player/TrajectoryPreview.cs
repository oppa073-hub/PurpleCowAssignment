using UnityEngine;

public class TrajectoryPreview : MonoBehaviour
{
    [SerializeField] private float maxDistance = 30f;
    [SerializeField] private LayerMask hitLayer;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector2 startPos, Vector2 direction)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPos);

        Vector2 currentPosition = startPos;
        Vector2 currentDirection = direction.normalized;

        RaycastHit2D firstHit = Physics2D.Raycast(startPos, currentDirection, maxDistance, hitLayer);

        if (!firstHit)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, startPos + currentDirection * maxDistance);
            return;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(1, firstHit.point);

        if (firstHit.collider.CompareTag("Enemy")) return;

        if (!firstHit.collider.CompareTag("Wall")) return;

        Vector2 reflectDirection = Vector2.Reflect(currentDirection, firstHit.normal);
        Vector2 reflectStartPos = firstHit.point + reflectDirection * 0.05f;

        RaycastHit2D secondHit = Physics2D.Raycast(reflectStartPos, reflectDirection, maxDistance, hitLayer);

        lineRenderer.positionCount = 3;

        if (!secondHit)
        {
            lineRenderer.SetPosition(2, reflectStartPos + reflectDirection * maxDistance);
            return;
        }

        lineRenderer.SetPosition(2, secondHit.point);
    }
}
