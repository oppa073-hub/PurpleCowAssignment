using UnityEngine;
using System.Collections;

public class Freezable : MonoBehaviour
{
    private MonsterMover mover;
    private Coroutine freezeRoutine;

    private void Awake()
    {
        mover = GetComponent<MonsterMover>();
    }
    public void ApplyFreeze(float duration, float slowRate)
    {
        if (freezeRoutine != null) StopCoroutine(freezeRoutine);

        freezeRoutine = StartCoroutine(FreezeRoutine(duration, slowRate));
    }

    private IEnumerator FreezeRoutine(float duration, float slowRate)
    {
        mover.ApplySlow(slowRate);

        yield return new WaitForSeconds(duration);

        mover.ResetSpeed();
        freezeRoutine = null;
    }
}
