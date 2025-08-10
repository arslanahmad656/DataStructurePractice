using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Net.Quic;
using System.Net.WebSockets;

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

            for (int i = 0; i <  currentLevelCount; i++)
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
}
