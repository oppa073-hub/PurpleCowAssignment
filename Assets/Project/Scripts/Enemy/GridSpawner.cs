using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private List<MonsterData> monsterPool = new List<MonsterData>();

    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 6;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 cellSize = new Vector2(1.5f, 1.5f);

    public List<MonsterHealth> SpawnRandomMonsters(int count)
    {
        List<MonsterHealth> spawnedMonsters = new List<MonsterHealth>();
        bool[,] occupied = new bool[rows, columns];

        for (int i = 0; i < count; i++)
        {
            MonsterData data = monsterPool[Random.Range(0, monsterPool.Count)];

            bool spawned = TrySpawnMonster(data, occupied, out MonsterHealth monster);

            if (spawned) spawnedMonsters.Add(monster);
        }

        return spawnedMonsters;
    }

    private bool TrySpawnMonster(MonsterData data, bool[,] occupied, out MonsterHealth monster)
    {
        monster = null;

        List<Vector2Int> cells = new List<Vector2Int>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (CanPlaceMonster(row, col, data, occupied)) cells.Add(new Vector2Int(row, col));
            }
        }

        if (cells.Count <= 0) return false;

        int randomIndex = Random.Range(0, cells.Count);
        Vector2Int selectedCell = cells[randomIndex];

        SetOccupied(selectedCell.x, selectedCell.y, data, occupied);

        monster = SpawnMonster(selectedCell.x, selectedCell.y, data);
        return true;
    }

    private bool CanPlaceMonster(int row, int column, MonsterData data, bool[,] occupied)
    {
        if (row + data.gridHeight > rows) return false;

        if (column + data.gridWidth > columns) return false;

        for (int r = row; r < row + data.gridHeight; r++)
        {
            for (int c = column; c < column + data.gridWidth; c++)
            {
                if (occupied[r, c]) return false;
            }
        }

        return true;
    }

    private void SetOccupied(int row, int column, MonsterData data, bool[,] occupied)
    {
        for (int r = row; r < row + data.gridHeight; r++)
        {
            for (int c = column; c < column + data.gridWidth; c++)
            {
                occupied[r, c] = true;
            }
        }
    }

    private MonsterHealth SpawnMonster(int row, int column, MonsterData data)
    {
        Vector2 spawnPos = GetMonsterSpawnPosition(row, column, data);

        GameObject monsterObj = ObjectPoolManager.Instance.GetObject(data.monsterPrefab.gameObject, spawnPos, Quaternion.identity);

        MonsterHealth monster = monsterObj.GetComponent<MonsterHealth>();
        monster.Initialize(data);

        return monster;
    }

    private Vector2 GetMonsterSpawnPosition(int row, int column, MonsterData data)
    {
        Vector2 firstCellPos = GetCellPosition(row, column);

        Vector2 offset = new Vector2((data.gridWidth - 1) * cellSize.x * 0.5f, -(data.gridHeight - 1) * cellSize.y * 0.5f);

        return firstCellPos + offset;
    }

    private Vector2 GetCellPosition(int row, int column)
    {
        return startPosition + new Vector2(column * cellSize.x,-row * cellSize.y);
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