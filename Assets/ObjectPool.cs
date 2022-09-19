using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnityObjectPool<T> : ObjectPool<T> where T : UnityEngine.Object
{
    private readonly T original;

    public UnityObjectPool(T original)
    {
        this.original = original;
    }

    protected override T CreateInstance()
    {
        return Object.Instantiate(original,Vector3.zero,Quaternion.identity);
    }
}

public class ObjectPool<T>
{
    
    private Dictionary<T, PoolItem<T>> lookupTable = new Dictionary<T, PoolItem<T>>();

    public T GetInstance()
    {
        foreach (PoolItem<T> item in lookupTable.Values)
        {
            if (!item.inUse)
            {
                item.inUse = true;
                return item.instance;
            }
        }

        T instance = CreateInstance();
        lookupTable.Add(instance, new PoolItem<T>(instance));
        return instance;
    }

    protected virtual T CreateInstance()
    {
        return (T)Activator.CreateInstance(typeof(T));
    }
    
    public void FreeInstance(T instance)
    {
        if (lookupTable.TryGetValue(instance, out PoolItem<T> found))
        {
            found.inUse = false;
        }
        else
        {
            Debug.LogWarning("Tried to free an object that wasn't in the pool!");
        }
    }
}
