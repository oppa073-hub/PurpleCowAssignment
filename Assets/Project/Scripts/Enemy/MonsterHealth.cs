using System;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private float explosionRadius = 1.5f;  //폭발 패시브 범위

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
        isDead = false;
        OnDead = null;
        MonsterMover mover = GetComponent<MonsterMover>();
        if (mover != null) mover.Initialize(data);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        DamageTextManager.Instance.ShowDamage(damage, transform.position);

        if (currentHp <= 0)
        {
            isDead = true;
            ApplyLastMatchExplosion();
            OnDead?.Invoke(this);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }

    private void ApplyLastMatchExplosion()
    {
        var playerSkillInventory = FindFirstObjectByType<PlayerSkillInventory>();
        PassiveSkillData lastMatch = playerSkillInventory.GetPassiveSkill(PassiveType.LastMatch);

        if (lastMatch == null) return;

        int level = playerSkillInventory.GetLevel(lastMatch);
        int damage = Mathf.RoundToInt(lastMatch.levels[level - 1].value);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].CompareTag("Enemy")) continue;
            if (hits[i].gameObject == gameObject) continue;  //자기 뺴고

            MonsterHealth monster = hits[i].GetComponent<MonsterHealth>();

            if (monster != null) monster.TakeDamage(damage);
        }
    }
}