using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 1.0f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0.0f, Speed, 0.0f, Space.World);
    }
}
