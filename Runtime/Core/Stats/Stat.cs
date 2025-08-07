using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField, ReadOnly] private StatComponentSO _statComponent;

    [SerializeField] private float _baseValue;

    [SerializeField, ReadOnly, Tooltip("Value is calculated by Base Value and Modifiers")] private float _value;

    [SerializeField] private List<StatModifier> _modifiers = new();

    public StatComponentSO StatComponent => _statComponent;
    public float Ratio => (_value - _statComponent.MinValue) / (_statComponent.MaxValue - _statComponent.MinValue);

    public float BaseValue
    {
        get => _baseValue;
        internal set
        {
            if (!Mathf.Approximately(_baseValue, value))
            {
                _baseValue = Mathf.Clamp(value, _statComponent.MinValue, _statComponent.MaxValue);
                RecalculateValue();
            }
        }
    }

    public float Value => _value;
    public IReadOnlyList<StatModifier> Modifiers => _modifiers;

    public event Action<float> OnValueChanged;

    public Stat(StatComponentSO statComponent, float baseValue)
    {
        _statComponent = statComponent;
        BaseValue = baseValue;
        RecalculateValue();
    }

    internal void RecalculateValue()
    {
        float sumPercentAdditive = 0f;
        float finalValue = _baseValue;

        for (int i = 0; i < _modifiers.Count; i++)
        {
            var mod = _modifiers[i];
            switch (mod.ModifierType)
            {
                case ModifierType.Flat:
                    finalValue += mod.Value;
                    break;

                case ModifierType.PercentAdditive:
                    sumPercentAdditive += mod.Value;
                    bool isLastAdd = (i + 1 >= _modifiers.Count || _modifiers[i + 1].ModifierType != ModifierType.PercentAdditive);
                    if (isLastAdd)
                        finalValue *= 1 + sumPercentAdditive;
                    break;

                case ModifierType.PercentMultiplicative:
                    finalValue *= 1 + mod.Value;
                    break;
            }
        }

        finalValue = Mathf.Clamp(finalValue, _statComponent.MinValue, _statComponent.MaxValue);

        if (!Mathf.Approximately(_value, finalValue))
        {
            _value = finalValue;
            OnValueChanged?.Invoke(_value);
        }
    }

    internal void AddModifier(StatModifier modifier)
    {
        if (modifier == null) return;
        if (_modifiers.Contains(modifier)) return; // optional deduplication
        _modifiers.Add(modifier);
        _modifiers.Sort(CompareModifierType);
        RecalculateValue();
    }

    internal void RemoveModifier(StatModifier modifier)
    {
        if (_modifiers.Remove(modifier))
            RecalculateValue();
    }

    internal void RemoveAllModifiers()
    {
        _modifiers.Clear();
        RecalculateValue();
    }

    internal void RemoveModifiersFromSource(ScriptableObject source)
    {
        _modifiers.RemoveAll(m => m.Source == source);
        RecalculateValue();
    }

    private int CompareModifierType(StatModifier x, StatModifier y)
    {
        return x.ModifierType.CompareTo(y.ModifierType);
    }
}