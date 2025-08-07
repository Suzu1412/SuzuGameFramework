using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3VariableSO", menuName = "Scriptable Objects/Variable/Vector3VariableSO")]
public class Vector3VariableSO : ScriptableObject
{
    [SerializeField] private Vector3 _value;
    public Vector3 Value => _value;

    public event Action<Vector3> OnValueChanged;

    public void SetValue(Vector3 value)
    {
        if (_value != value)
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public void Modify(Vector3 delta) => SetValue(_value + delta);
}
