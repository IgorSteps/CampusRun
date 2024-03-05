using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject ScoreDisplay;
    public int Score = 0;
    void Update()
    {
        ScoreDisplay.GetComponent<Text>().text = Score.ToString();
    }
}
