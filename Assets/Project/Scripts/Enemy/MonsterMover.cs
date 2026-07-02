using UnityEngine;
using System.Collections;
public class MonsterMover : MonoBehaviour
{
    private float moveDelayTime = 1f;
    private float originMoveSpeed;  //아이스볼
    private float currentMoveSpeed;

    public void Initialize(MonsterData data)
    {
        currentMoveSpeed = data.moveSpeed;
        originMoveSpeed = data.moveSpeed;
    }
    void Update()
    {
        if (Time.time < moveDelayTime) return;
        transform.Translate(Vector2.down * currentMoveSpeed * Time.deltaTime);
    }
    public void ApplySlow(float slowRate)
    {
        currentMoveSpeed = originMoveSpeed * (1f - slowRate);
        Debug.Log("감속 적용: " + currentMoveSpeed);
    }

    public void ResetSpeed()
    {
        currentMoveSpeed = originMoveSpeed;
        Debug.Log("감속 해제");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("FailZone")) return;

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null) playerHealth.TakeDamage(1);

        Destroy(gameObject);
    }
}