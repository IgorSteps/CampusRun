using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public AudioSource coinCollectSound;
    private void OnTriggerEnter(Collider other)
    {
        coinCollectSound.Play();
        this.gameObject.SetActive(false);
    }

}
