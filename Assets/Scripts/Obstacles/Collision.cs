using UnityEngine;

public class Collision : MonoBehaviour
{
    public GameObject ScreenController;
    public GameObject Player;
    public GameObject PlayerModel;
    private Movement _playerMovement;
    private Animator _playerAnimator;

    public void Start()
    {
        _playerMovement = Player.GetComponent<Movement>();
        _playerAnimator = PlayerModel.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _playerMovement.enabled = false; // stop player from moving.
        _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
        ScreenController.GetComponent<ShowEndScreen>().enabled = true; // show end screen.
    }
}
