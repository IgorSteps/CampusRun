using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ShowStartScreen handles showing the Main Menu after 8 seconds in the End Menu.
/// </summary>
public class ShowStartScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Beginning());
    }

    IEnumerator Beginning()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(0);
    }
}
