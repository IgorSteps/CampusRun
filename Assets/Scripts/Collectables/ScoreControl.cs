using TMPro;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{
    public static int s_Score = 0;
    public GameObject ScoreNumber;
    public GameObject FinalScoreNumber;

    // TODO: Maybe add a conditional if score hasn't changed don't do GetComponent
    // to boost performance?
    void Update()
    {
        ScoreNumber.GetComponent<TextMeshProUGUI>().text = s_Score.ToString();
        FinalScoreNumber.GetComponent<TextMeshProUGUI>().text = s_Score.ToString();
    }
}
