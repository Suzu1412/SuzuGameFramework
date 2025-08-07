using System;
using System.Collections;
using UnityEngine;

public abstract class BaseRuntimeSetSO : ScriptableObject
{
    public abstract IList GetItems();
    public abstract event Action OnItemsChanged;
}
