using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnityObjectPool<T> : ObjectPool<T> where T : Object
{
    private readonly T original;
    public UnityObjectPool(T original) => this.original = original;
    protected override T CreateInstance() => Object.Instantiate(original, Vector3.zero, Quaternion.identity);

    protected override void DeleteInstance(T instance) => Object.Destroy(instance);
}

public class ObjectPool<T>
{
    
    public bool isDynamic = true;
    public int maxPoolSize = 20;
    public int maxFreeItems = 2;

    private Dictionary<T, PoolItem<T>> lookupTable = new Dictionary<T, PoolItem<T>>();

    public ObjectPool()
    {
        if (!isDynamic)
        {
            while (lookupTable.Values.Count < maxPoolSize)
            {
                FreeInstance(GetInstance());
            }
        }
    }

    public T GetInstance()
    {
        foreach (PoolItem<T> item in lookupTable.Values)
            if (!item.inUse)
            {
                item.Activate();
                return item.instance;
            }

        if (isDynamic || lookupTable.Values.Count < maxPoolSize)
        {
            T instance = CreateInstance();
            PoolItem<T> poolItem = new PoolItem<T>(instance);
            lookupTable.Add(instance, poolItem);
            poolItem.Activate();
            return instance;
        }
        else
        {
            Debug.LogError("Pool exceeded the maximum size!");
            return default(T);
        }
    }

    protected virtual T CreateInstance() => (T)Activator.CreateInstance(typeof(T));

    public void FreeInstance(T instance)
    {
        if (lookupTable.TryGetValue(instance, out PoolItem<T> found)) found.DeActivate();
        else Debug.LogWarning("Tried to free an object that wasn't in the pool!");
        if (isDynamic)
        {
            List<PoolItem<T>> freeItems = lookupTable.Values.Where(item => !item.inUse).ToList();
            if (freeItems.Count > maxFreeItems)
            {
                lookupTable.Remove(freeItems[0].instance);
                DeleteInstance(freeItems[0].instance);
            }
        }
    }

    protected virtual void DeleteInstance(T instance) { }
}
