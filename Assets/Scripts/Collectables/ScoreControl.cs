using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{
    public static int s_Score = 0;
    public GameObject ScoreText;

    // Update is called once per frame
    void Update()
    {
        ScoreText.GetComponent<TextMeshProUGUI>().text = s_Score.ToString();
    }
}
