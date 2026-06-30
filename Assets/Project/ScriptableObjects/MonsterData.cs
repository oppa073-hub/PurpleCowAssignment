using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "PurpleCow/Monster Data")]
public class MonsterData : ScriptableObject
{
    public MonsterHealth monsterPrefab;
    public string monsterName;
    public int maxHp = 30;
    public float moveSpeed = 1f;
}