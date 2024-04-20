using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOldSection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyer")){
            Destroy(gameObject);
            Debug.Log("Destroyed " + gameObject.name);
        }
    }
}
