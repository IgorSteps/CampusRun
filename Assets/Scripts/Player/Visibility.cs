using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    private SkinnedMeshRenderer _renderer;

    public void Start()
    {
        _renderer = GameObject.Find("Boy01_Body_Geo").GetComponent<SkinnedMeshRenderer>();
    }
    public IEnumerator ToggleVisibility(float duration, float interval)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            _renderer.enabled = !_renderer.enabled;

            yield return new WaitForSeconds(interval);
            timeElapsed += interval;
        }

        _renderer.enabled = true;
    }
}
