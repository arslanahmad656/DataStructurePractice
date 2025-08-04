using LinkedLists.Nodes;
using System.Globalization;

namespace LinkedLists;

public class CircularListDoubly<T>
{
    private NodeDoubly<T>? head;
    private NodeDoubly<T>? tail;

    public int Count
    {
        get
        {
            if (head == null) return 0;

            int count = 1;
            for (var curr = head; curr != tail; curr = curr!.Next, count++)
            {
                // empty
            }

            return count;
        }
    }

    public void Append(T item)
    {
        var node = new NodeDoubly<T> { Data =  item };
        if (head is null)
        {
            head = node;
            tail = node;
            node.Next = node;
            node.Previous = node;
        }
        else
        {
            tail!.Next = node;
            head!.Previous = node;
            node.Next = head;
            node.Previous = tail;

            tail = node;
        }
    }


    public void Prepend(T item)
    {
        var node = new NodeDoubly<T> { Data = item };
        if (head == null)
        {
            head = node;
            tail = node;
            node.Next = node;
            node.Previous = node;
        }
        else
        {
            node.Next = head;
            head.Previous = node;

            tail!.Next = node;
            node.Previous = tail;

            head = node;
        }
    }

    public bool Insert(int index, T item)
    {
        if (index < 0)
        {
            return false;
        }

        if (head is null && index != 0)
        {
            return false;
        }

        var node = new NodeDoubly<T> { Data = item };
        if (index == 0)
        {
            node.Next = head;
            node.Previous = tail;

            head!.Previous = node;
            tail!.Next = node;

            head = node;
        }
        else
        {
            var curr = head;
            int i = 0;
            while (i < index && curr != tail)
            {
                curr = curr!.Next;
                i++;
            }

            if (curr == head && index > i + 1)
            {
                // at max, we can insert after tail.
                return false;
            }

            node.Next = curr!.Next;
            node.Previous = curr;

            curr!.Next!.Previous = node;
            curr!.Next = node;

            if (curr == tail)
            {
                tail = node;
                head!.Previous = tail;
            }
        }

        return true;
    }

    public bool RemoveAtHead()
    {
        if (head is null)
        {
            return false;
        }

        if (head == tail)
        {
            head = null;
            tail = null;

            return true;
        }

        var oldHead = head;

        tail!.Next = head.Next;
        head.Next!.Previous = tail;

        head = head.Next;

        oldHead.Next = null;
        oldHead.Previous = null;

        return true;
    }

    public bool RemoveAtTail()
    {
        if (head is null)
        {
            return false;
        }

        if (head == tail)
        {
            head = tail = null;
            return true;
        }

        var oldTail = tail;

        tail!.Previous!.Next = head;
        head.Previous = tail.Previous;

        oldTail!.Next = null;
        oldTail.Previous = null;

        return true;
    }

    public bool RemoveAt(int index)
    {
        if (head is null)
        {
            return false;
        }

        if (index < 0)
        {
            return false;
        }

        if (head == tail)
        {
            if (index != 0)
            {
                // invalid scenario
                return false;
            }

            head = tail = null;
            return true;
        }

        int nodeIndex = 0;
        var curr = head;
        while (nodeIndex < index && curr != tail)
        {
            nodeIndex++;
            curr = curr!.Next;
        }

        if (nodeIndex < index)
        {
            // means not enough number of nodes.

            return false;
        }

        curr!.Previous!.Next = curr!.Next;
        curr!.Next!.Previous = curr!.Previous;

        if (curr == head)
        {
            head = curr.Next;
        }
        else if (curr == tail)
        {
            tail = curr.Previous;
        }

        curr.Next = null;
        curr.Previous = null;

        return true;
    }

    public CircularListDoubly<T?>[] SplitInParts(int parts)
    {
        if (parts <= 0)
        {
            return [];
        }

        if (head is null)
        {
            return [];
        }

        var totalNodes = this.Count;
        parts = Math.Min(parts, totalNodes);
        var nodesPerPart = Math.Ceiling((double)totalNodes / parts);

        var curr = head;
        var lists = new CircularListDoubly<T?>[parts];
        for (int i = 0, totalNodesProcessed = 0; i < parts; i++)
        {
            var list = new CircularListDoubly<T?>();
            var nodesToAdd = i == parts - 1 ? totalNodes - totalNodesProcessed : nodesPerPart;
            
            for (int j = 0; j < nodesToAdd; j++)
            {
                list.Append(curr!.Data);
                curr = curr.Next;
                totalNodesProcessed++;
            }

            lists[i] = list;
        }

        return lists;
    }

    public static CircularListDoubly<T?> Merge(params CircularListDoubly<T?>[] lists)
    {
        if (lists.Length == 0)
        {
            return new();
        }

        var mergedList = lists[0].DeepCopy();
        CircularListDoubly<T?>? currentList = null;
        for (int i = 1; i < lists.Length; i++)
        {
            currentList = lists[i].DeepCopy();

            mergedList.tail!.Next = currentList.head;
            currentList.head!.Previous = mergedList.tail;
        }

        if (currentList is not null)
        {
            // means that there was at least one other list to be merged. Hence we need to adjust the heads and tails of the final list.
            mergedList.head!.Previous = currentList.tail;
            currentList.tail!.Next = mergedList.head;

            mergedList.tail = currentList.tail;
        }

        return mergedList;
    }

    public CircularListDoubly<T?> DeepCopy()
    {
        var copy = new CircularListDoubly<T?>();

        if (head is null)
        {
            return copy;
        }

        var curr = head;

        do
        {
            copy.Append(curr!.Data);
            curr = curr.Next;
        } while (curr != head);

        return copy;
    }

    public bool SwapNodes(int i, int j)
    {
        if (head is null)
        {
            return false;
        }

        if (i < 0 || j < 0)
        {
            return false;
        }

        if (i == j)
        {
            return false;
        }

        if (head == tail)
        {
            return false;
        }

        NodeDoubly<T>? iNode = null;
        NodeDoubly<T>? jNode = null;

        int index = 0;
        var curr = head;
        do
        {
            if (index == i)
            {
                iNode = curr;
            }

            if (index == j)
            {
                jNode = curr;
            }

            index++;
            curr = curr!.Next;
        } while ((iNode == null || jNode == null) && curr != head);

        if (iNode is null || jNode is null)
        {
            // at least one of the index is beyond the total number of nodes.

            return false;
        }

        // now we know that the swap is possible.
    }

    private bool SwapNodes(NodeDoubly<T> iNode, NodeDoubly<T> jNode)
    {
        if (iNode == jNode)
        {
            return false;
        }

        if (iNode.Next == jNode)
        {
            // iNode and jNode are adjacent with j following i

            // Example list: A <-> I <-> J <-> B

            var iPrev = iNode.Previous;
            var jNext = jNode.Next;

            // adjust the neighbours (A and B)
            iPrev!.Next = jNode;
            jNext!.Previous = iNode;

            // adjust own pointers
            iNode.Previous = jNode;
            jNode.Next = iNode;
            iNode.Next = jNext;
            jNode.Previous = iPrev;
        }
        else if (jNode.Next == iNode)
        {
            // adjacent with j following i

            // Example list: A <-> J <-> I <-> B

            var jPrev = jNode.Previous;
            var iNext = iNode.Next;

            // neighbours
            jPrev!.Next = iNode;
            iNext!.Previous = jNode;

            // self links
            jNode.Previous = iNode;
            iNode.Next = jNode;
            jNode.Next = iNext;
            iNode.Previous = jPrev;
        }
        else
        {
            // i and j are not adjacent hence no circular error could be introduced hence this case is the general case

            var iPrev = iNode.Previous;
            var iNext = iNode.Next;
            var jPrev = jNode.Previous;
            var jNext = jNode.Next;

            // neighbours
            iPrev!.Next = jNode;
            iNext!.Previous = jNode;
            jPrev!.Next = iNode;
            jNext!.Previous = iNode;

            // self links
            iNode.Previous = jPrev;
            iNode.Next = jNext;
            jNode.Previous = iPrev;
            jNode.Next = iNext;
        }

        // Now check if either or both of the nodes were head or tail
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
}
