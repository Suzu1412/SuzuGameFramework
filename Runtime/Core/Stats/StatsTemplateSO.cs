using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stats/Character Template", fileName = "Character Template")]
public class StatsTemplateSO : ScriptableObject
{
    [SerializeField] private List<StatBaseValue> _statValues = new();

    public IReadOnlyList<StatBaseValue> StatValues => _statValues;

    public float GetBaseValue(StatComponentSO stat)
    {
        foreach (var entry in _statValues)
        {
            if (entry.StatComponent == stat)
                return entry.BaseValue;
        }
        return 0f;
    }

    private void OnValidate()
    {
        if (_statValues == null) return;

        foreach (var stat in _statValues)
        {
            if (stat.StatComponent == null) continue;
            stat.BaseValue = Mathf.Clamp(stat.BaseValue, stat.StatComponent.MinValue, stat.StatComponent.MaxValue);
        }
    }
}

[System.Serializable]
public class StatBaseValue
{
    public StatComponentSO StatComponent;
    public float BaseValue;
}
