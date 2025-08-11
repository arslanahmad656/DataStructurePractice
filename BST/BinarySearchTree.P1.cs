namespace BST;

public partial class BinarySearchTree<T>
{
    private static Node<T>? Search(Node<T>? root, T targetValue)
    {
        var curr = root;
        while (curr is not null)
        {
            if (curr == targetValue)
                return curr;

            curr = targetValue < curr ? curr.Left : curr.Right;
        }

        return null;
    }

    private static Node<T> GetParentNodeForInsertion(Node<T> root,  in T targetValue)
    {
        if (Comparer<T>.Default.Compare(targetValue, root.Value) < 0)
        {
            if (root.Left is null)
            {
                return root;
            }

            var parentFromLeft = GetParentNodeForInsertion(root.Left, targetValue);
            return parentFromLeft;
        }
        else
        {
            if (root.Right is null)
            {
                return root;
            }

            var parentFromRight = GetParentNodeForInsertion(root.Right, targetValue);
            return parentFromRight;
        }
    }

    private static (Node<T> Node, Node<T>? ParentNode) FindMinimum(Node<T> root)
    {
        Node<T>? prev = null;
        var curr = root;
        while (curr.Left is not null)
        {
            prev = curr;
            curr = curr.Left;
        }

        return (curr, prev);
    }

    private static (Node<T> Node, Node<T>? ParentNode) FindMaximum(Node<T> root)
    {
        Node<T>? prev = null;
        var curr = root;
        while (curr.Right is not null)
        {
            prev = curr;
            curr = curr.Right;
        }

        return (curr, prev);
    }

    private Node<T>? GetParentNode(T value, out bool success, out bool isRoot)
    {
        if (root is null)
        {
            success = false;
            isRoot = false;

            return null;
        }

        if (root == value)
        {
            isRoot = true;
            success = true;

            return root;
        }

        var parent = GetParentNode(root, value);
        success = parent is not null;
        isRoot = false;
        
        return parent;
    }

    private static Node<T>? GetParentNode(Node<T>? root, in T value)
    {
        if (root is null)
        {
            return null;
        }

        if ((root.Left is not null && root.Left == value) || (root.Right is not null && root.Right == value))
        {
            return root;
        }

        if (value < root)
        {
            return GetParentNode(root.Left, value);
        }
        else
        {
            return GetParentNode(root.Right, value);
        }
    }

    private static void RecurseInOrder(Node<T>? root, in Action<T?> action)
    {
        if (root == null)
        {
            return;
        }

        RecurseInOrder(root.Left, action);

        action(root.Value);

        RecurseInOrder(root.Right, action);
    }

    private static int GetNodeHeightRecursively(Node<T>? root)
    {
        if (root is null)
        {
            return -1;
        }

        var heightOfLeftSubtree = GetNodeHeightRecursively(root.Left);
        var heightOfRightSubtree = GetNodeHeightRecursively(root.Right);

        var nodeHeight = 1 + Math.Max(heightOfLeftSubtree, heightOfRightSubtree);

        return nodeHeight;
    }
}
