
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Attack Stat", fileName = "Attack Stat")]
public class AttackStatSO : StatComponentSO
{
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(_statName))
            _statName = "Attack";

        _minValue = 1f;
        _maxValue = 99f;
#endif
    }
}