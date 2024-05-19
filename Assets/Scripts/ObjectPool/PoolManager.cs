using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// PoolManager is a singleton that handles the creation, storage, retrieval, and recycling of game objects.
/// </summary>
public class PoolManager : MonoBehaviour
{
    // Singleton instance of the Pool Manager.
    public static PoolManager s_Instance;
    // List of all pools.
    [SerializeField] private List<Pool> _pools; 
    // Pool of pools.
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    /// <summary>
    /// Awake populates the pool dictionary with different pools.
    /// </summary>
    private void Awake()
    {
        s_Instance = this;
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Populate the dictionary with different pools(section, coin, obstacle).
        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Popualte the pool(eg. the section pool) with deactivated instances of their prefab.
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.AddComponent<Poolable>().poolName = pool.prefab.name; // Add the component with the pool name
                objectPool.Enqueue(obj);
            }

            // Add that pool to dictionary.
            _poolDictionary.Add(pool.prefab.name, objectPool);
        }

        Debug.Log("PoolManager is ready");
    }

    /// <summary>
    /// GetObject gets the object from the top of a pool queue with the given name.
    /// </summary>
    public GameObject GetObject(string name)
    {
        if (!_poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool '" + name + "' doesn't exist.");
            return null;
        }

        if (_poolDictionary[name].Count > 0)
        {
            GameObject obj = _poolDictionary[name].Dequeue();
            obj.SetActive(true);
            
            return obj;
        }
        else
        {
            // TODO: Dynamically create extra objects?
            Debug.LogWarning("All objects in the '" + name + "' pool are in use.");
            return null;
        }
    }

    /// <summary>
    /// ReturnObject deactivates and places the given object back into the pool with the given name. 
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable != null && _poolDictionary.ContainsKey(poolable.poolName))
        {
            ResetRigidBodyObject(obj);
            obj.SetActive(false);
            _poolDictionary[poolable.poolName].Enqueue(obj);
        }
        else if (poolable != null)
        {
            Debug.LogError("Pool '" + poolable.poolName + "' not found");
        }
        else
        {
            Debug.LogError("Poolable component missing on the '" + obj.name + "' game object.");
        }
    }

    private void ResetRigidBodyObject(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
