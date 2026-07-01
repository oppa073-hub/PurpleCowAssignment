using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    [SerializeField] private DamageTextUI damageTextPrefab;
    [SerializeField] private Canvas canvas;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    public void ShowDamage(int damage, Vector3 worldPosition)
    {
        DamageTextUI damageText = Instantiate(damageTextPrefab, canvas.transform);

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        damageText.transform.position = screenPosition;

        damageText.Initialize(damage);
    }
}