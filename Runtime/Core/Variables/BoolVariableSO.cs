using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariableSO", menuName = "Scriptable Objects/Variable/BoolVariableSO")]
public class BoolVariableSO : ScriptableObject
{
    [SerializeField] private bool _value;
    public bool Value => _value;

    public event Action<bool> OnValueChanged;

    public void SetValue(bool value)
    {
        if (_value != value)
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public void Toggle() => SetValue(!_value);
}
