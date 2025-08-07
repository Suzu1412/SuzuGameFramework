using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pools/ObjectPool")]
public class ObjectPoolSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _initialCapacity = 10;
    [SerializeField] private int _maxCapacity = 100;

    private Stack<GameObject> _pool = new();
    private Transform _container;

    private bool _initialized = false;

    private void OnEnable()
    {
        _initialized = false;
    }

    public void Initialize()
    {
        if (_initialized) return;

        _container = new GameObject($"Pool_{_prefab.name}").transform;

        for (int i = 0; i < _initialCapacity; i++)
        {
            _pool.Push(CreateInstance());
        }

        _initialized = true;
    }

    public void Prewarm(int amount)
    {
        Initialize();

        int toPrewarm = Mathf.Min(amount, _maxCapacity - _pool.Count);
        for (int i = 0; i < toPrewarm; i++)
        {
            var obj = CreateInstance();
            _pool.Push(obj);
        }
    }

    private GameObject CreateInstance()
    {
        var obj = Instantiate(_prefab, _container);
        obj.SetActive(false);

        if (obj.TryGetComponent<IPoolable>(out var poolable))
            poolable.AssignPool(this);

        return obj;
    }

    public GameObject Get()
    {
        Initialize();

        var obj = _pool.Count > 0 ? _pool.Pop() : CreateInstance();
        obj.SetActive(true);
        obj.transform.SetParent(_container);

        if (obj.TryGetComponent<IPoolable>(out var poolable))
            poolable.OnGetFromPool();
             poolable.IsInPool = false; // Mark it as in-use
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            // Prevent double return
            if (poolable.IsInPool)
            {
                Debug.LogWarning($"Trying to return object already in pool: {obj.name}");
                return;
            }

            poolable.OnReturnToPool();
            poolable.IsInPool = true;
        }

        if (_pool.Count < _maxCapacity)
        {
            obj.SetActive(false);
            _pool.Push(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
