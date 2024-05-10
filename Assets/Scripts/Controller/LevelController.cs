using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// LevelController manages the spawning and recycling level sections.
/// </summary>
public class LevelController : MonoBehaviour
{
    // Determines when to spawn new sections.
    [SerializeField] private Transform _player;
    // The Z position where the next section should spawn.
    [SerializeField] private float _spawnZ = 0.0f;
    // Number of sections active on screen.
    [SerializeField] private int _numSectionsOnScreen = 5;
    // Distance behind the player where sections can be recycled.
    [SerializeField] private float _safeZone = 50.0f;
    // List of active sections.
    private readonly List<GameObject> activeSections = new List<GameObject>();
    private float roadZOffset = 18.0f;
    
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
        if (_player.position.z - _safeZone > (_spawnZ - _numSectionsOnScreen * Constants.SECTION_LENGTH))
        {
            SpawnSection();
            RecycleSection();
        }
    }

    private void SpawnSection()
    {
        GameObject section = PoolManager.s_Instance.GetObject("Section");
        section.transform.position = new Vector3(0, 0, _spawnZ);
        SetupSection(section);
        _spawnZ += Constants.SECTION_LENGTH;
        activeSections.Add(section);
    }

    /// <summary>
    /// Setup the section design, needs to be done here because if these objects just exist on the Section prefab
    /// they never get recycled/pulled as they don't come from the pool.
    /// </summary>
    /// <param name="section"></param>
    private void SetupSection(GameObject section)
    {
        GameObject leftGround = PoolManager.s_Instance.GetObject("LeftGround");
        leftGround.transform.position = new Vector3(-12.05f, -0.092f, _spawnZ);
        leftGround.transform.SetParent(section.transform);

        GameObject rightGround = PoolManager.s_Instance.GetObject("RightGround");
        rightGround.transform.position = new Vector3(12.05f, -0.092f, _spawnZ);
        rightGround.transform.SetParent(section.transform);

        GameObject leftTile = PoolManager.s_Instance.GetObject("LeftTile");
        leftTile.transform.position = new Vector3(-4.27f, 0.18455f, _spawnZ - roadZOffset);
        leftTile.transform.SetParent(section.transform);

        GameObject middleTile = PoolManager.s_Instance.GetObject("MiddleTile");
        middleTile.transform.position = new Vector3(-1.37f, 0.18455f, _spawnZ - roadZOffset);
        middleTile.transform.SetParent(section.transform);

        GameObject rightTile = PoolManager.s_Instance.GetObject("RightTile");
        rightTile.transform.position = new Vector3(1.53f, 0.18455f, _spawnZ - roadZOffset);
        rightTile.transform.SetParent(section.transform);
    }

    private void RecycleSection()
    {
        GameObject section = activeSections[0];

        // Collect all children to be recycled into a separate list,
        // because you can't iterate over and modify children in the same loop.
        List<GameObject> childrenToRecycle = new List<GameObject>();
        foreach (Transform child in section.transform)
        {
            childrenToRecycle.Add(child.gameObject);
        }

        // Recycle all collected children
        foreach (GameObject child in childrenToRecycle)
        {
            child.transform.SetParent(null); // de-assign from the parent section.
            PoolManager.s_Instance.ReturnObject(child, child.tag);
        }

        // Check if all children are recycled
        if (section.transform.childCount > 0)
        {
            Debug.LogError("Failed to recycle all children, there are '" + section.transform.childCount + "' children left.");
        }

        activeSections.RemoveAt(0);
        PoolManager.s_Instance.ReturnObject(section, "Section");
    }
}
