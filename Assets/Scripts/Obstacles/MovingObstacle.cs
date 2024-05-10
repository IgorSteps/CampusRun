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

        if (other.CompareTag("Obstacle"))
        {
            Explode(other);
        }
    }

    void Explode(Collider crate)
    {
        PoolManager.s_Instance.ReturnObject(crate.gameObject, "Obstacle");
        
        GameObject fracturedCrate = PoolManager.s_Instance.GetObject("FracturedCrate");
        // Set parent to the section.
        fracturedCrate.transform.SetParent(crate.gameObject.transform.parent);
        // Set position to the position of the original create.
        fracturedCrate.transform.position = crate.gameObject.transform.position;
        foreach (Transform t in fracturedCrate.transform)
        {
            var rigidBody = t.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(1.0f, crate.gameObject.transform.position, 1.0f);
            }

            StartCoroutine(Shrink(t, 2));
        }

        // Delay the recycle to give the time for animation to finish.
        StartCoroutine(Recycle(fracturedCrate, 5));
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

    IEnumerator Recycle(GameObject gameObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.s_Instance.ReturnObject(gameObj, "FracturedCrate");
    }
}
