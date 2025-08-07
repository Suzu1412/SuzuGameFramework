using UnityEngine;

public abstract class StatComponentSO : ScriptableObject
{
    [SerializeField] protected string _statName;
    [SerializeField] protected float _minValue = 0f;
    [SerializeField] protected float _maxValue = 9999f;

    public string StatName => _statName;
    public float MinValue => _minValue;
    public float MaxValue => _maxValue;
}