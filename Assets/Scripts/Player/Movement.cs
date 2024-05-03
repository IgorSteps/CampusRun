using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _frontSpeed = Constants.DEFAULT_PLAYER_FORWARD_SPEED;
    [SerializeField] private float _sideSpeed = Constants.DEFAULT_PLAYER_SIDE_SPEED;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private readonly float[] _lanePositions = { -3f, 0f, 3f };

    public GameObject _playerModel;
    private Animator _playerAnimator;
    
    private Vector2 _movementInput;

    void Start()
    {
        _playerAnimator = _playerModel.GetComponent<Animator>();
    }

    void Update()
    {
        // Forward movement
        transform.Translate(_frontSpeed * Time.deltaTime * Vector3.forward, Space.World);

        // Side movement.
        Vector3 newPosition = transform.position;
        // Smooth transition to lane position
        newPosition.x = Mathf.Lerp(newPosition.x, _lanePositions[_currentLane], Time.deltaTime * _sideSpeed); 
        transform.position = newPosition;
    }

    // OnMove updates the lane choice based on the InputAction
    public void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
        Debug.Log("movement y= "+ _movementInput.y);
        int newLane = _currentLane;
        if (_movementInput.x < 0) // Move Left
        {
            if (newLane > 0)
                newLane--;

        }
        else if (_movementInput.x > 0) // Move Right
        {
            if (newLane < 2)
                newLane++;
        } 
        else if (_movementInput.y > 0) // Jump
        {
            _playerAnimator.SetTrigger("OnJump");
        }

        // If the lane has changed, trigger the jump animation
        if (newLane != _currentLane)
        {
            _currentLane = newLane;
            // TODO: Figure out how to sync Jump animation and lane movements.
            //_playerAnimator.SetTrigger("OnJump");
        }
    }
}