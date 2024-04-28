using UnityEngine;

/// <summary>
/// Collision handels collision response with obstacles.
/// </summary>
public class Collision : MonoBehaviour
{
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Animator _playerAnimator;

    public void Start()
    {
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerMovement.enabled = false; // stop Player from moving.
        _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
        _endScreen.enabled = true; // show end screen.
    }
}
