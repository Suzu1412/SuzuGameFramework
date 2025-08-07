using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariableSO", menuName = "Scriptable Objects/Variable/FloatVariableSO")]
public class FloatVariableSO : ScriptableObject
{
    [SerializeField] private float _baseValue;
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 100f;

    [SerializeField, ReadOnly, Tooltip("Clamped current value")] private float _value;

    public float Value => _value;
    public float BaseValue
    {
        get => _baseValue;
        internal set
        {
            float clamped = Mathf.Clamp(value, _minValue, _maxValue);
            if (!Mathf.Approximately(_baseValue, clamped))
            {
                _baseValue = clamped;
                SetValue(clamped);
            }
        }
    }

    public float MinValue => _minValue;
    public float MaxValue => _maxValue;

    public float Ratio => _maxValue != 0 ? (_value / _maxValue) : 0f;

    public event Action<float> OnValueChanged;

    private void OnEnable()
    {
        SetValue(_baseValue);
    }

    private void OnValidate()
    {
        SetValue(_baseValue);
    }

    public void Initialize(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        BaseValue = baseValue;
    }

    public void SetValue(float newValue)
    {
        float clamped = Mathf.Clamp(newValue, _minValue, _maxValue);
        if (!Mathf.Approximately(_value, clamped))
        {
            _value = clamped;
            OnValueChanged?.Invoke(_value);
        }
    }

    public void ModifyValue(float delta)
    {
        SetValue(_value + delta);
    }
}
