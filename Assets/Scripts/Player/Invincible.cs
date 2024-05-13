using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    public bool IsInvincible;

    void Start()
    {
        IsInvincible = false;
    }

    public IEnumerator MakeInvincible(float duration)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
    }
}
