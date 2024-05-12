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
    private float _magnetPerlinScale = 0.1f;
    // Noise offsets for coin and obstacle.
    private float _coinPerlinOffset;
    private float _cratePerlinOffset;
    private float _columnPerlinOffset;
    private float _carPerlinOffset;
    private float _magnetPerlinOffset;
    // Threshold controls obstacle density. Lower means more obstacles. Higher means decreased frequency.
    private float _cratePlacementThreshold = 0.6f;
    private float _columnPlacementThreshold = 0.6f;
    private float _carPlacementThreshold = 0.8f;
    private float _magnetPlacementThreshold = 0.9f;

    // Constants.
    private Vector3 _startPosition =Constants.START_POS;

    private void OnEnable()
    {
        _coinPerlinOffset = Random.Range(0f, 10000f);
        _cratePerlinOffset = Random.Range(0f, 10000f);
        _columnPerlinOffset = Random.Range(0f, 10000f);
        _carPerlinOffset = Random.Range(0f, 10000f);
        _magnetPerlinOffset = Random.Range(0f, 10000f);
        Populate();
    }

    private void Populate()
    {
        float currentZ = 0f;
        bool magnetPlaced = false;
        while (currentZ < Constants.SECTION_LENGTH)
        {
            // To keep track which lanes are filled.
            bool[] laneFilled = new bool[Constants.NUM_OF_LANES];

            // Place coins.
            float coinNoiseValue = Mathf.PerlinNoise(_coinPerlinOffset, currentZ * _coinPerlinScale);
            int coinLaneIdx = Mathf.FloorToInt(coinNoiseValue * Constants.NUM_OF_LANES); // To floor nose value to 1,2 or 3.
            PlaceCoin(GetXPosition(coinLaneIdx), _startPosition.y, _startPosition.z + currentZ);
            laneFilled[coinLaneIdx] = true;

            // Place crates.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    float crateNoiseValue = Mathf.PerlinNoise(_cratePerlinOffset, currentZ * _cratePerlinScale + lane);
                    if (crateNoiseValue > _cratePlacementThreshold)
                    {
                        PlaceCrate(GetXPosition(lane), _startPosition.y - Constants.CRATE_Y_OFFSET, _startPosition.z + currentZ);
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
                        PlaceColumn(GetXPosition(lane), _startPosition.y - Constants.CRATE_Y_OFFSET, _startPosition.z + currentZ);
                        laneFilled[lane] = true;
                    }
                }
            }

            // Place cars.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane])
                {
                    float carObstacleValue = Mathf.PerlinNoise(_carPerlinOffset, currentZ * _carPerlinScale + lane);
                    if (carObstacleValue > _carPlacementThreshold)
                    {
                        PlaceCar(GetXPosition(lane), _startPosition.y - Constants.CAR_Y_OFFSET, _startPosition.z + currentZ);
                        float carSpacing = Random.Range(Constants.MIN_CAR_SPACING, Constants.MAX_CAR_SPACING);
                        currentZ += carSpacing;
                    }
                }
            }

            // Place magnets.
            for (int lane = 0; lane < 3; lane++)
            {
                if (!laneFilled[lane] && !magnetPlaced)
                {
                    float magnetValue = Mathf.PerlinNoise(_magnetPerlinOffset, currentZ * _magnetPerlinScale + lane);
                    if (magnetValue > _magnetPlacementThreshold)
                    {
                        PlaceMagnet(GetXPosition(lane), _startPosition.y, _startPosition.z + currentZ);
                        laneFilled[lane] = true;
                        magnetPlaced = true;
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

    private void PlaceCar(float xPos, float yPos, float zPos)
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

    private void PlaceMagnet(float xPos, float yPos, float zPos)
    {
        GameObject magnet = PoolManager.s_Instance.GetObject("Magnet");
        magnet.transform.position = new Vector3(xPos, yPos, zPos);
        magnet.transform.SetParent(this.transform, false);
    }

    /// <summary>
    /// GetXPosition gets the lane's x position based on lane index.
    /// </summary>
    private float GetXPosition(int laneIdx)
    {
        return _startPosition.x - laneIdx * Constants.NUM_OF_LANES;
    }

}
