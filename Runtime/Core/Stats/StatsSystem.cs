using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private StatsTemplateSO _template;

    private readonly Dictionary<StatComponentSO, Stat> _stats = new();
    private readonly Dictionary<System.Type, Stat> _statsByType = new();
    private bool _isInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _stats.Clear();
        _statsByType.Clear();

        foreach (var statEntry in _template.StatValues)
        {
            var stat = new Stat(statEntry.StatComponent, statEntry.BaseValue);
            _stats[statEntry.StatComponent] = stat;
            _statsByType[statEntry.StatComponent.GetType()] = stat;
        }
        _isInitialized = true;
    }

    public Stat GetStat(StatComponentSO statComponent)
    {
        if (!_isInitialized) Initialize();

        if (_stats.TryGetValue(statComponent, out var stat))
        {
            return stat;
        }
        Debug.LogWarning($"Stat '{statComponent.name}' of type {statComponent.GetType().Name} not found in {transform.parent?.name}!");
        return null;
    }

    public Stat GetStat<T>() where T : StatComponentSO
    {
        if (!_isInitialized) Initialize();

        if (_statsByType.TryGetValue(typeof(T), out var stat))
        {
            return stat;
        }

        Debug.LogWarning($"Stat of type {typeof(T).Name} not found in {transform.parent?.name}!");
        return null;
    }

    public float GetValue(StatComponentSO statComponent) => GetStat(statComponent)?.Value ?? 0f;
    public float GetMin(StatComponentSO statComponent) => statComponent.MinValue;
    public float GetMax(StatComponentSO statComponent) => statComponent.MaxValue;

    /// <summary>
    /// Retrieves the current maximum health value from the associated <see cref="HealthStatSO"/>.
    /// Returns 0 if the stat is not found or uninitialized.
    /// </summary>
    /// <returns>The current value of the health stat, or 0 if not available.</returns>
    public float GetMaxHealth() => GetStat<HealthStatSO>()?.Value ?? 0f;
    /// <summary>
    /// Retrieves the current Attack value from the associated <see cref="AttackStatSO"/>.
    /// Returns 0 if the stat is not found or uninitialized.
    /// </summary>
    /// <returns>The current value of the health stat, or 0 if not available.</returns>
    public float GetAttack() => GetStat<AttackStatSO>()?.Value ?? 0f;


    public void AddModifier(StatModifier modifier)
    {
        GetStat(modifier.StatComponent)?.AddModifier(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        GetStat(modifier.StatComponent)?.RemoveModifier(modifier);
    }

    public void AddTemporaryModifier(StatModifier modifier, float duration)
    {
        AddModifier(modifier);
        StartCoroutine(RemoveAfterDelay(modifier, duration));
    }

    private IEnumerator RemoveAfterDelay(StatModifier modifier, float duration)
    {
        yield return Helpers.GetWaitForSeconds(duration);
        RemoveModifier(modifier);
    }

    public void RemoveAllModifiers()
    {
        foreach (var stat in _stats.Values)
            stat.RemoveAllModifiers();
    }

    public void RecalculateAll()
    {
        foreach (var stat in _stats.Values)
            stat.RecalculateValue();
    }

    #if UNITY_EDITOR
    [ContextMenu("Validate Stat Types")]
    private void ValidateStatTypes()
    {
        var seen = new HashSet<System.Type>();
        foreach (var entry in _template.StatValues)
        {
            var type = entry.StatComponent.GetType();
            if (!seen.Add(type))
            {
                Debug.LogWarning($"Duplicate stat type found: {type.Name}", entry.StatComponent);
            }
        }
    }
    #endif
}
