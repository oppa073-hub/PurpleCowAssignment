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
    private List<MonsterHealth> activeMonsters = new List<MonsterHealth>();
    private bool isSpawning = true;

    public int ActiveMonsterCount => activeMonsters.Count;
    public bool IsSpawning => isSpawning;

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
                activeMonsters.Add(monster);

                monster.OnDead += killProgressManager.HandleMonsterDead;
                monster.OnDead += HandleSpawnedMonsterDead;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void HandleSpawnedMonsterDead(MonsterHealth monster)  //사망처리
    {
        monster.OnDead -= HandleSpawnedMonsterDead;
        activeMonsters.Remove(monster);

        killProgressManager.CheckStageClear();
    }
    public void StopSpawn()
    {
        isSpawning = false;
    }
}