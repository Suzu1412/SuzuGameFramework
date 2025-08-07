using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Health Stat", fileName = "Health Stat")]
public class HealthStatSO : StatComponentSO
{
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_statName))
            _statName = "Health";

        _minValue = 1f;
        _maxValue = 999f;
#endif
    }
}