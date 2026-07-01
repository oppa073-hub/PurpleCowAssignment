using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;

    private int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}