using System.Collections;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    private MonsterHealth monster;
    private int currentStack;
    private Coroutine burnRoutine;
    private void Awake()
    {
        monster = GetComponent<MonsterHealth>();
    }
    public void ApplyBurn(int damagePerSecond, int maxStack, float duration)
    {
        currentStack++;

        if (currentStack > maxStack) currentStack = maxStack;

        if (burnRoutine != null) StopCoroutine(burnRoutine);

        burnRoutine = StartCoroutine(BurnRoutine(damagePerSecond, duration));
    }

    private IEnumerator BurnRoutine(int damagePerSecond, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            monster.TakeDamage(damagePerSecond * currentStack);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        currentStack = 0;
        burnRoutine = null;
    }
}
