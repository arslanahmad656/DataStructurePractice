namespace StacksAndQueues;

public class Deque<T>
{
    private class Node
    {
        public T Value;
        public Node? Prev;
        public Node? Next;

        public Node(T value)
        {
            Value = value;
        }
    }

    private Node? head;
    private Node? tail;
    private int count;

    public int Count => count;
    public bool IsEmpty => count == 0;

    // Add to front
    public void AddFront(T value)
    {
        var node = new Node(value);
        if (head == null)
        {
            head = tail = node;
        }
        else
        {
            node.Next = head;
            head.Prev = node;
            head = node;
        }
        count++;
    }

    // Add to back
    public void AddBack(T value)
    {
        var node = new Node(value);
        if (tail == null)
        {
            head = tail = node;
        }
        else
        {
            node.Prev = tail;
            tail.Next = node;
            tail = node;
        }
        count++;
    }

    // Remove from front
    public T RemoveFront()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty");
        var value = head!.Value;
        head = head.Next;
        if (head != null) head.Prev = null;
        else tail = null;
        count--;
        return value;
    }

    // Remove from back
    public T RemoveBack()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty");
        var value = tail!.Value;
        tail = tail.Prev;
        if (tail != null) tail.Next = null;
        else head = null;
        count--;
        return value;
    }

    // Peek front
    public T PeekFront()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty");
        return head!.Value;
    }

    // Peek back
    public T PeekBack()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty");
        return tail!.Value;
    }
}

