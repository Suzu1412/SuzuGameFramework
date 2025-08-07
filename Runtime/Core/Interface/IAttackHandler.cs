using UnityEngine;

public interface IAttackHandler
{
    /// <summary>
    /// Executes an attack in a specified direction from an optional fire point.
    /// </summary>
    void Attack(Transform firePoint = null);

    /// <summary>
    /// Cancels any ongoing attack or charge-up.
    /// </summary>
    void CancelAttack();

    /// <summary>
    /// Begins charging an attack (if supported).
    /// </summary>
    void BeginCharge(Transform firePoint = null);

    /// <summary>
    /// Releases the charged attack (if supported).
    /// </summary>
    void ReleaseCharge(Transform firePoint = null);
}
