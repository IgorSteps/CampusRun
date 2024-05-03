using UnityEngine;

/// <summary>
/// Collision handels collision response with obstacles.
/// </summary>
public class Collision : MonoBehaviour
{
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Animator _playerAnimator;
    private GameObject _player;

    public void Start()
    {
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
        _player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            float heigthDiff = other.transform.position.y - this.gameObject.transform.position.y;
            if (heigthDiff > 1.5f)
            {
                // Run on top of the obstacle.
                return;
            }
            else
            {
                _playerMovement.enabled = false; // stop Player from moving.
                _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
                _endScreen.enabled = true; // show end screen.
            }
        }
    }
}
