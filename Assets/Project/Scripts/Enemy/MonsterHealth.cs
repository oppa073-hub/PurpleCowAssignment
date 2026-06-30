using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    private int currentHp;

    private void Awake()
    {
        currentHp = monsterData.maxHp;
    }
    public void Initialize(MonsterData data)
    {
        monsterData = data;
        currentHp = monsterData.maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0) Destroy(gameObject); //임시
    }
}