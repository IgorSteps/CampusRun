using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{
    public static int s_Score = 0;
    public GameObject ScoreNumber;

    // Update is called once per frame
    void Update()
    {
        ScoreNumber.GetComponent<TextMeshProUGUI>().text = s_Score.ToString();
    }
}
