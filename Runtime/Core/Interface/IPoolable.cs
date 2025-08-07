using UnityEngine;

public interface IPoolable
{
    void OnGetFromPool();
    void OnReturnToPool();
    void AssignPool(ObjectPoolSO pool);
    bool IsInPool { get; set; } // <-- add this

}
