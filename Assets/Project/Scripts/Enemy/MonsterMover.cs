using UnityEngine;
using System.Collections;
public class MonsterMover : MonoBehaviour
{
    private float moveSpeed;
    private float moveDelayTime = 1f;

    public void Initialize(MonsterData data)
    {
        moveSpeed = data.moveSpeed;
    }
    void Update()
    {
        if (Time.time < moveDelayTime) return;
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }
}