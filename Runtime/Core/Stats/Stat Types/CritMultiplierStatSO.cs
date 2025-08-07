using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Critical Damage Stat", fileName = "Critical Damage Stat")]
public class CritMultiplierStatSO : StatComponentSO
{
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_statName))
            _statName = "Critical Damage";

        _minValue = 1.5f;
        _maxValue = 5f;
#endif
    }
}