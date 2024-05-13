using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // To avoid unneccessary shaking.
    public bool IsShaking = false;
    public IEnumerator Shake()
    {
        IsShaking = true;
        Vector3 originalPosition = gameObject.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < Constants.SHAKE_DURATION)
        {
            float x = Random.Range(-1f, 1f) * Constants.SHAKE_FORCE;

            gameObject.transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original position.
        gameObject.transform.localPosition = originalPosition;
        IsShaking = false;
    }
}
