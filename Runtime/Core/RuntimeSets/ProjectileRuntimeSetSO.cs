using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/RTS/Projectile RuntimeSet")]
public class ProjectileRuntimeSetSO : ScriptableObject
{
    [SerializeField] private HashSet<IProjectile> _items = new();
    public HashSet<IProjectile> Items => _items;
    public event Action OnItemsChanged;

    // Adapter for IRuntimeSet
    private void OnEnable() => _items.Clear();

    public void Add(IProjectile item)
    {
        if (_items.Add(item))
        {
            _items.Add(item);
            OnItemsChanged?.Invoke();
        }
    }

    public void Remove(IProjectile item)
    {
        if (_items.Remove(item))
        {
            _items.Remove(item);
            OnItemsChanged?.Invoke();
        }
    }

    public void Clear()
    {
        if (_items.Count > 0)
        {
            _items.Clear();
            OnItemsChanged?.Invoke();
        }
    }
}
