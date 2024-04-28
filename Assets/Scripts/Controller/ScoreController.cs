using TMPro;
using UnityEngine;

/// <summary>
/// ScoreController controls the coin score displayed in the main level scene.
/// </summary>
public class ScoreController : MonoBehaviour
{
    public static int s_CoinScore = 0;

    [SerializeField] private GameObject _scoreNumber;
    private TextMeshProUGUI _scoreComponent;

    private void Start()
    {
        _scoreComponent = _scoreNumber.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _scoreComponent.text = s_CoinScore.ToString();
    }
}
