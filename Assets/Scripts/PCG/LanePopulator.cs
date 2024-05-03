using UnityEngine;

/// <summary>
/// LanePopulator handles procedural coin and obstacle content generation for each section using Perlin Noise.
/// </summary>
public class LanePopulator : MonoBehaviour
{
    // Distance between rows of objects along the z-axis.
    private float _spacing = 1.0f;
    // Scales for coin and obstacle Perlin noise. Controls how rapidly the path shifts between lanes. Higher means smoother, lower makes it more abrupt.
    private float _coinPerlinScale = 0.1f;
    private float _cratePerlinScale = 0.1f;
    private float _columnPerlinScale = 0.1f;
    // Noise offsets for coin and obstacle.
    private float _coinPerlinOffset;
    private float _cratePerlinOffset;
    private float _columnPerlinOffset;
    // Threshold controls obstacle density. Lower means more obstacles. Higher means decreased frequency.
    private float _cratePlacementThreshold = 0.6f;
    private float _columnPlacementThreshold = 0.7f;

    private void OnEnable()
    {
        _coinPerlinOffset = Random.Range(0f, 10000f);
        _cratePerlinOffset = Random.Range(0f, 10000f);
        _columnPerlinOffset = Random.Range(0f, 10000f);
        Populate();
    }

    private void Populate()
    {
        Vector3 startPosition = new(3.0f, 1.0f, 0.0f); // TODO: Move to constants.
        float currentZ = 0f;

        while (currentZ < Constants.SECTION_LENGTH)
        {
            // Keep track of which lanes are filled.
            bool[] laneFilled = new bool[3];

            // Generate a noise value for coin and floor it to the range of lane indices (0, 1, 2).
            float coinNoiseValue = Mathf.PerlinNoise(_coinPerlinOffset, currentZ * _coinPerlinScale);
            int counLaneIdx = Mathf.FloorToInt(coinNoiseValue * 3);
            // Calculate the x position based on the lane index, where right-most lane is at _startPosition.x.
            float coinLaneX = startPosition.x - counLaneIdx * 3.0f;

            // Place the coin.
            PlaceCoin(coinLaneX, startPosition.y, startPosition.z + currentZ);
            // Set the flag that this lane is now filled.
            laneFilled[counLaneIdx] = true;

            // Place crates in other free lanes.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    // Offset by lane index for diversity.
                    float obstacleNoiseValue = Mathf.PerlinNoise(_cratePerlinOffset, currentZ * _cratePerlinScale + lane);
                    float obstacleLaneX = startPosition.x - lane * 3.0f;
                    if (obstacleNoiseValue > _cratePlacementThreshold)
                    {
                        PlaceCrate(obstacleLaneX, startPosition.y - 0.30f, startPosition.z + currentZ);
                    }
                }
            }

            // Place cars in other free lanes.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    float secondObstacleNoiseValue = Mathf.PerlinNoise(_columnPerlinOffset, currentZ * _columnPerlinScale + lane);
                    if (secondObstacleNoiseValue > _columnPlacementThreshold)
                    {
                        float secondObstacleLaneX = startPosition.x - lane * 3.0f;
                        PlaceColumn(secondObstacleLaneX, startPosition.y + 1.70f, startPosition.z + currentZ);
                    }
                }
            }

            // Increment z position by the spacing.
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

    private void PlaceCrate(float xPos, float yPos, float zPos)
    {
        GameObject obstacle = PoolManager.s_Instance.GetObject("Obstacle");
        if (obstacle != null)
        {
            obstacle.transform.position = new Vector3(xPos, yPos, zPos);
            obstacle.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceColumn(float xPos, float yPos, float zPos)
    {
        GameObject column = PoolManager.s_Instance.GetObject("Column");
        if (column != null)
        {
            column.transform.position = new Vector3(xPos, yPos, zPos);
            column.transform.SetParent(this.transform, false);
        }
    }
}
