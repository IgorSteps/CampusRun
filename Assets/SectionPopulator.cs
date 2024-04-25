using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SectionPopulator : MonoBehaviour
{
    public Transform sectionStart; // Start position for coin generation in a section

    private void OnEnable()
    {
        PopulateObstacles();
    }

    private void PopulateObstacles()
    {
        // This places the first thing at the center of the right-most row
        Vector3 startPosition = new(6.0f, 1.5f, 0.0f);
      
        for (int column = 0; column < 3; column++)
        {
            // Because the ground is 9, by subtracting 3 from each
            startPosition.x -= 3;
            for (int row = 0; row < 20; row++)
            {
                PlaceCoin(startPosition.x, startPosition.y, startPosition.z + row);
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
        }
    }
}
