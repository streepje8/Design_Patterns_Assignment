public class PoolItem<T>
{
    public T instance { get; private set; }
    public bool inUse = false;
    private bool isAdvanced = false;
    private IAdvancedPoolable advancedPoolable = null;
    public PoolItem(T instance)
    {
        this.instance = instance;
        isAdvanced = typeof(T).IsSubclassOf(typeof(IAdvancedPoolable));
        if (isAdvanced) advancedPoolable = (IAdvancedPoolable)instance;
    }

    public void Activate()
    {
        inUse = true;
        advancedPoolable?.onActivate();
    }

    public void DeActivate()
    {
        inUse = false;
        advancedPoolable?.onDeActivate();
    }
}
