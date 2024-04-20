using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _sectionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Instantiate(_sectionPrefab, new Vector3(0, 0, transform.position.z + 15), Quaternion.identity);
            Debug.Log("Created new section");
        }
    }
}
