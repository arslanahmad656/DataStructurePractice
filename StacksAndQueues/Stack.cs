using LinkedLists;

namespace StacksAndQueues;

public class Stack<T>
{
    private readonly SinglyList<T> list = new();

    public bool IsEmpty => !list.HasData;

    public int Count { get; private set; }

    public void Push(T item)
    {
        list.Append(item);
        Count++;
    }

    public T? Peek() => list.GetTailValue();

    public T? Pop(out bool wasEmpty)
    {
        wasEmpty = false;

        if (IsEmpty)
        {
            wasEmpty = true;
            return default;
        }

        var topValue = Peek();
        list.DeleteAtTail();

        Count--;
        return topValue;
    }
}
