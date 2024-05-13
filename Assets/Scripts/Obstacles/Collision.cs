using System.Collections;
using UnityEngine;

/// <summary>
/// Collision handels collision response with obstacles.
/// </summary>
public class Collision : MonoBehaviour
{
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Invincible _playerInvinciblePowerUp;
    private Visibility _playerVisiblity;
    private Animator _playerAnimator;
    private GameObject _mainCamera;
    private CameraShake _shaker;

    public void Start()
    {
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        _playerInvinciblePowerUp = GameObject.Find("Player").GetComponent<Invincible>();
        _playerVisiblity = GameObject.Find("Player").GetComponent<Visibility>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
        _mainCamera = GameObject.Find("Main Camera");
        _shaker = _mainCamera.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_playerInvinciblePowerUp.IsInvincible)
        {
            Vector3 diff = other.transform.position - this.gameObject.transform.position;
            float heightDiff = diff.y;
            float horizontalDiff = Mathf.Abs(diff.x); // Makes sure it's positive for equality checks.

            if (heightDiff > Constants.RUN_ON_TOP_OF_CRATE_THRESHOLD) // Top collision.
            {
                return;
            }
            else if (horizontalDiff > Constants.SIDE_COLLISION_THRESHOLD) // Side collision.
            {
                _playerMovement.OnSideCollision();
                if (!_shaker.IsShaking && !_playerInvinciblePowerUp.IsInvincible)
                {
                    StartCoroutine(_playerVisiblity.ToggleVisibility(Constants.VISIBILITY_EFFECT_DURATION, Constants.VISIBILITY_INTERVAL_DURATION));
                    StartCoroutine(_playerInvinciblePowerUp.MakeInvincible(Constants.INVINIBILTIY_EFFECT_DURATION));
                    StartCoroutine(_shaker.Shake());
                }
            }
            else // Front collision.
            {
                _playerMovement.enabled = false; // stop Player from moving.
                _playerAnimator.Play("Stumble Backwards");
                _endScreen.enabled = true; // show end screen.
            }

        }
    }
}
