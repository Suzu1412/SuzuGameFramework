using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Defense Stat", fileName = "Defense Stat")]
public class DefenseStatSO : StatComponentSO
{
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_statName))
            _statName = "Defense";

        _minValue = 1f;
        _maxValue = 99f;
#endif
    }
}