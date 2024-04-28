using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// EndMenu handels setting final game stats like number of coins collected and distance covered.
/// </summary>
public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject _finalCoinScoreNumber;
    private TextMeshProUGUI _finalCoinScoreText;
    [SerializeField] private GameObject _finalDistanceScoreNumber;
    private TextMeshProUGUI _finalDistanceScoreText;

    // Start is called before the first frame update
    void Start()
    {
        _finalCoinScoreText = _finalCoinScoreNumber.GetComponent<TextMeshProUGUI>();
        _finalDistanceScoreText = _finalDistanceScoreNumber.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _finalCoinScoreText.text = ScoreController.s_CoinScore.ToString();
        _finalDistanceScoreText.text = DistanceController.s_DistanceScore;
    }
}
