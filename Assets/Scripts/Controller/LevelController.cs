using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;

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
        PrepareSection(section);
        _spawnZ += Constants.SECTION_LENGTH;
        activeSections.Add(section);
    }

    /// <summary>
    /// PrepareSection sets up the section design, needs to be done here because if these objects just
    /// exist on the Section prefab they never get recycled/pulled as they don't come from the pool.
    /// </summary>
    /// 
    // TODO: set up scales and rotations programmatically.
    private void PrepareSection(GameObject section)
    {
        // Mountains.
        GameObject leftMountain = PoolManager.s_Instance.GetObject("Mountain");
        leftMountain.transform.position = new Vector3(-24.05f, -0.092f, _spawnZ);
        leftMountain.transform.SetParent(section.transform);

        GameObject rightMountain = PoolManager.s_Instance.GetObject("Mountain");
        rightMountain.transform.position = new Vector3(24.05f, -0.092f, _spawnZ);
        rightMountain.transform.SetParent(section.transform);

        // Grounds.
        GameObject leftGround = PoolManager.s_Instance.GetObject("Ground");
        leftGround.transform.position = new Vector3(-12.05f, -0.092f, _spawnZ);
        leftGround.transform.SetParent(section.transform);

        GameObject rightGround = PoolManager.s_Instance.GetObject("Ground");
        rightGround.transform.position = new Vector3(12.05f, -0.092f, _spawnZ);
        rightGround.transform.SetParent(section.transform);

        // Road tiles.
        GameObject leftTile = PoolManager.s_Instance.GetObject("Tile");
        leftTile.transform.position = new Vector3(-4.27f, 0.18455f, _spawnZ - roadZOffset);
        leftTile.transform.SetParent(section.transform);

        GameObject middleTile = PoolManager.s_Instance.GetObject("Tile");
        middleTile.transform.position = new Vector3(-1.37f, 0.18455f, _spawnZ - roadZOffset);
        middleTile.transform.SetParent(section.transform);

        GameObject rightTile = PoolManager.s_Instance.GetObject("Tile");
        rightTile.transform.position = new Vector3(1.53f, 0.18455f, _spawnZ - roadZOffset);
        rightTile.transform.SetParent(section.transform);
    }

    /// <summary>
    /// Recycle sections and children recursively.
    /// </summary>
    private void RecycleSection()
    {
        GameObject section = activeSections[0];

        // Recycle all children and children of children etc of the section.
        RecycleChildrenRecursively(section);

        activeSections.RemoveAt(0);
        PoolManager.s_Instance.ReturnObject(section);
    }

    /// <summary>
    /// RecycleChildrenRecursively recuresively recycles children of the parent.
    /// </summary>
    private void RecycleChildrenRecursively(GameObject parent)
    {
        // Needs a temporary list to hold children because you can't modify lists during iteration(thanks C#).
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            // Recursively recycle the children of this child.
            RecycleChildrenRecursively(child);

            // Unparent and return to the pool.
            child.transform.SetParent(null);
            PoolManager.s_Instance.ReturnObject(child);
        }

        // Let's log in case the above failed.
        if (parent.transform.childCount > 0)
        {
            Debug.LogWarning("Failed to recycle all '" + parent.name + "' children, there are '" + parent.transform.childCount + "' children left.");
        }
    }
}
