using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.back, Space.World);
    }
}
