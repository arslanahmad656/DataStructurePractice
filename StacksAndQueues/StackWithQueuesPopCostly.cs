namespace StacksAndQueues;

public class StackWithQueuesPopCostly<T>
{
    private readonly Queue<T> q1 = new();
    private readonly Queue<T> q2 = new();

    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    public void Push(T item)
    {
        GetQueues().NonEmptyQueue.Enqueue(item);
        Count++;
    }

    public T? Dequeue(out bool success)
    {
        success = !IsEmpty;
        if (IsEmpty)
        {
            return default;
        }

        var (emptyQ, nonEmptyQ) = GetQueues();
        TransferExceptLast(emptyQ, nonEmptyQ);

        var item = nonEmptyQ.Dequeue(out _);
        Count--;

        return item;
    }

    public T? Peek(out bool success)
    {
        success = !IsEmpty;
        if (IsEmpty)
        {
            return default;
        }

        var (emptyQ, nonEmptyQ) = GetQueues();
        TransferExceptLast(emptyQ, nonEmptyQ);

        var item = nonEmptyQ.Dequeue(out _);
        emptyQ.Enqueue(item!);

        return item;
    }

    private (Queue<T> EmptyQueue, Queue<T> NonEmptyQueue) GetQueues() =>
        q1.IsEmpty ? (q1, q2) : (q2, q1);

    private void TransferExceptLast(Queue<T> dest, Queue<T> src)
    {
        while (src.Count > 1)
        {
            dest.Enqueue(src.Dequeue(out _)!);
        }
    }
}
