using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(prefab))
            poolDict[prefab] = new Queue<GameObject>();

        GameObject obj;

        if (poolDict[prefab].Count > 0)
        {
            obj = poolDict[prefab].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab);
            obj.AddComponent<PooledObject>().Initialize(prefab);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        PooledObject pooled = obj.GetComponent<PooledObject>();

        if (pooled == null || pooled.OriginPrefab == null)
        {
            Destroy(obj);
            return;
        }

        if (!poolDict.ContainsKey(pooled.OriginPrefab)) poolDict[pooled.OriginPrefab] = new Queue<GameObject>();

        obj.SetActive(false);
        poolDict[pooled.OriginPrefab].Enqueue(obj);
    }
}