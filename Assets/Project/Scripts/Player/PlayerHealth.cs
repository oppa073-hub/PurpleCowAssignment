using UnityEngine;
using System;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;

    private int currentHp;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    public event Action<int, int> OnHpChanged;

    private void Awake()
    {
        currentHp = maxHp;
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        OnHpChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}