using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SceneryPopulator handles procedural scenery content generation for each section.
/// </summary>
public class SceneryPopulator : MonoBehaviour
{
    // Scenery:
    [SerializeField] private int _treesPerSection = 20;
    [SerializeField] private int _rockPerSection = 10;
    [SerializeField] private int _smallRockPerSection = 10;
    [SerializeField] private int _treeStumpsPerSection = 10;
    // To keep track of occupied positions by scenery objects.
    private HashSet<Vector3> _occupiedPositions;

    private void OnEnable()
    {
        _occupiedPositions = new HashSet<Vector3>(); // Clear for every section.
        PopulateScenery();
    }

    /// <summary>
    /// PopulateScenery procedurally generates scenery for the section.
    /// </summary>
    private void PopulateScenery()
    {
        // TODO: There is 100% better techniques to populate terrains, but
        // in the given timeframe, it is not possible to implement them correctly.
        // I think it's fine to just randomly place things for this project.
        for (int i = 0; i < _treesPerSection; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            
            // TODO: Should the tree type be an enum?
            int treeType = Random.Range(1, 4);
            PlaceTree(treeType, position);
        }

        for (int i = 0; i < _smallRockPerSection; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceSmallRocks(position);
        }

        for (int i = 0; i < _treeStumpsPerSection; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceTreeStump(position);
        }

        for (int i = 0; i < _rockPerSection; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceRock(position);
        }
    }

    private void PlaceTree(int treeType, Vector3 pos)
    {
        // Build the name of the tree pool name, in the format TreeNumber.
        string treePoolName = string.Format("Tree"+treeType);
        GameObject tree = PoolManager.s_Instance.GetObject(treePoolName);
        if (tree != null)
        {
            tree.transform.position = pos;
            tree.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceRock(Vector3 pos)
    {
        GameObject rock = PoolManager.s_Instance.GetObject("Rock");
        if (rock != null)
        {
            pos.y = 0.3f;
            rock.transform.position = pos;
            rock.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceSmallRocks(Vector3 pos)
    {
        GameObject sRocks = PoolManager.s_Instance.GetObject("SmallRocks");
        if (sRocks != null)
        {
            pos.y = 0.3f;
            sRocks.transform.position = pos;
            sRocks.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceTreeStump(Vector3 pos)
    {
        GameObject tStump = PoolManager.s_Instance.GetObject("TreeStump");
        if (tStump != null)
        {
            tStump.transform.position = pos;
            tStump.transform.SetParent(this.transform, false);
        }
    }

    /// <summary>
    /// GenerateXPosition generates x-position for scenery objects outside the player's area.
    /// </summary>
    private float GenerateXPosition()
    {
        // Randomly choose where to place(left or right side of the player area).
        bool placeOnLeft = Random.value > 0.5f;
        if (placeOnLeft)
        {
            return Random.Range(Constants.SECTION_AREA_END, Constants.PLAYER_AREA_END);  // Left side, outside player area
        }
        else
        {
            return Random.Range(Constants.PLAYER_AREA_START, Constants.SECTION_AREA_START);  // Right side, outside player area
        }
    }

    /// <summary>
    /// GenerateUnoccupiedPosition generates an unoccupied position for the scenery object.
    /// </summary>
    private Vector3 GenerateUnoccupiedPosition()
    {
        Vector3 potentialPosition;
        do {
            float xPos = GenerateXPosition();
            float yPos = 0f;
            float zPos = Random.Range(0.0f, Constants.SECTION_LENGTH);
            potentialPosition = new Vector3(xPos, yPos, zPos);
        } while (_occupiedPositions.Contains(potentialPosition));

        _occupiedPositions.Add(potentialPosition);
        return potentialPosition;
    }
}
