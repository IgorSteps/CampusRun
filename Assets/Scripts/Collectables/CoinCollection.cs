using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public AudioSource coinCollectSound;
    private void OnTriggerEnter(Collider other)
    {
        coinCollectSound.Play();
        ScoreControl.s_Score += 1;
        this.gameObject.SetActive(false);
    }
}
