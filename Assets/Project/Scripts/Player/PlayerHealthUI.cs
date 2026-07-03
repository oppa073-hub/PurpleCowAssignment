using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image hpImg;
    [SerializeField] private TMP_Text hpText;
    private void OnEnable()
    {
        playerHealth.OnHpChanged += UpdateHpUI;
    }

    private void OnDisable()
    {
        playerHealth.OnHpChanged -= UpdateHpUI;
    }

    private void Start()
    {
        UpdateHpUI(playerHealth.CurrentHp, playerHealth.MaxHp);
    }

    private void UpdateHpUI(int currentHp, int maxHp)
    {
        hpImg.fillAmount = (float)currentHp / maxHp;
        hpText.text = currentHp.ToString();
    }
}
