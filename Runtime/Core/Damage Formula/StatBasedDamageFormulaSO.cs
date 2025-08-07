using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Damage Formula/Stat Based")]
public class StatBasedDamageFormulaSO : DamageFormulaSO
{
    [SerializeField] private StatComponentSO _statToScaleWith; // e.g. Attack, Magic, etc.
    [SerializeField] private float _statMultiplier = 1f;

    public override HitInfo Calculate(GameObject attacker, GameObject target)
    {
        float finalDamage = 0f;
        bool isCritical = false;

        if (attacker.TryGetComponent<StatsSystem>(out var attackerStats))
        {
            float statValue = attackerStats.GetStat(_statToScaleWith)?.Value ?? 0f;
            finalDamage += statValue * _statMultiplier;

            //var resistance = target.GetComponent<ResistanceProfile>();
            //float resistanceMultiplier = resistance?.GetMultiplier(DamageType) ?? 1f;
            //finalDamage *= resistanceMultiplier;

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

        return new HitInfo(
            source: attacker,
            amount: finalDamage,
            damageType: DamageType,
            isCritical: isCritical
        );
    }
}
