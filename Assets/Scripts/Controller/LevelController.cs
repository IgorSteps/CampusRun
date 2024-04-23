using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LevelController manages the spawning and recycling level sections.
/// </summary>
public class LevelController : MonoBehaviour
{
    // Determines when to spawn new sections.
    [SerializeField] private Transform _player;
    // The Z position where the next section should spawn.
    [SerializeField] private float _spawnZ = 0.0f;
    // Length of the section.
    [SerializeField] private float _sectionLength = 30.0f;
    // Number of sections active on screen.
    [SerializeField] private int _numSectionsOnScreen = 5;
    // Distance behind the player where sections can be recycled.
    [SerializeField] private float _safeZone = 50.0f;
    // List of active sections.
    private List<GameObject> activeSections = new List<GameObject>();

    /// <summary>
    /// Start initially populates the world with a number of sections.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < _numSectionsOnScreen; i++)
        {
            SpawnSection();
        }
    }

    /// <summary>
    /// Update checks the player's position to see if they have moved past a threshold, 
    /// which triggers the spawning and recycling of sections.
    /// </summary>
    private void Update()
    {
        if (_player.position.z - _safeZone > (_spawnZ - _numSectionsOnScreen * _sectionLength))
        {
            SpawnSection();
            RecycleSection();
        }
    }

    private void SpawnSection()
    {
        GameObject section = PoolManager.s_Instance.GetSection();
        section.transform.position = Vector3.forward * _spawnZ;
        _spawnZ += _sectionLength;
        activeSections.Add(section);
    }

    private void RecycleSection()
    {
        GameObject section = activeSections[0];
        activeSections.RemoveAt(0);
        PoolManager.s_Instance.ReturnSection(section);
    }
}
