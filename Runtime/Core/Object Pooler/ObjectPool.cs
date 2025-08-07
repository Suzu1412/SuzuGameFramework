using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : ScriptableObject
{
    public abstract Component Get();
    public abstract void Return(Component obj);
}
