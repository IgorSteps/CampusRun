using UnityEngine;

/// <summary>
/// LanePopulator handles procedural coin and obstacle content generation for each section.
/// </summary>
public class LanePopulator : MonoBehaviour
{
    // Distance between coins.
    private float _spacing = 1.0f;
    // Controls how rapidly the coin path shifts between lanes. Can be adjsuted to
    // make the path change more gently or more abruptly.Higher means smoother, lower makes it more abrupt.
    private float _perlinScale = 0.1f;

    private void OnEnable()
    {
        GenerateCoinPath();
    }

    private void GenerateCoinPath()
    {
        Vector3 startPosition = new(3.0f, 1.0f, 0.0f);

       

        float currentZ = 0f;
        float perlinOffset = Random.Range(0f, 10000f);

        while (currentZ < Constants.SECTION_LENGTH)
        {
            // Keep track of which lanes are filled
            bool[] laneFilled = new bool[3];

            // Generate a noise value and map it to the range of lane indices (0, 1, 2)
            float noiseValue = Mathf.PerlinNoise(perlinOffset, currentZ * _perlinScale);
            int laneIndex = Mathf.FloorToInt(noiseValue * 3); // Converts noise to an index 0, 1, or 2

            // Calculate the x position based on the lane index.
            // Because right-most lane is at startPosition.x - 3.
            float laneX = startPosition.x - laneIndex * 3f;

            // Place the coin.
            PlaceCoin(laneX, startPosition.y, startPosition.z + currentZ);
            laneFilled[laneIndex] = true;

            // After placing the coin, place obstacles in the empty lanes
            for (int i = 0; i < laneFilled.Length; i++)
            {
                if (!laneFilled[i])  // Check if the lane is empty
                {
                    float obstacleX = startPosition.x - i * 3f;
                    PlaceObstacle(obstacleX, startPosition.y - 0.30f, startPosition.z + currentZ);
                }
            }

            currentZ += _spacing;
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
}
