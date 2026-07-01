using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private List<MonsterData> monsterPool = new List<MonsterData>();

    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 6;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 cellSize = new Vector2(1.5f, 1.5f);


    public List<MonsterHealth> SpawnRandomMonsters(int count)
    {
        List<MonsterHealth> spawnedMonsters = new();
        List<Vector2Int> cells = new();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                cells.Add(new Vector2Int(row, col));
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (cells.Count <= 0) break;

            int randomIndex = Random.Range(0, cells.Count);
            Vector2Int cell = cells[randomIndex];
            cells.RemoveAt(randomIndex);

            MonsterHealth monster = SpawnMonster(cell.x, cell.y);
            spawnedMonsters.Add(monster);
        }

        return spawnedMonsters;
    }
    private MonsterHealth SpawnMonster(int row, int column)  
    {
        Vector2 spawnPos = GetCellPosition(row, column);

        MonsterData data = monsterPool[Random.Range(0, monsterPool.Count)];

        GameObject monsterObj = ObjectPoolManager.Instance.GetObject(data.monsterPrefab.gameObject, spawnPos, Quaternion.identity);
        MonsterHealth monster = monsterObj.GetComponent<MonsterHealth>();

        monster.Initialize(data);

        return monster;
    }
    private Vector2 GetCellPosition(int row, int column)  
    {
        return startPosition + new Vector2(
            column * cellSize.x,
            -row * cellSize.y
        );
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = GetCellPosition(row, col);
                Gizmos.DrawWireCube(pos, cellSize);
            }
        }
    }
}
