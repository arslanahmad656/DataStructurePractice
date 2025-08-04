using LinkedLists.Nodes;

namespace LinkedLists;

public class DoublyList<T>
{
    private NodeDoubly<T>? head;
    private NodeDoubly<T>? tail;
    private readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

    public void Append(T value)
    {
        if (InsertFirst(value))
        {
            return;
        }

        tail!.Next = new()
        {
            Data = value,
            Previous = tail,
        };

        tail = tail.Next;
    }

    public void Prepend(T value)
    {
        if (InsertFirst(value))
        {
            return;
        }

        var node = new NodeDoubly<T>()
        {
            Data = value,
            Next = head,
        };

        head!.Previous = node;
        head = node;
    }

    public bool IsPalindrome()
    {
        if (head is null || tail is null)
        {
            return false;
        }

        for (NodeDoubly<T>? ptr1 = head, ptr2 = tail; ptr1 is not null && ptr2 is not null && !ReferenceEquals(ptr1, ptr2); ptr2 = ptr2.Previous, ptr1 = ptr1.Next)
        {
            if (!comparer.Equals(ptr1.Data, ptr2.Data))
            {
                return false;
            }
        }

        return true;
    }

    public void Reverse()
    {
        if (head is null || tail is null)
        {
            return;
        }

        if (ReferenceEquals(head, tail))
        {
            return;
        }

        (head, tail) = (tail, head);
    }

    private bool InsertFirst(T value)
    {
        if (head == null)
        {
            // tail must also be null since it's the first element to be inserted

            head = new() { Data = value };
            tail = head;

            return true;
        }

        return false;
    }

    public DoublyList<T?> DeepCopy()
    {
        var copy = new DoublyList<T?>();

        if (head is null)
        {
            return copy;
        }

        for (var curr = head; curr != null; curr = curr.Next)
        {
            copy.Append(curr!.Data);
        }

        return copy;
    }

    public DoublyList<T?> Merge(params DoublyList<T?>[] lists)
    {
        if (lists.Length == 0)
        {
            return new();
        }

        var mergedList = lists[0].DeepCopy();   // first list could be empty. this case will be handled in the loop.

        for (int i = 1; i < lists.Length; i++)
        {
            var currentList = lists[i].DeepCopy();
            if (currentList.head is null)
            {
                continue;
            }

            if (mergedList.head is null)
            {
                mergedList = currentList;
                continue;  // this is the first list in the merge hence we need to continue from the next one.
            }

            mergedList.tail!.Next = currentList.head!;
            currentList.head!.Previous = mergedList.tail;

            mergedList.tail = currentList.tail;
        }

        return mergedList;
    }
}
