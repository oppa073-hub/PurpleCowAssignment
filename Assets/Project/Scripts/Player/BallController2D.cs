using UnityEngine;
using System;
using System.Collections;
public class BallController2D : MonoBehaviour
{
    [SerializeField] private BallData ballData;

    private Collider2D ballCollider;
    private Rigidbody2D rb;
    private bool isMoving;
    private int currentDamage;
    private float skillChance;
    private float skillDuration;
    private float skillValue;
    private int skillSubDamage;
    private int skillStackCount;

    private float wallHitDamageBonusRate;
    private float nextHitDamageMultiplier = 1f; //패시브용
    private float criticalChance;
    private float criticalDamageRate;
    private bool isTemporaryBall;  //클러스트볼 용

    private Vector2 lastMoveDirection;

    public BallData BallData => ballData;

    public event Action<BallController2D> OnRecovered;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();
    }
    public void Initialize(BallData data, SkillLevelData levelData, float wallBonusRate, float critChance, float critDamageRate, bool temporaryBall = false)
    {
        ballData = data;
        currentDamage = levelData.damage;
        skillChance = levelData.chance;
        skillDuration = levelData.duration;
        skillValue = levelData.value;
        skillSubDamage = levelData.subDamage;
        skillStackCount = levelData.stackCount;
        wallHitDamageBonusRate = wallBonusRate;
        nextHitDamageMultiplier = 1f;
        criticalChance = critChance;
        criticalDamageRate = critDamageRate;
        isMoving = false;
        isTemporaryBall = temporaryBall;
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
    private void HitSameRowEnemies(float hitY)  //레이저볼이 같은행 레이저 발사
    {
        MonsterHealth[] monsters = FindObjectsByType<MonsterHealth>(FindObjectsSortMode.None);

        for (int i = 0; i < monsters.Length; i++)
        {
            float distanceY = Mathf.Abs(monsters[i].transform.position.y - hitY);

            if (distanceY <= ballData.laserRowRange)
            {
                monsters[i].TakeDamage(skillSubDamage);
            }
        }
    }
    private void SpawnClusterBall()
    {
        if (ballData.clusterBallData == null) return;

        Vector2 currentDirection = rb.linearVelocity.normalized;
        float randomAngle = UnityEngine.Random.Range(-ballData.clusterAngle, ballData.clusterAngle);
        Vector2 splitDirection = Quaternion.Euler(0f, 0f, randomAngle) * currentDirection;

        SkillLevelData levelData = new SkillLevelData();
        levelData.damage = skillSubDamage;

        BallController2D clusterBall = Instantiate(ballData.clusterBallData.ballPrefab, transform.position, Quaternion.identity);

        clusterBall.Initialize(ballData.clusterBallData, levelData, wallHitDamageBonusRate, criticalChance, criticalDamageRate, true);

        clusterBall.Launch(splitDirection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RecoverZone"))
        {
            isMoving = false;
            rb.linearVelocity = Vector2.zero;

            if (isTemporaryBall)
            {
                ObjectPoolManager.Instance.ReturnObject(gameObject);
                return;
            }

            OnRecovered?.Invoke(this);
        }
        if (other.CompareTag("Enemy"))
        {
            MonsterHealth monster = other.GetComponent<MonsterHealth>();

            if (monster != null)
                monster.TakeDamage(currentDamage);

            return;
        }
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

            if (ballData.isLaser)
            {
                HitSameRowEnemies(collision.transform.position.y);
            }
            if (ballData.isFire)
            {

                Burnable burnable = collision.collider.GetComponent<Burnable>();

                if (burnable != null)
                {
                    burnable.ApplyBurn(skillSubDamage, skillStackCount, skillDuration);
                }
            }
            if (ballData.isIce)
            {
                if (UnityEngine.Random.value < skillChance)
                {
                    Freezable freezable = collision.collider.GetComponent<Freezable>();

                    if (freezable != null) freezable.ApplyFreeze(skillDuration, skillValue);
                }
            }
            if (ballData.isCluster)
            {
                if (UnityEngine.Random.value < skillChance)
                {
                    SpawnClusterBall();
                }
            }
        }
    }
}