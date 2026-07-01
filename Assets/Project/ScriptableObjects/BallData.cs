using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "PurpleCow/Ball Data")]
public class BallData : ScriptableObject
{
    public BallController2D ballPrefab;
    public string ballName;
    public int damage = 8;
    public float speed = 12f;
    public float criticalChance = 0f;
    public float criticalDamageRate = 0.5f;

    [Header("고스트볼")]
    public bool isPiercing;

    [Header("레이저볼")]
    public bool isLaser;
    public int laserDamage;
    public float laserRowRange = 0.5f;

    [Header("파이어볼")]
    public bool isFire;
    public int burnDamagePerSecond;
    public int maxBurnStack;
    public float burnDuration;
}