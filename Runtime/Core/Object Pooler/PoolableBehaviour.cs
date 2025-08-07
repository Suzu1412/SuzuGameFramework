using System.Collections;
using UnityEngine;

public abstract class PoolableBehaviour : MonoBehaviour, IPoolable
{
    protected Coroutine _returnCoroutine;
    protected ObjectPoolSO _pool;

    public bool IsInPool { get; set; }

    public virtual void OnGetFromPool() { }
    public virtual void OnReturnToPool() { StopAllCoroutines(); }

    public void ReturnToPool()
    {
        if (_pool == null)
        {
            Debug.LogWarning($"{name} has no pool assigned.");
            Destroy(gameObject);
        }
        else
        {
            _pool.Return(gameObject);
        }
    }

    public void ReturnToPoolAfter(float seconds)
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);
        _returnCoroutine = StartCoroutine(DelayReturn(seconds));
    }

    private IEnumerator DelayReturn(float seconds)
    {
        yield return Helpers.GetWaitForSeconds(seconds);
        ReturnToPool();
    }

    public void AssignPool(ObjectPoolSO pool)
    {
        _pool = pool;
    }

}
