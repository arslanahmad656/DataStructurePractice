using System.Linq.Expressions;
using LinkedLists.Helpers;
using LinkedLists.Nodes;

namespace LinkedLists;

public class CircularListSingly<T>
{
    private NodeSingly<T>? head;
    private NodeSingly<T>? tail;

    public int Count
    {
        get
        {
            if (head is null)
            {
                return 0;
            }

            var count = 1;
            for (var curr = head; curr != tail; curr = curr.Next, count++)
            {
                // empty;
            }

            return count;
        }
    }

    public void Append(T item)
    {
        var node = new NodeSingly<T>
        {
            Data = item,
        };

        if (head == null)
        {
            node.Next = node;
            head = node;
            tail = node;

            return;
        }

        tail!.Next = node;  // never 
        node.Next = head;

        tail = node;
    }

    public void Prepend(T item)
    {
        var node = new NodeSingly<T> { Data = item, };

        if (head is null)
        {
            node.Next = node;
            head = node;
            tail = node;

            return;
        }

        node.Next = head;
        tail!.Next = node;

        head = node;
    }

    public bool Insert(int index, T item)
    {
        var node = new NodeSingly<T> { Data = item };
        if (index == 0)
        {
            if (head is null)
            {
                head = node;
                tail = node;
                node.Next = node;

                return true;
            }

            node.Next = head;
            head = node;
            tail!.Next = head;

            return true;
        }

        var curr = head;
        var i = 1;

        while (curr != tail && i < index)
        {
            curr = curr!.Next;
            i++;
        }

        if (i < index && curr == tail)
        {
            // circled back. no position to insert

            return false;
        }

        node.Next = curr!.Next;
        curr.Next = node;

        if (curr == tail)
        {
            // if inserted after last element
            tail = node;
        }

        return true;
    }

    public bool RemoveHead()
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

        var oldHead = head;
        head = head.Next;
        tail!.Next = head;

        oldHead.Next = null;
        return true;
    }

    public bool RemoveTail()
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

        var nodeBeforeTail = head;
        while (nodeBeforeTail!.Next != tail)
        {
            nodeBeforeTail = nodeBeforeTail.Next;
        }

        tail!.Next = null;
        nodeBeforeTail.Next = head;
        tail = nodeBeforeTail;

        return true;
    }

    public CircularListSingly<T?>[]? SplitInParts(int parts)
    {
        if (head is null)
        {
            return [];
        }

        if (parts <= 0)
        {
            return null;
        }

        var totalNodes = Count;
        var numberOfItemsPerPart = Math.Ceiling((double)Count / parts);

        var curr = head;
        var lists = new CircularListSingly<T?>[parts];
        for (int part = 1, totalNodesEncountered = 0; part <= parts; part++)
        {
            var nodesToRead = part == parts ? (totalNodes - totalNodesEncountered) : numberOfItemsPerPart;

            var list = new CircularListSingly<T?>();

            for (int i = 1; i <= nodesToRead; i++)
            {
                list.Append(curr!.Data);
                curr = curr.Next;
                totalNodesEncountered++;
            }

            lists[part - 1] = list;
        }

        return lists;
    }

    public CircularListSingly<T?> Merge(params CircularListSingly<T>[] lists)
    {
        if (lists.Length == 0)
        {
            return new();
        }

        var mergedList = lists[0].DeepCopy();

        for (int i = 1; i < lists.Length; i++)
        {
            var currentList = lists[i].DeepCopy();

            if (currentList.head is null)
            {
                // empty list. 
                continue;
            }

            if (mergedList.head is null)
            {
                // means this is the first non empty list found to merge
                mergedList = currentList;
                continue;
            }

            mergedList.tail!.Next = currentList.head;
            currentList.tail!.Next = mergedList.head;

            mergedList.tail = currentList.tail;
        }

        return mergedList;
    }

    public CircularListSingly<T?> DeepCopy()
    {
        var copy = new CircularListSingly<T?>();

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

        if (iNode.Next == jNode && jNode.Next == iNode)
        {
            // A --> B --> A (or conceptually: A <--> B)
            // this case is already symmetrical. There is nothing to swap since even after swapping the result stays the same.
            return true;    // to indicate that nothing was there two swap but since this could be a valid scenario hence returning true.
        }

        // Remember: in order to avoid cycles (a node pointing to itself) during swaps, always handle the case of adjacent nodes as special cases
        if (iNode.Next == jNode)
        {
            // adjacent
            // A --> B --> X --> Y --> C --> A
            // i: X, j: Y
            // Handles the following cases:
            // A --> B --> X --> Y --> C --> A
            // X --> Y --> A --> B --> X (Adjacency on the head)
            // A --> B --> X --> Y --> A (Adjacency on the tail)


            var (iPrev, _) = GetPreviousNodes(iNode, null);
            var jNext = jNode.Next;

            // Neighbours links
            iPrev!.Next = jNode;

            // self links
            iNode.Next = jNext;
            jNode.Next = iNode; // not iNext
        }
        else if (jNode.Next == iNode)
        {
            // adjacent
            // A --> B --> X --> Y --> C --> A
            // i: Y, j: X
            // Handles the following cases:
            // same as the above if branch 

            var (jPrev, _) = GetPreviousNodes(jNode, _);
            var iNext = iNode.Next;

            jPrev!.Next = iNode;

            jNode.Next = iNext;
            iNode.Next = jNode; // not jNext
        }
        else
        {
            // case when the nodes to swap are not adjacent. It does not matter which comes before or after.
            // A --> X --> B --> C --> Y --> D --> A
            // Handles the following cases:
            // A --> X --> B --> C --> Y --> D --> A (None of the neighbours at the borders)
            // X --> A --> B --> Y --> C --> X  (One of the neighbours at the head)
            // A --> X --> B --> C --> Y --> A (One of the neighbouts at the tail)

            var (iPrev, jPrev) = GetPreviousNodes(iNode, jNode);    // have to calculate this since the node knows only about its next node and not about the previous one
            var iNext = iNode.Next;
            var jNext = jNode.Next;

            // handle the links of the neighbours
            iPrev!.Next = jNode;
            jPrev!.Next = iNode;

            // handle own links
            iNode.Next = jNext;
            jNode.Next = iNext;
        }

        // check if head / tail needs to be changed

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

    private (NodeSingly<T>? iPrevious, NodeSingly<T>? jPrevious) GetPreviousNodes(NodeSingly<T>? iNode, NodeSingly<T>? jNode)
    {
        if (iNode is null && jNode is null)
        {
            return (null, null);
        }

        if (head is null)
        {
            return (null, null);
        }

        if (head == tail)
        {
            return (null, null);    // invalid case. If only one node, there is not previous
        }

        NodeSingly<T>? iPrev = null, jPrev = null;

        var curr = head;
        do
        {
            if (iNode != null && curr!.Next == iNode) // don't need to do iPrev == null since at max one node can be previous to a node. similary this holds for jNode
            {
                iPrev = curr;
            }
            else if (jNode != null && curr!.Next == jNode)    // a node can be previous to at most one node. hence curr can either be previous to iNode or jNode or neither's but cannot be the previous to the both
            {
                jPrev = curr;
            }

            curr = curr!.Next;
        } while (curr != head && (iPrev is null || jPrev is null));   // Since this is a circular list, none of iPrev and jPrev can be null. (excluding the case when one of the iNode or jNode is null)

        return (iPrev, jPrev);
    }

    private void Rotate(int k)
    {
        if (head == null)
        {
            return;
        }

        if (head == tail)
        {
            return;
        }

        var totalNodes = this.Count;
        var normalizedK = Helper.GetNormalizedKForRotation(k, totalNodes);

        if (normalizedK % totalNodes == 0)
        {
            return;
        }

        // now, 0 < k < totalNodes

        var kthNode = head;
        for (int i = 1; i < normalizedK; i++)
        {
            kthNode = kthNode!.Next;
        }

        // since it's a circular list, the links stay the same. only the head and tail change.
        head = kthNode!.Next;
        tail = kthNode;
    }
}
