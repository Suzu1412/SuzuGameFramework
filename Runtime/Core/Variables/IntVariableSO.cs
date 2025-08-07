using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariableSO", menuName = "Scriptable Objects/Variable/IntVariableSO")]
public class IntVariableSO : ScriptableObject
{
    [SerializeField] private int _baseValue;
    [SerializeField] private int _minValue = 0;
    [SerializeField] private int _maxValue = 100;

    [SerializeField, ReadOnly] private int _value;

    public int Value => _value;
    public int BaseValue
    {
        get => _baseValue;
        internal set
        {
            int clamped = Mathf.Clamp(value, _minValue, _maxValue);
            if (_baseValue != clamped)
            {
                _baseValue = clamped;
                SetValue(clamped);
            }
        }
    }

    public int MinValue => _minValue;
    public int MaxValue => _maxValue;

    public float Ratio => _maxValue != 0 ? (float)_value / _maxValue : 0f;

    public event Action<int> OnValueChanged;

    private void OnEnable() => SetValue(_baseValue);
    private void OnValidate() => SetValue(_baseValue);

    public void Initialize(int baseValue, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        BaseValue = baseValue;
    }

    public void SetValue(int newValue)
    {
        int clamped = Mathf.Clamp(newValue, _minValue, _maxValue);
        if (_value != clamped)
        {
            _value = clamped;
            OnValueChanged?.Invoke(_value);
        }
    }

    public void ModifyValue(int delta) => SetValue(_value + delta);
}
