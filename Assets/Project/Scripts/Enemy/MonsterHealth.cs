using System;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    private int currentHp;
    private bool isDead;

    public event Action<MonsterHealth> OnDead;

    private void Awake()
    {
        currentHp = monsterData.maxHp;
    }
    public void Initialize(MonsterData data)
    {
        monsterData = data;
        currentHp = monsterData.maxHp;

        MonsterMover mover = GetComponent<MonsterMover>();
        if (mover != null) mover.Initialize(data);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            isDead = true;
            OnDead?.Invoke(this);
            Destroy(gameObject);  //임시
        }
    }
}