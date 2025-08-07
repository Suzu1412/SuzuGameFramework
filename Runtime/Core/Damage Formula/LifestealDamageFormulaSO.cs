using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Damage Formula/Lifesteal")]
public class LifestealDamageFormulaSO : DamageFormulaSO
{
    [SerializeField] private float _baseDamage = 20f;
    [SerializeField] private StatComponentSO _scaleStat; // e.g., Strength
    [SerializeField] private float _scaleMultiplier = 1.0f;

    [SerializeField, Range(0f, 1f)] private float _lifestealPercent = 0.25f;

    public override HitInfo Calculate(GameObject attacker, GameObject target)
    {
        float finalDamage = 0f;

        if (attacker.TryGetComponent<StatsSystem>(out var stats))
        {
            float statValue = stats.GetStat(_scaleStat)?.Value ?? 0f;

            finalDamage = _baseDamage + statValue * _scaleMultiplier;

            //var resistance = target.GetComponent<ResistanceProfile>();
            //float resistanceMultiplier = resistance?.GetMultiplier(DamageType) ?? 1f;
            //finalDamage *= resistanceMultiplier;

            
        }

        return new HitInfo(
                source: attacker,
                amount: finalDamage,
                damageType: DamageType,
                isCritical: false,
                ignoreInvincibility: false,
                lifeStealPercent: _lifestealPercent
            );

    }
}
