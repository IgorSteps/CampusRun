using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ShowEndScree handles showing the End Menu scene.
/// </summary>
public class ShowEndScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(2);
    }
}
