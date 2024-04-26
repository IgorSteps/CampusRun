using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _speed = Constants.DEFAULT_ROATION_SPEED;

    void Update()
    {
        transform.Rotate(0.0f, _speed, 0.0f, Space.World);
    }
}
