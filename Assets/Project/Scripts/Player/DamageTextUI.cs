using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float lifeTime = 0.7f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color criticalColor = Color.red;

    private float timer;

    public void Initialize(int damage, bool isCritical)
    {
        damageText.text = damage.ToString();

        if (isCritical)
        {
            damageText.color = criticalColor;
        }
        else
        {
            damageText.color = normalColor;
        }

        timer = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.unscaledDeltaTime);

        timer += Time.unscaledDeltaTime;

        if (timer >= lifeTime)
        {
            Destroy(gameObject); //임시 테스트
        }
    }
}