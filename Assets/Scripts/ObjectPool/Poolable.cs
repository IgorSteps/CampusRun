using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Poolable is a component that is assigned to every game object in the pool to keep
/// track of pool name for recycling.
/// </summary>
public class Poolable : MonoBehaviour
{
    public string poolName;
}
