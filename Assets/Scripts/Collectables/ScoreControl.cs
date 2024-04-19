using TMPro;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{
    public static int s_Score = 0;

    [SerializeField]
    private GameObject _scoreNumber;
    [SerializeField]
    private GameObject _finalScoreNumber;

    private TextMeshProUGUI _scoreComponent;
    private TextMeshProUGUI _finalScoreText;

    private void Start()
    {
        _scoreComponent = _scoreNumber.GetComponent<TextMeshProUGUI>();
        _finalScoreText = _finalScoreNumber.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _scoreComponent.text = s_Score.ToString();
        _finalScoreText.text = s_Score.ToString();
    }
}
