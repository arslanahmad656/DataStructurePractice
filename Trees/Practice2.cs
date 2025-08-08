namespace Trees;

public static class Practice2
{
    public static void TraverseInOrder<T>(BinaryTree<T>.Node<T> root, Action<T?> action)
    {
        var curr = root;
        var st = new Stack<BinaryTree<T>.Node<T>>();

        while (curr is not null || st.Count > 0)
        {
            // go to the left most element
            while (curr is not null)
            {
                st.Push(curr);
                curr = curr.Left;
            }

            // curr is null at this point.
            // the top of the stack is the left most element to traverse
            curr = st.Pop();

            // visit the current node after the left is done
            action(curr.Value);

            // now we need to visit the node of the curr. this will be done in the next iteration because if curr.Right is null, the next element in the stack is the parent of the curr
            // set the curr to the right so that the right subtree may be visited
            curr = curr.Right;
        }
    }
}
