using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private float _pullForce = 15f;
    private float _pullRadius = 5f;
    public LayerMask coinLayer;
    private bool _isMagnetActive = false;
    private float _magnetDuration = 10f;

    private void Update()
    {
        if (_isMagnetActive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _pullRadius, coinLayer);
            foreach (var hitCollider in hitColliders)
            {
                Rigidbody rigidBody = hitCollider.GetComponent<Rigidbody>();

                if (rigidBody != null)
                {
                    Vector3 forceDirection = transform.position - hitCollider.transform.position;
                    rigidBody.MovePosition(rigidBody.position + (_pullForce * Time.deltaTime * forceDirection.normalized));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a Magnet object.
        if (other.gameObject.CompareTag("Magnet"))
        {
            Activate();
            
            // Return to the pool.
            other.gameObject.transform.SetParent(null);
            PoolManager.s_Instance.ReturnObject(other.gameObject);
        }
    }

    void Activate()
    {
        _isMagnetActive = true;
        Invoke(nameof(Deactivate), _magnetDuration);
    }

    void Deactivate()
    {
        _isMagnetActive = false;
    }
}
