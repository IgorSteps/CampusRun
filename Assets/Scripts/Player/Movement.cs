using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float FrontSpeed = Constants.DEFAULT_FORWARD_SPEED;
    public float SideSpeed = Constants.DEFAULT_SIDE_SPEED;

    private Vector2 _movement;

    void Update()
    {
        // Forward movement
        transform.Translate(FrontSpeed * Time.deltaTime * Vector3.forward, Space.World);
        
        // Side movement.
        if (_movement.x < 0 && this.gameObject.transform.position.x > Constants.LEFT_BOUNDARY)
        {
            transform.Translate(SideSpeed * Time.deltaTime * Vector3.left);
        }
        else if (_movement.x > 0 && this.gameObject.transform.position.x < Constants.RIGHT_BOUNDARY)
        {
            transform.Translate(SideSpeed * Time.deltaTime * Vector3.right);
        }
    }

    // OnMove updates the movement variable based on the InputAction.
    public void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
        Debug.Log("Move input received: " + _movement);
    }
}