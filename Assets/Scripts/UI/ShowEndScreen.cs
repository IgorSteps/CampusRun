using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowEndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _endScreen;
    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        _endScreen.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
}
