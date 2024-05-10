using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Animator _playerAnimator;

    public void Start()
    {
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.back, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Car collided with player");
            _playerMovement.enabled = false; // stop Player from moving.
            _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
            _endScreen.enabled = true; // show end screen.
        }
    }
}
