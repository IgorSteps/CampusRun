using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _frontSpeed = Constants.DEFAULT_FORWARD_SPEED;
    [SerializeField]
    private float _sideSpeed = Constants.DEFAULT_SIDE_SPEED;

    private Vector2 _movement;

    void Update()
    {
        // Forward movement
        transform.Translate(_frontSpeed * Time.deltaTime * Vector3.forward, Space.World);
        
        // Side movement.
        if (_movement.x < 0 && this.gameObject.transform.position.x > Constants.LEFT_BOUNDARY)
        {
            transform.Translate(_sideSpeed * Time.deltaTime * Vector3.left);
        }
        else if (_movement.x > 0 && this.gameObject.transform.position.x < Constants.RIGHT_BOUNDARY)
        {
            transform.Translate(_sideSpeed * Time.deltaTime * Vector3.right);
        }
    }

    // OnMove updates the movement variable based on the InputAction.
    public void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }
}