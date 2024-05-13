using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    private ShowEndScreen _endScreen;
    private Movement _playerMovement;
    private Invincible _playerInvinciblePowerUp;
    private Animator _playerAnimator;
    private GameObject _player;

    public void Start()
    {
        _player = GameObject.Find("Player");
        _endScreen = GameObject.Find("Screen").GetComponent<ShowEndScreen>();
        _playerMovement = _player.GetComponent<Movement>();
        _playerAnimator = GameObject.Find("Aj@Running").GetComponent<Animator>();
        _playerInvinciblePowerUp = _player.GetComponent<Invincible>();
    }

    void Update()
    {
        if (_player.transform.position.z + Constants.CAR_MOVE_OFFSET > gameObject.transform.position.z)
        {
            transform.Translate(Constants.CAR_SPEED * Time.deltaTime * Vector3.back, Space.World);
        }
        // Once passes the player - disable this script, so we don't do unnecessary explosion behind the player.
        if (_player.transform.position.z - Constants.CAR_MOVE_OFFSET / 2 > gameObject.transform.position.z)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_playerInvinciblePowerUp.IsInvincible)
        {
            _playerMovement.enabled = false; // stop Player from moving.
            _playerAnimator.Play("Stumble Backwards"); // play stumble animation.
            _endScreen.enabled = true; // show end screen.
        }

        if (other.CompareTag("Obstacle"))
        {
            Explode(other);
        }
    }

    void Explode(Collider crate)
    {
        // Recycle old crate
        crate.gameObject.transform.SetParent(null);
        PoolManager.s_Instance.ReturnObject(crate.gameObject);

        // Get the fractured crate in its place.
        GameObject fracturedCrate = PoolManager.s_Instance.GetObject("FracturedCrate");
        fracturedCrate.transform.SetParent(crate.gameObject.transform.parent);
        fracturedCrate.transform.position = crate.gameObject.transform.position;
        SetupFracturedCrate(fracturedCrate);

        foreach (Transform t in fracturedCrate.transform)
        {
            var rigidBody = t.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(1.0f, crate.gameObject.transform.position, 1.0f);
            }

            StartCoroutine(Shrink(t, 2));
        }
    }

    IEnumerator Shrink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 newScale = t.localScale;
        while (newScale.x >= 0)
        {
            newScale -= new Vector3(4f, 4f, 4f);
            if (newScale.x < 0) {
                newScale.x = 0;
                newScale.y = 0;
                newScale.z = 0;
            }

            t.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void SetupFracturedCrate(GameObject fracturedCrate)
    {
        // Setup inner pieces.
        int numberOfInnerPieces = 3;
        for (int i = 0; i < numberOfInnerPieces; i++)
        {
            string pieceName = "InnerFrame_cell." + i.ToString("D3");  // Formats i as 000.
            GameObject piece = PoolManager.s_Instance.GetObject(pieceName);
            piece.transform.SetParent(fracturedCrate.transform, false);
        }

        // Setup outer pieces.
        int numberOfOuterPieces = 15;
        for (int i = 0; i < numberOfOuterPieces; i++)
        {
            string pieceName = "OuterFrame_cell." + i.ToString("D3");  // Formats i as 000.
            GameObject piece = PoolManager.s_Instance.GetObject(pieceName);
            piece.transform.SetParent(fracturedCrate.transform, false);
        }
    }

}
