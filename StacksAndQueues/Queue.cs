using LinkedLists;
using System.Diagnostics.CodeAnalysis;

namespace StacksAndQueues;

public class Queue<T>
{
    public SinglyList<T> data = new();

    public bool IsEmpty => !data.HasData;

    public int Count { get; private set; }

    public void Enqueue(T item)
    {
        data.Prepend(item);
        Count++;
    }

    public T? Dequeue([NotNullWhen(true)] out bool success)
    {
        if (IsEmpty)
        {
            success = false;
            return default;
        }

        var item = data.GetTailValue();
        data.DeleteAtTail();
        Count--;

        success = true;
        return item;
    }

    public T? Peek([NotNullWhen(true)] out bool success)
    {
        if (IsEmpty)
        {
            success = false;
            return default;
        }

        success = true;
        return data.GetTailValue();
    }
}
