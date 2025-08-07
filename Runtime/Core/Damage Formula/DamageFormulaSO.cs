using UnityEngine;

public abstract class DamageFormulaSO : ScriptableObject
{
    [SerializeField] private DamageType _damageType = DamageType.Physical;
    [SerializeField] private bool _allowCrit = true;

    public DamageType DamageType => _damageType;
    public bool AllowCrit => _allowCrit;

    public abstract HitInfo Calculate(GameObject attacker, GameObject target);
}
