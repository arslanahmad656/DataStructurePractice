namespace Trees;

public static class Practice1
{
    public static void TraverseTreePreOrder<T>(BinaryTree<T>.Node<T>? root, Action<T?> action)
    {
        if (root == null)
        {
            return;
        }

        action(root.Value);

        TraverseTreePreOrder(root.Left, action);
        TraverseTreePreOrder(root.Right, action);
    }

    public static void TraverseTreeInOrder<T>(BinaryTree<T>.Node<T>? root, Action<T?> action)
    {
        if (root is null)
        {
            return;
        }

        TraverseTreeInOrder(root.Left, action);
        action(root.Value);
        TraverseTreeInOrder(root.Right, action);
    }

    public static void TraverseTreePostOrder<T>(BinaryTree<T>.Node<T>? root, Action<T?> action)
    {
        if (root is null)
        {
            return;
        }

        TraverseTreePostOrder(root.Left, action);
        TraverseTreePostOrder(root.Right, action);
        action(root.Value);
    }

    public static void TraverseTreeLevelOrder<T>(BinaryTree<T>.Node<T>? root, Action<T?> action)
    {
        if (root is null)
        {
            return;
        }

        var q = new Queue<BinaryTree<T>.Node<T>>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            var node = q.Dequeue();
            action(node.Value);

            if (node.Left != null)
            {
                q.Enqueue(node.Left);
            }

            if (node.Right != null)
            {
                q.Enqueue(node.Right);
            }
        }
    }

    public static void ActRecursively<T>(T[] arr, Action<T> action)
    {
        Worker(0);
        void Worker(int i)
        {
            if (i == arr.Length)
            {
                return;
            }

            action(arr[i]);
            Worker(i + 1);
        }
    }
}
