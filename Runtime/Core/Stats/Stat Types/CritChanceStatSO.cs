using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Critical Chance Stat", fileName = "Critical Chance Stat")]
public class CritChanceStatSO : StatComponentSO
{
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_statName))
            _statName = "Critical Chance";

        _minValue = 0f;
        _maxValue = 1f;
#endif
    }
}