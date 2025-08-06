using System.Security;
using LinkedLists.Helpers;
using LinkedLists.Nodes;

namespace LinkedLists;

public class DoublyList<T>
{
    private NodeDoubly<T>? head;
    private NodeDoubly<T>? tail;
    private readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

    public int Length
    {
        get
        {
            int count = 0;

            for (var curr = head; curr != null; curr = curr.Next, count++)
            {
                // empty
            }

            return count;
        }
    }

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

    public T? ElementAtHead() => head is null ? default : head.Data;

    public T? ElementAtTail() => tail is null ? default : tail.Data;

    public bool RemoveAtHead()
    {
        if (head is null)
        {
            return false;
        }

        if (head == tail)
        {
            head = tail = null;
        }

        head = head!.Next;
        head!.Previous!.Next = null;
        head.Previous = null;

        return true;
    }

    public bool RemoveAtTail()
    {
        if (tail is null)
        {
            return false;
        }

        if (tail == head)
        {
            return false;
        }

        tail = tail.Previous;
        tail!.Next!.Previous = null;
        tail.Next = null;

        return true;
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

    private bool SwapNodes(NodeDoubly<T> iNode, NodeDoubly<T> jNode)
    {
        if (iNode == jNode)
        {
            return false;
        }

        if (iNode.Next == jNode)
        {
            // adjacent when j follows i.
            // Cases handled:
            // null <-- A <--> X <--> Y <--> B --> null (none of the nodes at the border)
            // null <-- X <--> Y <--> A <--> B --> null (nodes to swap at the head)
            // null <-- A <--> B <--> X <--> Y --> null (nodes to swap at the tail)
            // null <-- X <--> Y --> null (only two nodes. hence both at the head and at the tail)

            var iPrev = iNode.Previous;
            var jNext = jNode.Previous;

            // neighbours
            if (iPrev is not null)
            {
                // iPrev is null when iNode is the head node
                iPrev.Next = jNode;
            }

            if (jNext is not null)
            {
                // jNext is null when jNode is the tail node
                jNext.Previous = iNode;
            }

            // self
            iNode.Previous = jNode;
            iNode.Next = jNext;
            jNode.Previous = iPrev;
            jNode.Next = iNode;
        }
        else if (jNode.Next == iNode)
        {
            // apart from the position of the nodes, the rest is the same as in the above if branch

            var jPrev = jNode.Previous;
            var iNext = iNode.Next;

            if (jPrev is not null)
            {
                jPrev.Next = iNode;
            }

            if (iNext is not null)
            {
                iNext.Previous = jNode;
            }

            jNode.Previous = iNode;
            jNode.Next = iNext;
            iNode.Previous = jPrev;
            iNode.Next = jNode;
        }
        else
        {
            // Case when the nodes to swap are not adjacent.
            // Handles the following cases:
            // null <-- A <--> X <--> B <--> C <--> Y <--> D --> null (none of the nodes at the borders)
            // null <-- X <--> A <--> B <--> Y <--> C --> null (one of the node at the head)
            // null <-- A <--> B <--> X <--> C <--> D <--> Y --> null (one of the node at the tail)

            var iPrev = iNode.Previous;
            var iNext = iNode.Next;
            var jPrev = jNode.Previous;
            var jNext = jNode.Next;

            // handle the links of the neighbours
            if (iPrev is not null)
            {
                // when the iNode is head, the iPrev is null.
                iPrev.Next = jNode;
            }

            if (iNext is not null)
            {
                // when the iNode is tail, the iNext is null
                iNext.Previous = jNode;
            }

            if (jPrev is not null)
            {
                // jPrev is null when jNode is the head node
                jPrev.Next = iNode;
            }

            if (jNext is not null)
            {
                // jNext is null when jNode is the tail node
                jNext.Previous = iNode;
            }

            // Now handle the own links
            iNode.Previous = jPrev;
            iNode.Next = jNext;
            jNode.Previous = iPrev;
            jNode.Next = iNext;
        }

        // check if head / tail needs to update
        if (iNode == head)
        {
            head = jNode;
        }
        else if (jNode == head)
        {
            head = iNode;
        }

        if (iNode == tail)
        {
            tail = jNode;
        }
        else if (jNode == tail)
        {
            tail = iNode;
        }

        return true;
    }

    public void Rotate(int k)
    {
        if (head == null)
        {
            return;
        }

        if (head == tail)
        {
            return;
        }

        var length = this.Length;
        var normalizedK = Helper.GetNormalizedKForRotation(k, length);

        if (normalizedK % length == 0)
        {
            return;
        }

        // at this point, 0 < normalizedK < length

        NodeDoubly<T> kthNode = head;
        for (int i = 1; i < normalizedK; i++)
        {
            kthNode = kthNode.Next!; // k is normalized, there is definitely going to be a non-null kthNode.Next
        }

        var kNext = kthNode.Next;

        tail!.Next = head;
        head.Previous = tail;
        kthNode.Next = null;
        kNext!.Previous = null;

        head = kNext;
        tail = kthNode;
    }
}
