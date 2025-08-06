namespace StacksAndQueues;

public class QueueWithStacks<T>
{
    private readonly Stack<T> inStack = new();
    private readonly Stack<T> outStack = new();

    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public void Enqueue(T item)
    {
        inStack.Push(item);
        Count++;
    }

    public T? Dequeue(out bool success)
    {
        // If outStack is empty, move all items from inStack to outStack
        // This reverses the order, so the oldest element ends up on top of outStack — achieving FIFO

        success = !IsEmpty;
        PreparePopStack();
        Count--;
        return outStack.Pop(out _);
    }

    public T? Peek(out bool success)
    {
        success = !IsEmpty;
        PreparePopStack();
        return outStack.Peek();
    }

    private void PreparePopStack()
    {
        if (outStack.IsEmpty)
        {
            while (!inStack.IsEmpty)
            {
                outStack.Push(inStack.Pop(out _)!);
            }
        }
    }
}
