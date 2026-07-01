using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public GameObject OriginPrefab { get; private set; }

    public void Initialize(GameObject originPrefab)
    {
        OriginPrefab = originPrefab;
    }
}