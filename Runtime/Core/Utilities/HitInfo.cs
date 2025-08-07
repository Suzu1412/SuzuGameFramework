using UnityEngine;

public struct HitInfo
{
    public GameObject Source;       // Who caused the hit
    public float Amount;            // Damage or healing amount
    public Vector3? HitPoint;       // Optional: where on the target it hit
    public Vector3? HitDirection;   // Optional: direction of impact
    public DamageType DamageType;   // Optional: element or category of damage
    public bool IsCritical;         // Optional: critical hit flag
    public bool IgnoreInvincibility; // Optional: ignore invincibility state
    public float LifestealPercent;   // Optional: Recovers a certain amount of life


    public HitInfo(
        GameObject source,
        float amount,
        DamageType damageType = DamageType.Physical,
        bool isCritical = false,
        bool ignoreInvincibility = false,
        Vector3? hitPoint = null,
        Vector3? hitDirection = null,
        float lifeStealPercent = 0f
        )
    {
        Source = source;
        Amount = amount;
        DamageType = damageType;
        IsCritical = isCritical;
        IgnoreInvincibility = ignoreInvincibility;
        HitPoint = hitPoint;
        HitDirection = hitDirection;
        LifestealPercent = lifeStealPercent;
    }
}
