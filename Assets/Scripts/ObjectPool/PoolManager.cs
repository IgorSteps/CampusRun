using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// PoolManager is a singelton that handles the creation, storage, retrieval, and recycling of section objects.
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
                objectPool.Enqueue(obj);
            }

            // Add that pool to dictionary.
            _poolDictionary.Add(pool.name, objectPool);
        }
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
            // TODO: Dynamically create extra objects if necessary.
            Debug.LogWarning("All objects in the '" + name + "' pool are in use.");
            return null;
        }
    }

    /// <summary>
    /// ReturnObject deactivates and places the given object back into the pool with the given name. 
    /// </summary>
    public void ReturnObject(GameObject obj, string name)
    {
        if (!_poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool '" + name + "' doesn't exist.");
            return;
        }

        obj.SetActive(false);
        _poolDictionary[name].Enqueue(obj);
    }
}
