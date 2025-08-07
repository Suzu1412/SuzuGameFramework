using UnityEngine;
using System;

public interface IDamageable
{
    void TakeDamage(HitInfo hit);
    GameObject GameObject { get; }
    void Kill(GameObject gameObject, DeathCauseType cause);
    event Action OnDeath;
}