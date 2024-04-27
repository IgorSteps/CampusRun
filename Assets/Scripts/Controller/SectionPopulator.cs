using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// SectionPopulator is handless procedural content generation for each section.
/// </summary>
public class SectionPopulator : MonoBehaviour
{
    // Coins/Obstacles:
    [SerializeField] private int _numOfThingsInLane = 15;
    // Scenery:
    [SerializeField] private int _treesPerSection = 20;
    [SerializeField] private int _rockPerSection = 10;
    [SerializeField] private int _smallRockPerSection = 10;
    [SerializeField] private int _treeStumpsPerSection = 10;
    // To keep track of occupied positions by scenery objects.
    private readonly HashSet<Vector3> _occupiedPositions = new HashSet<Vector3>();

    private void OnEnable()
    {
        // Populate the section.
        PopulateLanes();
        //PopulateScenery();
    }

    /// <summary>
    /// PopulateLanes procedurally generates coins and obstacles in each lane for the section.
    /// </summary>
    private void PopulateLanes()
    {
        // This places the first coin/obstacle at the center of the right-most row.
        Vector3 startPosition = new(6.0f, 1.0f, 0.0f);

        // Randomly select one lane to be free of obstacles.
        int freeLane = Random.Range(0, 3);

        for (int lane = 0; lane < 3; lane++)
        {
            // Because the player's ground area is 9 units wide, we place each coin at its own lane by
            // subtracting 3 from each.
            startPosition.x -= 3;
            for (int row = 0; row < _numOfThingsInLane; row++)
            {
                if (lane == freeLane)
                {
                    // Place only coins in the free lane
                    PlaceCoin(startPosition.x, startPosition.y, startPosition.z + row);
                }
                else
                {
                    // Randomly decide to place a coin or an obstacle
                    if (Random.Range(0, 2) == 0) // 50% chance for each
                    {
                        PlaceCoin(startPosition.x, startPosition.y, startPosition.z + row);
                    }
                    else
                    {
                        // TODO: Move the offset to a separate variable?
                        PlaceObstacle(startPosition.x, startPosition.y - 0.2f, startPosition.z + row);
                    }
                }
            }
        }
    }

    /// <summary>
    /// PopulateScenery procedurally generates scenery for the section.
    /// </summary>
    private void PopulateScenery()
    {
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

    private void PlaceCoin(float xPos, float yPos, float zPos)
    {
        GameObject coin = PoolManager.s_Instance.GetObject("Coin");
        if (coin != null)
        {
            coin.transform.position = new Vector3(xPos, yPos, zPos);
            coin.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceObstacle(float xPos, float yPos, float zPos)
    {
        GameObject obstacle = PoolManager.s_Instance.GetObject("Obstacle");
        if (obstacle != null)
        {
            obstacle.transform.position = new Vector3(xPos, yPos, zPos);
            obstacle.transform.SetParent(this.transform, false);
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

    /// <summary>
    /// OnDisable recycles game objects and clears the occupied positions set.
    /// </summary>
    void OnDisable()
    {
        // Recycle objects. 
        foreach (Transform child in transform)
        {
            switch(transform.tag)
            {
                case "Coin":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Coin");
                    break;
                case "Obstacle":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Obstacle");
                    break;
                case "Tree":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree");
                    break;
                case "Tree2":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree2");
                    break;
                case "Tree3":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree3");
                    break;
                case "Tree4":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree4");
                    break;
                case "SmallRocks":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "SmallRocks");
                    break;
                case "TreeStump":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "TreeStump");
                    break;
                case "Rock":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Rock");
                    break;
                case "Section":
                    // Sections are recycled in the level controller.
                    break;
                default:
                    Debug.LogWarning("Cannot recycle '" + transform.tag + "', I don't know it");
                    break;
            }
        }

        // Clear the occupied positions.
        _occupiedPositions.Clear();
    }
}
