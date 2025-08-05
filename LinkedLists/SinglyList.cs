using LinkedLists.Nodes;
using System.Text;

namespace LinkedLists;

public partial class SinglyList<T>
{
    private NodeSingly<T>? head;
    private NodeSingly<T>? tail;
    private readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

    public int Length
    {
        get
        {
            int length = 0;
            for (var curr = head; curr != null; curr = curr.Next, length++)
            {
                // empty;
            }

            return length;
        }
    }

    public void Append(T data)
    {
        var node = new NodeSingly<T>
        {
            Data = data,
            Next = null,
        };

        if (head is null)
        {
            // means it's a first insertion.
            // tails must also be null

            head = node;
            tail = head;
        }
        else
        {
            tail!.Next = node;  // must never be null here
            tail = node;
        }
    }

    public void Prepend(T data)
    {
        var node = new NodeSingly<T>
        {
            Data = data,
            Next = head,
        };

        head = node;
        tail ??= node;  // if the list was empty before this
    }

    public bool InsertAt(T data, int index)
    {
        if (index < 0)
        {
            return false;
        }

        var node = new NodeSingly<T>
        {
            Data = data,
        };

        if (head is null)
        {
            if (index is not 0)
            {
                return false;
            }

            head = node;
            tail = node;

            return true;
        }

        int i = 0;
        var curr = head;
        NodeSingly<T>? prev = null;
        while (i < index && curr != null)
        {
            i++;
            prev = curr;
            curr = curr.Next;
        }

        if (i < index)
        {
            // means that there are not enough number of elements in the list
            return false;
        }

        if (prev is null)
        {
            // means node to be inserted before the head.
            node.Next = curr;   // current is head in this case
            head = node;
        }
        else
        {
            // means that node is to be inserted after prev
            prev.Next = node;
            node.Next = curr;

            if (curr is null)
            {
                // update tail if inserting at the end
                tail = node;
            }
        }

        return true;
    }

    public bool DeleteAtHead()
    {
        if (head is null)
        {
            return false;   // nothing to delete
        }

        head = head.Next;
        if (head is null)
        {
            tail = null;
        }

        return true;
    }

    public bool DeleteAtTail()
    {
        if (tail is null)
        {
            return false;
        }

        if (ReferenceEquals(head, tail))
        {
            head = null;
            tail = null;

            return true;
        }

        var nodeBeforeTail = head;
        for (; !ReferenceEquals(nodeBeforeTail!.Next, tail); nodeBeforeTail = nodeBeforeTail.Next)
        { }

        nodeBeforeTail.Next = null;
        tail = nodeBeforeTail;
        return true;
    }

    public bool DeleteAt(int index)
    {
        if (head is null)
        {
            return false;
        }

        if (index == 0)
        {
            // just move the head to its next node
            head = head.Next;
            if (head is null)
            {
                // means now the list is empty
                tail = null;
            }

            return true;
        }

        var prev = head;
        for (int i = 0; i < index - 1 && prev.Next != null; prev = prev.Next, i++)
        {
            // empty
        }

        if (prev?.Next is null)
        {
            // nothing to remove since there is nothing ahead.
            return false;
        }

        prev.Next = prev.Next.Next;
        if (prev.Next is null)
        {
            tail = prev;
        }

        return true;
    }

    public bool Remove(T value)
    {
        if (head is null)
        {
            return false;
        }

        if (comparer.Equals(head.Data, value))
        {
            head = head.Next;
            if (head is null)
            {
                tail = null;
            }

            return true;
        }

        var prev = head;
        while (prev.Next != null && !comparer.Equals(prev.Next.Data, value))
        {
            prev = prev.Next;
        }

        if (prev.Next is null)
        {
            // element not found
            return false;
        }

        prev.Next = prev.Next.Next;
        if (prev.Next is null)
        {
            // means last element was removed.
            tail = prev;
        }

        return true;
    }

    public int Find(T value)
    {
        if (head is null)
        {
            return -1;
        }

        int index = 0;
        var curr = head;
        for (; curr != null; curr = curr.Next, index++)
        {
            if (comparer.Equals(curr.Data, value))
            {
                return index;
            }
        }

        return -1;
    }

    public T? GetValue(int index)
    {
        if (index < 0 || head is null)
        {
            return default;
        }

        var curr = head;
        for (var i = 0; curr != null && i < index; i++, curr = curr.Next)
        {
            // empty
        }

        if (curr is null)
        {
            return default;
        }

        return curr.Data;
    }

    public void Iterate(Action<T?> action)
    {
        for (var curr = head; curr != null; curr = curr.Next)
        {
            action(curr.Data);
        }
    }

    public SinglyList<T?> Reverse()
    {
        if (head is null)
        {
            return new();
        }

        if (ReferenceEquals(head, tail))
        {
            var ls = new SinglyList<T?>();
            ls.Append(head.Data);

            return ls;
        }

        var reverseList = new SinglyList<T?>();
        for (var curr = head; curr != null; curr = curr.Next)
        {
            reverseList.Prepend(curr.Data);
        }

        return reverseList;
    }

    public T? GetMiddleElement()
    {
        if (head is null)
        {
            return default;
        }

        var length = this.Length;
        return GetValue(length / 2);
    }

    public string GetFormattedString()
    {
        if (head is null)
        {
            return string.Empty;
        }

        var strings = new SinglyList<string>();
        for (var curr = head; curr is not null; curr = curr.Next)
        {
            strings.Append(curr.Data?.ToString() ?? string.Empty);
        }

        var str = string.Join(" --> ", strings);
        return str;
    }

    public SinglyList<T?> DeepCopy()
    {
        var copy = new SinglyList<T?>();

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

    public SinglyList<T?> Merge(params SinglyList<T>[] lists)
    {
        if (lists.Length == 0)
        {
            return new();
        }

        var mergedList = lists[0].DeepCopy();

        for (int i = 1; i < lists.Length; i++)
        {
            var currentList = lists[i].DeepCopy();
            if (currentList.head == null)
            {
                // nothing to merge in this list.
                continue;
            }

            if (mergedList.head is null)
            {
                mergedList = currentList;
            }

            mergedList.tail!.Next = currentList.head!;
            mergedList.tail = currentList.tail!;
        }

        return mergedList;
    }

    private bool SwapNodes(NodeSingly<T> iNode, NodeSingly<T> jNode)
    {
        if (iNode is null || jNode is null)
        {
            return false;
        }

        if (iNode == jNode)
        {
            return false;
        }

        if (head == tail)
        {
            return false;  // nothing to swap
        }

        if (iNode.Next == jNode)
        {
            // cases handled:

            var iPrev = GetPreviousNodes(iNode, null).iPrevious;
            var jNext = jNode.Next;

            if (iPrev is not null)
            {
                iPrev.Next = jNode;
            }

            iNode.Next = jNext;
            jNode.Next = iNode;
        }
        else if (jNode.Next == iNode)
        {
            // same as the above if branch
            var jPrev = GetPreviousNodes(null, jNode).jPrevious;
            var iNext = iNode.Next;

            if (jPrev is not null)
            {
                jPrev.Next = iNode;
            }

            jNode.Next = iNext;
            iNode.Next = jNode;
        }
        else
        {
            // cases handled:

            var (iPrev, jPrev) = GetPreviousNodes(iNode, jNode);
            var (iNext, jNext) = (iNode.Next, jNode.Next);

            if (iPrev is not null)
            {
                iPrev.Next = jNode;
            }

            if (jPrev is not null)
            {
                jPrev.Next = iNode;
            }

            iNode.Next = jNext;
            jNode.Next = iNext;
        }

        // head / tail swapping
        if (head == iNode)
        {
            head = jNode;
        }
        else if (head == jNode)
        {
            head = iNode;
        }

        if (tail == iNode)
        {
            tail = jNode;
        }
        else if (tail == jNode)
        {
            tail = iNode;
        }

        return true;
    }

    private (NodeSingly<T>? iPrevious, NodeSingly<T>? jPrevious) GetPreviousNodes(NodeSingly<T>? iNode, NodeSingly<T>? jNode)
    {
        if (head is null)
        {
            return (null, null);
        }

        if (iNode is null && jNode is null)
        {
            return (null, null);
        }

        NodeSingly<T>? iPrev = null, jPrev = null;
        var curr = head;

        do
        {
            if (iNode is not null && curr.Next == iNode)
            {
                iPrev = curr;
            }
            else if (jNode is not null && curr.Next == jNode)
            {
                jPrev = curr;
            }

            curr = curr.Next;
        } while (curr != null && (iPrev is null || jPrev is null));

        return (iPrev, jPrev);
    }

    public void Sort()
    {
        // Using Selection sort for this purpose since it suits the best for non-contigious block of memory.
        if (head is null)
        {
            // no element, nothing to sort.
            return;
        }

        if (head == tail)
        {
            // only one node, already sorted.
            return;
        }

        var comparer = Comparer<T>.Default;
        for (var iNode = head; iNode != tail; iNode = iNode!.Next) // equivalent to for (int i = 0; i < arr.Length  -1; i++)
        {
            var minNode = iNode;    // assume that iNode contains the minimum data

            for (var jNode = iNode!.Next; jNode != null; jNode = jNode.Next)    // equivalent to for (int j = i + 1; j < arr.Length; j++)
            {
                if (comparer.Compare(jNode!.Data, minNode!.Data) < 0)   // finding the node having mininum data
                {
                    minNode = jNode;
                }
            }

            if (minNode == iNode)
            {
                continue;   // means that iNode is already the minimum one
            }

            SwapNodes(minNode!, iNode); // otherwise swap the nodes so that the list is sorted till iNode
            iNode = minNode;    // so that we can continue with our loop from the same position
        }
    }

    public void RemoveDuplicatesSorted()
    {
        if (head is null)
        {
            return;
        }

        if (head == tail)
        {
            return;
        }

        var comparer = EqualityComparer<T>.Default;
        for (var curr = head; curr != null;)
        {
            T? currData = curr!.Data;
            T? nextData = default;
            if (curr!.Next != null)
            {
                nextData = curr.Next.Data;
            }

            if (comparer.Equals(currData, nextData))
            {
                // means that the next one is the duplicate of the curr
                var nextNext = curr.Next!.Next;
                if (curr.Next is not null)
                {
                    curr.Next.Next = null;  // remove the redundaunt reference
                }
                curr.Next = nextNext;

                if (nextNext is null)
                {
                    tail = curr;    // update the tail if it's the last element
                }
            }
            else
            {
                // Advance only when we are sure that we have successfully removed the duplicates of the currData
                curr = curr.Next;
            }
        }
    }

    public void RemoveDuplicatesUnSorted()
    {
        if (head is null)
        {
            return;
        }

        if (head == tail)
        {
            return;
        }

        var comparer = EqualityComparer<T>.Default;
        for (var curr = head; curr != null; curr = curr.Next)
        {
            for (var toCheck = curr.Next; toCheck != null;)
            {
                var toCheckNext = toCheck.Next;
                if (comparer.Equals(curr.Data, toCheck.Data))
                {
                    RemoveNode(toCheck);
                }

                toCheck = toCheckNext;
            }
        }
    }

    private void RemoveNode(NodeSingly<T> node)
    {
        if (head is null)
        {
            return;
        }

        if (head == tail)
        {
            if (node == head)
            {
                head = tail = null;
            }

            return;
        }

        if (node == head)
        {
            var oldHead = head;

            head = head.Next;
            oldHead.Next = null;

            return;
        }

        NodeSingly<T>? prev;
        for (prev = head; prev != null && prev.Next != node; prev = prev.Next)
        {
            // empty
        }

        if (prev is null)
        {
            // impossible case since this could be true only if the node to remove was head but that case has already been handled.
            return;
        }

        if (node == tail)
        {
            prev.Next = null;
            tail = prev;

            return;
        }

        prev.Next = node.Next;
        node.Next = null; 
    }
}
