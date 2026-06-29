using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "PurpleCow/Ball Data")]
public class BallData : ScriptableObject
{
    public string ballName;
    public int damage = 8;
    public float speed = 12f;
    public float criticalChance = 0f;
    public float criticalDamageRate = 0.5f;
}