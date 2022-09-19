public class PoolItem<T>
{
    public T instance { get; private set; }
    public bool inUse = false;
    public PoolItem(T instance)
    {
        this.instance = instance;
    }
}
