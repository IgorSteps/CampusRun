using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;
    [SerializeField] float _moveOffset = 20.0f;
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

    void Update()
    {
        if (_player.transform.position.z + _moveOffset > gameObject.transform.position.z)
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.back, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerMovement.enabled = false; // stop Player from moving.
            _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
            _endScreen.enabled = true; // show end screen.
        }
    }
}
