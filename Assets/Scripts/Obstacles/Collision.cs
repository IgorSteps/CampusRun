using UnityEngine;

/// <summary>
/// Collision handels collision response with obstacles.
/// </summary>
public class Collision : MonoBehaviour
{
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Animator _playerAnimator;
    [SerializeField] private GameObject _player;
    public void Start()
    {
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
        _player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        float heigthDiff = other.transform.position.y - this.gameObject.transform.position.y;
        if (heigthDiff > 1.5f)
        {
             UpdateYPositionOnGround();
        }
        else
        {
            _playerMovement.enabled = false; // stop Player from moving.
            _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
            _endScreen.enabled = true; // show end screen.
        }
    }

    public LayerMask groundLayer;

    void UpdateYPositionOnGround()
    {
        _player.transform.position = new Vector3(
            _player.transform.position.x,
            this.gameObject.transform.position.y + 4.0f,
            _player.transform.position.z
            );
        Debug.Log("On top of the obstacle, y pos is " + _player.transform.position.y);
    }

    //void CheckForSideCollisions()
    //{
    //    RaycastHit hit;
    //    // Raycast forward to detect obstacles in the direction of movement
    //    if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1.0f, groundLayer)) // 1f is the lookahead distance, adjust as necessary
    //    {
    //        if (hit.collider.CompareTag("Obstacle"))
    //        {
    //            Debug.Log("Dead");
    //        }
    //    }
    //}
}
