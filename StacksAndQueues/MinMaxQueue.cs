namespace StacksAndQueues;

public class MinMaxQueue<T>
{
    private readonly Queue<T> mainQ = new();
    private readonly Deque<T> dqMin = new();
    private readonly Deque<T> dqMax = new();
    private readonly Comparer<T> comparer = Comparer<T>.Default;

    public int Count => mainQ.Count;
    
    public bool IsEmpty => mainQ.IsEmpty;

    public T? Min => dqMin.PeekFront(out _);

    public T? Max => dqMax.PeekFront(out _);

    public void Enqueue(T item)
    {
        mainQ.Enqueue(item);

        while (!dqMin.IsEmpty && comparer.Compare(item, dqMin.PeekBack(out _)) < 0)
        {
            dqMin.DequeueBack(out _);
        }

        dqMin.EnqueueBack(item);

        while(!dqMax.IsEmpty && comparer.Compare(item, dqMax.PeekBack(out _)) > 0)
        {
            dqMax.DequeueBack(out _);
        }

        dqMax.EnqueueBack(item);
    }

    public T? Dequeue(out bool success)
    {
        success = !IsEmpty;
        if (IsEmpty)
        {
            return default;
        }

        var item = mainQ.Dequeue(out success);
        if (comparer.Compare(item, dqMin.PeekFront(out _)) == 0)
        {
            dqMin.DequeueFront(out _);
        }

        if (comparer.Compare(item, dqMax.PeekFront(out _)) == 0)
        {
            dqMax.DequeueFront(out _);
        }

        return item;
    }
}
