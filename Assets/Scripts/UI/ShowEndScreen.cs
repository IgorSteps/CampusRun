using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowEndScreen : MonoBehaviour
{
    public GameObject EndScreen;

    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        EndScreen.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
}
