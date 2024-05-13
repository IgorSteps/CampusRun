using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        ScoreController.s_CoinScore = 0;
        SceneManager.LoadScene(1);
    }
}
