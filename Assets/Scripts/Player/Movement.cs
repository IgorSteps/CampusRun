using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _frontSpeed = Constants.DEFAULT_PLAYER_FORWARD_SPEED;
    [SerializeField] private float _sideSpeed = Constants.DEFAULT_PLAYER_SIDE_SPEED;
    [SerializeField] private float _upSpeed;

    public GameObject _playerModel;
    private Animator _playerAnimator;
    [SerializeField] private CharacterController _characterController;

    // For moving.
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private readonly float[] _lanePositions = { -3f, 0f, 3f };
    private Vector2 _movementInput;
    private float jumpHeight = 1.0f;
    private float _gravity = 10f;
    private bool _isGrounded;

    public LayerMask groundLayer;

    void Start()
    {
        _playerAnimator = _playerModel.GetComponent<Animator>();
    }

    void Update()
    {
        _isGrounded = IsGrounded();
        // Always apply gravity.
        if (!_isGrounded || _upSpeed < 0)
        {
            _upSpeed -= _gravity * Time.deltaTime * 2.5f ;
        }

        // Forward movement.
        Vector3 forwardMovement = _frontSpeed * Time.deltaTime * Vector3.forward;

        // Jump movement.
        Vector3 jumpMovement = _upSpeed * Time.deltaTime * Vector3.up;

        // Side movement.
        float targetXPosition = _lanePositions[_currentLane];
        float newXPosition = Mathf.Lerp(transform.position.x, targetXPosition, Time.deltaTime * _sideSpeed);
        Vector3 sideMovement = new Vector3(newXPosition - transform.position.x, 0, 0);

        // Combine.
        Vector3 finalMovement = forwardMovement + sideMovement + jumpMovement;
        _characterController.Move(finalMovement);
    }

    bool IsGrounded()
    {
        // Starts at the center of the player and point down
        RaycastHit hit;
        if (Physics.Raycast(
            transform.position,
            Vector3.down,
            out hit,
            (_characterController.height*2.0f) + 0.1f,
            groundLayer
        ))
        {
            return true;
        }
        return false;
    }

    // OnMove updates the lane choice based on the InputAction
    public void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
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
        else if (_movementInput.y > 0 && _isGrounded) // Jump
        {
            // Fancy maths to find speed required to reach jumpHeight using Earth's gravity.
            _upSpeed = Mathf.Sqrt(2.0f * _gravity * jumpHeight);
            _playerAnimator.SetTrigger("OnJump");
        }

        // If the lane has changed, trigger the jump animation
        if (newLane != _currentLane)
        {
            _currentLane = newLane;
        }
    }
}