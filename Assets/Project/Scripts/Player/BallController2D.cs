using UnityEngine;
using System;
public class BallController2D : MonoBehaviour
{
    [SerializeField] private BallData ballData;

    private Rigidbody2D rb;
    private bool isMoving;

    public event Action<BallController2D> OnRecovered;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(BallData data)
    {
        ballData = data;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Launch(Vector2.up);  //임시 테스트
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        if (rb.linearVelocity.sqrMagnitude < 0.01f) return;

        rb.linearVelocity = rb.linearVelocity.normalized * ballData.speed;
    }

    public void Launch(Vector2 direction)
    {
        isMoving = true;
        rb.linearVelocity = direction.normalized * ballData.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("RecoverZone")) return;

        isMoving = false;
        rb.linearVelocity = Vector2.zero;
        OnRecovered?.Invoke(this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy")) return;

        MonsterHealth monster = collision.collider.GetComponent<MonsterHealth>();

        if (monster != null)
            monster.TakeDamage(ballData.damage);
    }
}