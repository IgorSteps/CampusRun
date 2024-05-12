using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float pullForce = 5f;
    public float pullRadius = 5f;
    public LayerMask coinLayer;
    private bool isMagnetActive = false;
    private float magnetDuration = 10f;

    private void Update()
    {
        if (isMagnetActive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, pullRadius, coinLayer);
            foreach (var hitCollider in hitColliders)
            {
                Rigidbody rigidBody = hitCollider.GetComponent<Rigidbody>();

                if (rigidBody != null)
                {
                    Vector3 forceDirection = transform.position - hitCollider.transform.position;
                    rigidBody.MovePosition(rigidBody.position + pullForce * Time.deltaTime * forceDirection.normalized);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a Magnet object.
        if (other.gameObject.CompareTag("Magnet"))
        {
            ActivateMagnetEffect();
            PoolManager.s_Instance.ResetObject(other.gameObject);
        }
    }

    void ActivateMagnetEffect()
    {
        isMagnetActive = true;
        Invoke("DeactivateMagnetEffect", magnetDuration);
    }

    void DeactivateMagnetEffect()
    {
        isMagnetActive = false;
    }
}
