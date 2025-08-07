using UnityEngine;

[System.Serializable]
public class StatModifier
{
    [SerializeField] private ModifierType _modifierType;
    [SerializeField] private StatComponentSO _statComponent;
    [SerializeField][Tooltip("Use 1.0+ for multiplicative increases")] private float _value = 1.0f;

    [SerializeField] private ScriptableObject _source;

    public ModifierType ModifierType { get => _modifierType; internal set => _modifierType = value; }
    public StatComponentSO StatComponent { get => _statComponent; internal set => _statComponent = value; }
    public float Value { get => _value; internal set => _value = value; }

    public ScriptableObject Source => _source;

    public StatModifier(ModifierType modifierType, StatComponentSO statComponent, float value, ScriptableObject source = null)
    {
        _modifierType = modifierType;
        _statComponent = statComponent;
        _value = value;
        _source = source;
    }

    public void SetSource(ScriptableObject source)
    {
        _source = source;
    }
}
