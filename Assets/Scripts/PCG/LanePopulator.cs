using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

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
    private float _carPerlinScale = 0.1f;
    // Noise offsets for coin and obstacle.
    private float _coinPerlinOffset;
    private float _cratePerlinOffset;
    private float _columnPerlinOffset;
    private float _carPerlinOffset;
    // Threshold controls obstacle density. Lower means more obstacles. Higher means decreased frequency.
    private float _cratePlacementThreshold = 0.6f;
    private float _columnPlacementThreshold = 0.6f;
    private float _carPlacementThreshold = 0.7f;

    private const float _laneWidth = 3.0f;
    private const float _carYOffset = 0.9f;
    private const float _minCarSpacing = 6.0f;
    private const float _maxCarSpacing = 10.0f;

    private void OnEnable()
    {
        _coinPerlinOffset = Random.Range(0f, 10000f);
        _cratePerlinOffset = Random.Range(0f, 10000f);
        _columnPerlinOffset = Random.Range(0f, 10000f);
        _carPerlinOffset = Random.Range(0f, 10000f);
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
            int counLaneIdx = Mathf.FloorToInt(coinNoiseValue * _laneWidth);
            // Calculate the x position based on the lane index, where right-most lane is at _startPosition.x.
            float coinLaneX = startPosition.x - counLaneIdx * _laneWidth;

            PlaceCoin(coinLaneX, startPosition.y, startPosition.z + currentZ);
            // Set the flag that this lane is now filled.
            laneFilled[counLaneIdx] = true;

            // Place crates.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    // Offset by lane index for diversity.
                    float obstacleNoiseValue = Mathf.PerlinNoise(_cratePerlinOffset, currentZ * _cratePerlinScale + lane);
                    float obstacleLaneX = startPosition.x - lane * _laneWidth;
                    if (obstacleNoiseValue > _cratePlacementThreshold)
                    {
                        PlaceCrate(obstacleLaneX, startPosition.y - 0.30f, startPosition.z + currentZ);
                        laneFilled[lane] = true;
                    }
                }
            }

            // Place columns.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    float columnNoise = Mathf.PerlinNoise(_columnPerlinOffset, currentZ * _columnPerlinScale + lane);
                    if (columnNoise > _columnPlacementThreshold)
                    {
                        float columnX = startPosition.x - lane * 3.0f;
                        PlaceColumn(columnX, startPosition.y - 0.3f, startPosition.z + currentZ);
                        laneFilled[lane] = true;
                    }
                }
            }

            //Place cars.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    float carObstacleValue = Mathf.PerlinNoise(_carPerlinOffset, currentZ * _carPerlinScale + lane);
                    if (carObstacleValue > _carPlacementThreshold)
                    {
                        float carX = startPosition.x - lane * _laneWidth;
                        PlaceCarNewWay(carX, startPosition.y - _carYOffset, startPosition.z + currentZ);
                        float carSpacing = UnityEngine.Random.Range(_minCarSpacing, _maxCarSpacing);
                        currentZ += carSpacing;
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
        GameObject obstacle = PoolManager.s_Instance.GetObject("Crate");
        if (obstacle != null)
        {
            obstacle.transform.position = new Vector3(xPos, yPos, zPos);
            obstacle.transform.SetParent(this.transform, false);
        }
    }

    private void PlaceColumn(float xPos, float yPos, float zPos)
    {
        GameObject column = PoolManager.s_Instance.GetObject("Column");
        GameObject lowerCrate = PoolManager.s_Instance.GetObject("Crate");
        GameObject upperCrate = PoolManager.s_Instance.GetObject("Crate");
        if (column != null)
        {
            column.transform.position = new Vector3(xPos, yPos, zPos);
            column.transform.SetParent(this.transform, false);
            if (lowerCrate != null && upperCrate != null)
            {
                lowerCrate.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                lowerCrate.transform.SetParent(column.transform, false);
                upperCrate.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
                upperCrate.transform.SetParent(column.transform, false);
            }
        }
    }

  

    private void PlaceCarNewWay(float xPos, float yPos, float zPos)
    {
        GameObject car = PoolManager.s_Instance.GetObject("Car");
        if (car != null)
        {
            car.transform.position = new Vector3(xPos, yPos, zPos);
            car.transform.SetParent(this.transform, false);

            // Car pieces, no need to set positions here as they are set in the prefabs.
            // Glass.
            GameObject glass = PoolManager.s_Instance.GetObject("Glass");
            glass.transform.SetParent(car.transform, false);
            // Plates.
            GameObject plates = PoolManager.s_Instance.GetObject("Plates");
            plates.transform.SetParent(car.transform, false);
            // Steering.
            GameObject steeringWheel = PoolManager.s_Instance.GetObject("SteeringWheel");
            steeringWheel.transform.SetParent(car.transform, false);
            // Wheels.
            GameObject flWheel = PoolManager.s_Instance.GetObject("FrontLeftWheel");
            flWheel.transform.SetParent(car.transform, false);
            GameObject frWheel = PoolManager.s_Instance.GetObject("FrontRightWheel");
            frWheel.transform.SetParent(car.transform, false);
            GameObject rlWheel = PoolManager.s_Instance.GetObject("RearLeftWheel");
            rlWheel.transform.SetParent(car.transform, false);
            GameObject rrWheel = PoolManager.s_Instance.GetObject("RearRightWheel");
            rrWheel.transform.SetParent(car.transform, false);
        }
    }

}
