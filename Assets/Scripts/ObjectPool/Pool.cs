using UnityEngine;

[System.Serializable] 
public class Pool
{
    // Unique identifier for each pool type
    public string name;
    // Prefab for the pool.
    public GameObject prefab;
    // Initial size of the pool.
    public int poolSize;
}
