using UnityEngine;
using System;
public class BallController2D : MonoBehaviour
{
    [SerializeField] private BallData ballData;

    private Rigidbody2D rb;
    private bool isMoving;
    private int currentDamage;

    private float wallHitDamageBonusRate;
    private float nextHitDamageMultiplier = 1f; //패시브용
    private float criticalChance;
    private float criticalDamageRate;

    public event Action<BallController2D> OnRecovered;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(BallData data, int damage, float wallBonusRate, float critChance, float critDamageRate)
    {
        ballData = data;
        currentDamage = damage;
        wallHitDamageBonusRate = wallBonusRate;
        nextHitDamageMultiplier = 1f;
        criticalChance = critChance;
        criticalDamageRate = critDamageRate;
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
        if (collision.collider.CompareTag("Wall"))
        {
            nextHitDamageMultiplier += wallHitDamageBonusRate; return;
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            MonsterHealth monster = collision.collider.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                int finalDamage = Mathf.RoundToInt(currentDamage * nextHitDamageMultiplier);
                
                if (UnityEngine.Random.value < criticalChance) //크리티컬 적용
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * (1f + criticalDamageRate));
                }

                monster.TakeDamage(finalDamage);
            }

            nextHitDamageMultiplier = 1f;
        }
    }
}