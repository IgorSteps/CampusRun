using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenController;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _playerModel;

    private Movement _playerMovement;
    private Animator _playerAnimator;

    public void Start()
    {
        _playerMovement = _player.GetComponent<Movement>();
        _playerAnimator = _playerModel.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("called in Collision");

        _playerMovement.enabled = false; // stop player from moving.
        _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
        _screenController.GetComponent<ShowEndScreen>().enabled = true; // show end screen.
    }
}
