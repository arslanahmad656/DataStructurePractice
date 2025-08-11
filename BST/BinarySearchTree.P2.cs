using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Net.Quic;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace BST;

public partial class BinarySearchTree<T>
{
    public static BinarySearchTree<int> BuildRandomTree() => new()
    {
        root = new(10)
        {
            Left = new(5)
            {
                Left = new(3),
                Right = new(8)
            },
            Right = new(15)
            {
                Left = new(12),
                Right = new(20)
            }
        }
    };

    private static IEnumerable<T> InOrderIteratively(Node<T>? root)
    {
        var curr = root;
        var st = new Stack<Node<T>>();

        while (curr is not null || st.Count > 0)
        {
            // push all the left children
            while (curr is not null)
            {
                st.Push(curr);
                curr = curr.Left;
            }

            // current is null now.

            // the top element of the stack is the one to visit.
            curr = st.Pop();
            yield return curr.Value;

            // move the curr to curr.Right because if there is no element to the right of the curr, node will be visited next
            curr = curr.Right;
        }
    }

    private static IEnumerable<T> PreOrderIteratively(Node<T>? root)
    {
        if (root is null)
            yield break;

        var st = new Stack<Node<T>>();
        st.Push(root);

        while (st.Count > 0)
        {
            var curr = st.Pop();
            yield return curr.Value;

            if (curr.Right is not null)
            {
                st.Push(curr.Right);
            }

            if (curr.Left is not null)
            {
                st.Push(curr.Left);
            }
        }
    }

    private static IEnumerable<T> PostOrderIteratively(Node<T>? root)
    {
        if (root is null)
            yield break;

        var st1 = new Stack<Node<T>>();
        var st2 = new Stack<Node<T>>();

        st1.Push(root);

        while (st1.Count > 0)
        {
            var curr = st1.Pop();
            st2.Push(curr);

            if (curr.Left is not null)
            {
                st1.Push(curr.Left);
            }

            if (curr.Right is not null)
            {
                st1.Push(curr.Right);
            }
        }

        while (st2.Count > 0)
        {
            yield return st2.Pop().Value;
        }
    }

    private static IEnumerable<T> LevelOrderIteratively(Node<T>? root)
    {
        if (root is null)
        {
            yield break;
        }

        var q = new Queue<Node<T>>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            var curr = q.Dequeue();

            yield return curr.Value;

            if (curr.Left is not null)
            {
                q.Enqueue(curr.Left);
            }

            if (curr.Right is not null)
            {
                q.Enqueue(curr.Right);
            }
        }
    }

    private static IEnumerable<List<T>> LevelOrderByLevelIteratively(Node<T>? root)
    {
        if (root is null)
        {
            yield break;
        }

        var q = new Queue<Node<T>>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            var currentLevelCount = q.Count;
            var itemsInCurrentLevel = new List<T>(currentLevelCount);

            for (int i = 0; i < currentLevelCount; i++)
            {
                var curr = q.Dequeue();
                itemsInCurrentLevel.Add(curr.Value);

                if (curr.Left is not null)
                {
                    q.Enqueue(curr.Left);
                }

                if (curr.Right is not null)
                {
                    q.Enqueue(curr.Right);
                }
            }

            yield return itemsInCurrentLevel;
        }
    }

    private static int GetNodeHeight(Node<T>? root)
    {
        if (root is null)
        {
            return -1;
        }

        var q = new Queue<Node<T>>();
        q.Enqueue(root);

        int level = -1;

        while (q.Count > 0)
        {
            level++;
            var nodesInCurrentLevel = q.Count;

            for (int i = 0; i < nodesInCurrentLevel; i++)
            {
                var curr = q.Dequeue();

                if (curr.Left is not null)
                {
                    q.Enqueue(curr.Left);
                }

                if (curr.Right is not null)
                {
                    q.Enqueue(curr.Right);
                }
            }
        }

        return level;
    }

    private static bool IsBstValidViaInOrderTraversal(Node<T>? root, ref T? prevValue)
    {
        if (root is null)
        {
            // root is null, which means that either the tree is empty and by definition it's a valid BST.
            // or, we reached the empty node hence before that all the tree was a valid BST otherwise false would have been returned already.
            return true;
        }

        // since we're traversing in order, first we check the left subtree recursively.
        var isLeftBst = IsBstValidViaInOrderTraversal(root.Left, ref prevValue);
        if (!isLeftBst)
        {
            return false;
        }

        // now we check the node

        if (prevValue is not null && Comparer<T>.Default.Compare(root.Value, prevValue) < 0)
        {
            // prev null means this is the first node to visit.
            // normally, the current node's value must never be lesser than the previous because we know that in-order traversal always result in the ascending order of node values.
            return false;
        }

        // set the curr value as the prev
        prevValue = root.Value;

        // now check the right subtree.
        var isRightBst = IsBstValidViaInOrderTraversal(root.Right, ref prevValue);

        if (!isRightBst)
        {
            return false;
        }

        // everythign good?
        return true;
    }

    private static bool IsBstValidViaInOrderTraversalIterative(Node<T>? root)
    {
        if (root is null)
        {
            return true;
        }

        var isFirstIteration = true;
        T? prevValue = default;

        foreach (var currentValue in InOrderIteratively(root))
        {
            if (isFirstIteration)
            {
                isFirstIteration = false;
            }
            else if (Comparer<T>.Default.Compare(currentValue, prevValue) < 0)
            {
                return false;
            }

            prevValue = currentValue;
        }

        return true;
    }

    private static bool IsBstValidWithBounds(
    Node<T>? node,
    T minValue, bool hasMin,
    T maxValue, bool hasMax)
    {
        if (node is null)
            return true;

        // Check against the lower bound
        if (hasMin && Comparer<T>.Default.Compare(node.Value, minValue) <= 0)
            return false;

        // Check against the upper bound
        if (hasMax && Comparer<T>.Default.Compare(node.Value, maxValue) >= 0)
            return false;

        // Left subtree: update the upper bound
        if (!IsBstValidWithBounds(node.Left, minValue, hasMin, node.Value, true))
            return false;

        // Right subtree: update the lower bound
        if (!IsBstValidWithBounds(node.Right, node.Value, true, maxValue, hasMax))
            return false;

        return true;
    }

    // Public entry point
    private static bool IsBstValid(Node<T>? root)
    {
        return IsBstValidWithBounds(
            root,
            default!, false,   // No initial min bound
            default!, false    // No initial max bound
        );
    }

    private static Node<T>? FindLowestCommonAncestorRecursively(Node<T>? root, in T value1, in T value2)
    {
        if (root is null)
        {
            return null;   // No LCA 
        }

        var comparer = Comparer<T>.Default;
        if (comparer.Compare(value1, root.Value) < 0 && comparer.Compare(value2, root.Value) < 0)
        {
            // the LCA must be in the left subtree
            return FindLowestCommonAncestorRecursively(root.Left, value1, value2);
        }

        if (comparer.Compare(value1, root.Value) >= 0 && comparer.Compare(value2, value2) >= 0)
        {
            // LCA must be in the right subtree
            return FindLowestCommonAncestorRecursively(root.Right, value1, value2);
        }

        return root;
    }

    private static Node<T>? FindLowestCommonAncestor(Node<T>? root, T value1, T value2)
    {
        var curr = root;

        var comparer = Comparer<T>.Default;

        while (curr is not null)
        {
            if (comparer.Compare(value1, curr.Value) < 0 && comparer.Compare(value2, curr.Value) < 0)
            {
                curr = curr.Left;
            }
            else if (comparer.Compare(value1, curr.Value) >= 0 && comparer.Compare(value2, curr.Value) >= 0)
            {
                curr = curr.Right;
            }
            else
            {
                return curr;
            }
        }

        return null;
    }
}
