using System;
using System.Collections;
using System.Collections.Generic;

public interface IRuntimeSet
{
    int Count { get; }
    IList Items { get; }
    UnityEngine.Object GetItem(int index);
    event Action OnItemsChanged;

    void Clear();
}