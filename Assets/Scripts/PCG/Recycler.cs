using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    /// <summary>
    /// OnDisable recycles game objects.
    /// </summary>
    void OnDisable()
    {
        // Recycle objects. 
        foreach (Transform child in transform)
        {
            switch (child.gameObject.tag)
            {
                case "Coin":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Coin");
                    break;
                case "Car":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Car");
                    break;
                case "Obstacle":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Obstacle");
                    break;
                case "FracturedCrate":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "FracturedCrate");
                    break;
                case "Column":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Column");
                    break;
                case "Tree":
                    PoolManager.s_Instance.ReturnObject(child.gameObject, "Tree1");
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
                case "Skip":
                    // These objects are children of the Section, but they're part of the prefab so they get recycled anyway.
                    break;
                default:
                    Debug.LogWarning("Cannot recycle tag '" + child.gameObject.tag + "', the object's name is '" + child.gameObject.name + "'.");
                    break;
            }
        }
    }
}
