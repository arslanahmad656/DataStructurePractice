using System.Diagnostics.CodeAnalysis;

namespace StacksAndQueues;

public class QueueBound<T> (int capacity)
{
    private T[] data = new T[capacity];
    private int front = 0;
    private int rear = -1;
    private int count = 0;

    public int Capacity => capacity;
    public int Count => count;
    public bool IsFull => count == Capacity;
    public bool IsEmpty => count == 0;

    public bool Enqueue(T item)
    {
        if (IsFull)
        {
            return false;
        }

        rear = (rear + 1) % Capacity;
        data[rear] = item;
        count++;

        return true;
    }

    public T? Dequeue([NotNullWhen(true)]out bool success)
    {
        if (IsEmpty)
        {
            success = false;
            return default;
        }

        var item = data[front];

        front = (front +  1) % Capacity;
        count--;

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

        var item = data[front];
        success = true;
        return item;
    }


}
