using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float lifeTime = 0.7f;

    private float timer;

    public void Initialize(int damage)
    {
        damageText.text = damage.ToString();
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