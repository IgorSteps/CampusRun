using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    [SerializeField] private AudioSource _coinCollectSound;

    private void Start()
    {
        _coinCollectSound = GameObject.Find("Audio").GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _coinCollectSound.Play();
            ScoreController.s_CoinScore += 1;
            gameObject.transform.SetParent(null);
            PoolManager.s_Instance.ReturnObject(gameObject);
        }
    }
}
