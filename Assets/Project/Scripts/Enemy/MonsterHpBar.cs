using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private MonsterHealth monsterHealth;
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        monsterHealth.OnHpChanged += UpdateHpBar;
    }

    private void OnDisable()
    {
        monsterHealth.OnHpChanged -= UpdateHpBar;
    }
    private void Start()
    {
        UpdateHpBar(monsterHealth.CurrentHp, monsterHealth.MaxHp);
    }

    private void UpdateHpBar(int currentHp, int maxHp)
    {
        fillImage.fillAmount = (float)currentHp / maxHp;
    }
}