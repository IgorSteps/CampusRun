using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DistanceController handles distance calculation.
/// </summary>
public class DistanceController : MonoBehaviour
{
    public static string s_DistanceScore;
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private Transform _playerTransform;
    private Vector3 _startPosition;
    private float _distanceTravelled = 0f;

    void Start()
    {
        _startPosition = _playerTransform.position;
    }

    void Update()
    {
        _distanceTravelled = _playerTransform.position.z - _startPosition.z;
        _distanceText.text = ((int)_distanceTravelled).ToString("D6"); // format as 000000.
        s_DistanceScore = _distanceText.text;
    }
}
