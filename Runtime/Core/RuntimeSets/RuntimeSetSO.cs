using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSetSO<T> : ScriptableObject, IRuntimeSet
{
    [SerializeField] private List<T> _items = new();
    public IReadOnlyList<T> Items => _items;
    public event Action OnItemsChanged;

    protected void OnEnable()
    {
        Clear();
    }

    public virtual void Add(T item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            OnItemsChanged?.Invoke();
        }
    }

    public virtual void Remove(T item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            OnItemsChanged?.Invoke();
        }
    }

    public T GetFirstItem()
    {
        return Items[0];
    }

    public T GetLastItem()
    {
        return Items[^1];
    }

    public T GetRandomItem()
    {
        return Items[UnityEngine.Random.Range(0, Items.Count)];
    }

    public T GetItemIndex(int index)
    {
        return Items[index];
    }

    public int Count => Items.Count;

    // For editor use only
    IList IRuntimeSet.Items => _items;
    UnityEngine.Object IRuntimeSet.GetItem(int index)
    {
        return _items[index] as UnityEngine.Object;
    }

    public void Clear()
    {
        _items.Clear();
        OnItemsChanged?.Invoke();
    }
}
