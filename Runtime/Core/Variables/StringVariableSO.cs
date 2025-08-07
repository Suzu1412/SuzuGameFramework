using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StringVariableSO", menuName = "Scriptable Objects/Variable/StringVariableSO")]
public class StringVariableSO : ScriptableObject
{
    [SerializeField] private string _value;
    public string Value => _value;
    
    public event Action<string> OnValueChanged;

    public void SetValue(string value)
    {
        if (_value != value)
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
