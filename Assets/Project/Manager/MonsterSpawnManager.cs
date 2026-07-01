using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [SerializeField] private GridSpawner gridSpawner;
    [SerializeField] private KillProgressManager killProgressManager;

    [SerializeField] private int minSpawnCount = 2;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private float spawnInterval = 3f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
            {
                yield return null;
                continue;
            }

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

            List<MonsterHealth> monsters = gridSpawner.SpawnRandomMonsters(spawnCount);

            foreach (MonsterHealth monster in monsters)
            {
                monster.OnDead += killProgressManager.HandleMonsterDead;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}