using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PoolManager is a singelton that handles the creation, storage, retrieval, and recycling of section objects.
/// </summary>
public class PoolManager : MonoBehaviour
{
    // Singleton instance of the Pool Manager.
    public static PoolManager s_Instance;
    // The prefab for the level sections.
    [SerializeField] private GameObject _sectionPrefab;
    // Initial size of the pool.
    [SerializeField] private int _poolSize = 10;
    // Pool queue.
    private Queue<GameObject> _pool = new Queue<GameObject>();

    /// <summary>
    /// Awake populates the object _pool with a predefined number of section objects.
    /// </summary>
    private void Awake()
    {
        s_Instance = this;

        // Populate the _pool with deactivated instances of section prefab.
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject newSection = Instantiate(_sectionPrefab);
            newSection.SetActive(false);
            _pool.Enqueue(newSection);
        }
    }

    /// <summary>
    /// GetSection gets a section from the top of the pool queue or creates a new one if all sections are in use.
    /// </summary>
    public GameObject GetSection()
    {
        if (_pool.Count > 0)
        {
            GameObject section = _pool.Dequeue();
            section.SetActive(true);
            return section;
        }
        else
        {
            // TODO: Cap this somehow to avoid excessive memory use?
            // Create a new section if all objects are already in use.
            GameObject newSection = Instantiate(_sectionPrefab);
            newSection.SetActive(true);
            return newSection;
        }
    }

    /// <summary>
    /// ReturnSection deactivates a section and places it back into the pool. 
    /// </summary>
    public void ReturnSection(GameObject section)
    {
        // This makes the section reusable.
        section.SetActive(false);
        _pool.Enqueue(section);
    }
}
