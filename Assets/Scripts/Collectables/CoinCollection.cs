using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private AudioSource _coinCollectSound;

    private void Start()
    {
        _coinCollectSound = GameObject.Find("Audio").GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        _coinCollectSound.Play();
        ScoreController.s_CoinScore += 1;
        PoolManager.s_Instance.ReturnObject(gameObject, "Coin");
    }
}
