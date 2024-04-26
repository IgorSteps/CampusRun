using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SectionPopulator : MonoBehaviour
{
    [SerializeField] private int _numOfCoinsInColumn = 15;

    [SerializeField] private int maxTreesPerSection = 50;
    [SerializeField] private float minTreeSpacing = 5f;

    private HashSet<Vector3> _occupiedPositions;

    private void OnEnable()
    {
        _occupiedPositions = new HashSet<Vector3>();
        Populate();
        PopulateScenery();
    }

    private void Populate()
    {
        // This places the first thing at the center of the right-most row
        Vector3 startPosition = new(6.0f, 1.5f, 0.0f);
        // Randomly select one column to be free of obstacles
        int freeColumn = Random.Range(0, 3);

        for (int column = 0; column < 3; column++)
        {
            // Because the ground is 9, by subtracting 3 from each
            startPosition.x -= 3;
            for (int row = 0; row < _numOfCoinsInColumn; row++)
            {
                if (column == freeColumn)
                {
                    // Place only coins in the free column
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
                        PlaceObstacle(startPosition.x, startPosition.y, startPosition.z + row);
                    }
                }
            }
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

    private void PopulateScenery()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            int treeNum = Random.Range(1, 4);
            PlaceTree(treeNum, position);
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceSmallRocks(position);
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceTreeStump(position);
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 position = GenerateUnoccupiedPosition();
            PlaceRock(position);
        }

        // TODO: Should there be houses in the first place?
        //for (int i=0; i<2; i++)
        //{
        //    Vector3 position = GenerateUnoccupiedPosition();
        //    PlaceHouse(position);
        //}
    }

    private void PlaceTree(int treeNum, Vector3 pos)
    {
        string treePoolName = string.Format("Tree"+treeNum);
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

    private void PlaceHouse(Vector3 pos)
    {
        GameObject house = PoolManager.s_Instance.GetObject("House");
        if (house != null)
        {
            house.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            house.transform.position = pos;
            house.transform.SetParent(this.transform, false);
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

    private float GenerateXPosition()
    {
        // Player area extends from -4.5 to 4.5 along the x-axis
        float playerAreaStart = -4.5f;
        float playerAreaEnd = 4.5f;

        // Randomly decide to place on the left or right side of the player area
        bool placeOnLeft = Random.value > 0.5f;

        if (placeOnLeft)
        {
            return Random.Range(-18f, playerAreaStart);  // Left side, outside player area
        }
        else
        {
            return Random.Range(playerAreaEnd, 18f);  // Right side, outside player area
        }
    }

    private Vector3 GenerateUnoccupiedPosition()
    {
        Vector3 potentialPosition;
        do
        {
            float xPos = GenerateXPosition();
            float yPos = 0f;
            float zPos = Random.Range(0.0f, 30.0f);
            potentialPosition = new Vector3(xPos, yPos, zPos);
        } while (_occupiedPositions.Contains(potentialPosition));

        _occupiedPositions.Add(potentialPosition);
        return potentialPosition;
    }

    void OnDisable()
    {
        // Return all child obstacles to the pool
        foreach (Transform child in transform)
        {
            if (transform.CompareTag("Coin"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Coin");
            }

            if (transform.CompareTag("Obstacle"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Obstacle");
            }

            if (transform.CompareTag("Tree"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree");
            }

            if (transform.CompareTag("Tree2"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree2");
            }

            if (transform.CompareTag("Tree3"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree3");
            }

            if (transform.CompareTag("Tree4"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree4");
            }

            if (transform.CompareTag("SmallRocks"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "SmallRocks");
            }

            if (transform.CompareTag("TreeStump"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "TreeStump");
            }

            if (transform.CompareTag("House"))
            {
                PoolManager.s_Instance.ReturnObject(child.gameObject, "House");
            }

            _occupiedPositions.Clear();
        }
    }
}
