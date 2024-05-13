using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public LayerMask groundLayer;
    public float CurrentSpeed;
    public GameObject _playerModel;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _playerAnimator;
    

    private float _upSpeed;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private int _previousLane = 1;
    private readonly float[] _lanePositions = { -3f, 0f, 3f };
    private Vector2 _movementInput;
    private bool _isGrounded;

    void Start()
    {
        CurrentSpeed = Constants.DEFAULT_PLAYER_START_FORWARD_SPEED;
        _playerAnimator = _playerModel.GetComponent<Animator>();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGrounded = IsGrounded();
        // Clamp to the ground if grounded.
        if (_isGrounded && _upSpeed < 0.0f)
        {
            _upSpeed = 0.0f;
        }

        IncreaseSpeed();

        // Forward movement.
        Vector3 forwardMovement = CurrentSpeed * Time.deltaTime * Vector3.forward;

        // Jump movement.
        Vector3 jumpMovement = _upSpeed * Time.deltaTime * Vector3.up;

        // Side movement.
        Vector3 sideMovement = MovePlayerSideways(_lanePositions[_currentLane]);

        // Always apply gravity.
        ApplyGravity();

        // Combine.
        Vector3 finalMovement = forwardMovement + sideMovement + jumpMovement;
        _characterController.Move(finalMovement);
    }

    public void ReturnToPreviousLane()
    {
        if (_previousLane != _currentLane)
        {
            float newXPosition = Mathf.Lerp(
                transform.position.x,
                _lanePositions[_previousLane],
                (CurrentSpeed - 1.0f) * Time.deltaTime
            );
            gameObject.transform.position = new Vector3(
                newXPosition - transform.position.x,
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );

            _currentLane = _previousLane;
        }
    }

    public bool IsGrounded()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = Vector3.down;
        float rayLength = (_characterController.height * 2.0f) + 0.1f;

        // Draw a debug line.
        Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(
            transform.position,
            Vector3.down,
            out hit,
            rayLength,
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
            _upSpeed += Mathf.Sqrt(-3.0f * Constants.DOWNWARD_GRAVITY_FORCE * Constants.DEFAULT_JUMP_HEIGHT);
            _playerAnimator.SetTrigger("OnJump");
        }

        // Reset current and previous lanes, this way of doing it allows us to keep track of both.
        if (newLane != _currentLane)
        {
            _previousLane = _currentLane;
            _currentLane = newLane;
        }
    }

    private Vector3 MovePlayerSideways(float targetXPos)
    {
        float newXPosition = Mathf.Lerp(
            transform.position.x,
            targetXPos,
            (CurrentSpeed - 1.0f) * Time.deltaTime
        );
        return new Vector3(newXPosition - transform.position.x, 0, 0);
    }

    private void IncreaseSpeed()
    {
        if (CurrentSpeed < Constants.DEFAULT_PLAYER_MAX_FORWARD_SPEED)
        {
            CurrentSpeed += Constants.DEFAULT_PLAYER_ACCELERATION * Time.deltaTime;
        }
    }

    private void ApplyGravity()
    {
        _upSpeed += Constants.DOWNWARD_GRAVITY_FORCE * Time.deltaTime;
    }
}