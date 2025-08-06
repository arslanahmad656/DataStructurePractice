namespace StacksAndQueues;

public class StackWithQueuesPushCostly<T>
{
    // Two queues used to simulate stack behavior
    private readonly Queue<T> q1 = new();
    private readonly Queue<T> q2 = new();

    // Tracks the number of elements in the simulated stack
    public int Count { get; private set; }

    // True if the stack is empty
    public bool IsEmpty => Count == 0;

    // Push operation (costly)
    // Inserts an item in such a way that the newest item is always at the front
    public void Push(T item)
    {
        // Identify which queue is empty and which has the current data
        var (emptyQ, nonEmptyQ) = GetQueues();

        // Enqueue the new item into the empty queue
        emptyQ.Enqueue(item);

        // Transfer all items from the non-empty queue to the one where we just added the new item
        Transfer(emptyQ, nonEmptyQ);

        Count++;
    }

    // Pop operation (cheap)
    // Removes and returns the top of the stack
    public T? Pop(out bool success)
    {
        success = !IsEmpty;

        if (IsEmpty)
        {
            return default;
        }

        // The top of the stack is always at the front of the non-empty queue
        var nonEmptyQueue = GetQueues().NonEmptyQueue;

        Count--;
        return nonEmptyQueue.Dequeue(out _);
    }

    // Peek operation (cheap)
    // Returns the top element without removing it
    public T? Peek(out bool success) => GetQueues().NonEmptyQueue.Peek(out success);

    // Returns a tuple with one empty and one non-empty queue
    private (Queue<T> EmptyQueue, Queue<T> NonEmptyQueue) GetQueues() =>
        q1.IsEmpty ? (q1, q2) : (q2, q1);

    // Transfers all elements from source to destination queue
    // Used to reverse the order to maintain LIFO behavior
    private static void Transfer(Queue<T> dest, Queue<T> src)
    {
        while (!src.IsEmpty)
        {
            dest.Enqueue(src.Dequeue(out _)!);
        }
    }
}

