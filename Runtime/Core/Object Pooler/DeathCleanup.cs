using UnityEngine;

public class DeathCleanup : PoolableBehaviour
{
    private IDamageable _damageable;

    private void Awake()
    {
        _damageable = GetComponentInChildren<IDamageable>();
    }

    private void OnEnable()
    {
        if (_damageable != null)
            _damageable.OnDeath += ReturnToPool;
    }

    private void OnDisable()
    {
        if (_damageable != null)
            _damageable.OnDeath -= ReturnToPool;
    }
}