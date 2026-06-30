using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private List<MonsterData> monsterPool = new List<MonsterData>();

    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 6;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 cellSize = new Vector2(1.5f, 1.5f);

    private bool[,] occupied;
    private void Awake()
    {
        occupied = new bool[rows, columns];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))  //임시 테스트
        {
            SpawnRandomMonsters(3);  
        }
    }

    public void SpawnRandomMonsters(int count)
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!occupied[row, col]) emptyCells.Add(new Vector2Int(row, col));
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (emptyCells.Count <= 0) return;

            int randomIndex = Random.Range(0, emptyCells.Count);
            Vector2Int cell = emptyCells[randomIndex];
            emptyCells.RemoveAt(randomIndex);

            SpawnMonster(cell.x, cell.y);
            occupied[cell.x, cell.y] = true;
        }
    }

    private void SpawnMonster(int row, int column)  
    {
        Vector2 spawnPos = GetCellPosition(row, column);

        MonsterData data = monsterPool[Random.Range(0, monsterPool.Count)];

        MonsterHealth monster = Instantiate(data.monsterPrefab, spawnPos, Quaternion.identity);

        monster.Initialize(data);
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
