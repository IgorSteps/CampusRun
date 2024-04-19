using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    [SerializeField]
    private AudioSource _coinCollectSound;
    private void OnTriggerEnter(Collider other)
    {
        _coinCollectSound.Play();
        ScoreControl.s_Score += 1;
        this.gameObject.SetActive(false);
    }
}
