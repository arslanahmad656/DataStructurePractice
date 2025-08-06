using LinkedLists;

namespace StacksAndQueues;

public class Deque1<T>
{
    // front is head, back is tail
    private readonly DoublyList<T> data = new();

    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public void EnqueueFront(T item)
    {
        // insert at head
        data.Prepend(item);
        Count++;
    }

    public T? DequeueFront(out bool success)
    {
        // get from head
        success = !IsEmpty;
        if (IsEmpty)
        {
            return default;
        }

        var item = data.ElementAtHead();
        data.RemoveAtHead();
        Count--;

        return item;
    }

    public void EnqueueBack(T item)
    {
        data.Append(item);
        Count++;
    }

    public T? DequeueBack(out bool success)
    {
        success = !IsEmpty;
        if (IsEmpty)
        {
            return default;
        }

        var item = data.ElementAtTail();
        data.RemoveAtTail();
        Count--;
        return item;
    }

    public T? PeekFront(out bool success)
    {
        success = !IsEmpty;
        return data.ElementAtHead();
    }

    public T? PeekBack(out bool success)
    {
        success = !IsEmpty;
        return data.ElementAtTail();
    }
}
