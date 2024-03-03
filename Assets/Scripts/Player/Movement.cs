using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed; 
    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Speed * Time.deltaTime * Vector3.forward, Space.World);
    }
}
