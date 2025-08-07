using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Damage Formula/Attack Based")]
public class AttackBasedDamageFormula : DamageFormulaSO
{
    [SerializeField] private float _statMultiplier = 1f;

    public override HitInfo Calculate(GameObject attacker, GameObject target)
    {
        float finalDamage = 0f;
        bool isCritical = false;

        if (attacker.TryGetComponent(out StatsSystem attackerStats) &&
            target.TryGetComponent(out StatsSystem targetStats))
        {
            float attack = attackerStats.GetStat<AttackStatSO>()?.Value ?? 0f;
            float defense = targetStats.GetStat<DefenseStatSO>()?.Value ?? 0f;

            float effectiveDamage = attack * _statMultiplier;
            finalDamage = Mathf.Clamp(effectiveDamage - defense, 1, 9999);

            if (AllowCrit)
            {
                float critChance = attackerStats.GetStat<CritChanceStatSO>()?.Value ?? 0f;
                if (Random.value < critChance)
                {
                    isCritical = true;
                    float critMult = attackerStats.GetStat<CritMultiplierStatSO>()?.Value ?? 1.5f;
                    finalDamage *= critMult;
                }
            }
        }

        finalDamage = Mathf.Round(finalDamage);

        return new HitInfo(attacker, finalDamage, DamageType, isCritical);
    }
}