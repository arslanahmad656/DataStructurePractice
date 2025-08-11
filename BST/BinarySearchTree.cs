using System.Diagnostics.CodeAnalysis;

namespace BST;

/// <summary>
/// Allows a duplicate insertion to the right.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class BinarySearchTree<T>
{
    private Node<T>? root;

    public bool Search(T value) => Search(root, value) is not null;

    public IEnumerable<T> TraverseInOrder() => InOrderIteratively(root);

    public IEnumerable<T> TraverserPreOrder() => PreOrderIteratively(root);

    public IEnumerable<T> TraversePostOrder() => PostOrderIteratively(root);

    public IEnumerable<T> TraverseLevelOrder() => LevelOrderIteratively(root);

    public IEnumerable<List<T>> TraverseLevelOrderByLevel() => LevelOrderByLevelIteratively(root);

    public bool IsValid() => IsBstValidViaInOrderTraversalIterative(root);

    public int GetHeight() => GetNodeHeight(root);

    public T? GetLowestCommonAncestor(T value1, T value2) => FindLowestCommonAncestor(root, value1, value2) is Node<T> a ? a.Value : default;

    public T? GetMinimum(out bool success)
    {
        success = root is not null;

        return root is null ? default : FindMinimum(root).Node.Value;
    }

    public T? GetMaximum(out bool success)
    {
        success = root is not null;

        return root is null ? default : FindMaximum(root).Node.Value;
    }

    public void Insert(T value)
    {
        if (root is null)
        {
            root = new Node<T>(value);
            return;
        }

        var parentNode = GetParentNodeForInsertion(root, value);
        var newNode = new Node<T>(value);
        if (Comparer<T>.Default.Compare(value, parentNode.Value) < 0)
        {
            parentNode.Left = newNode;
        }
        else
        {
            parentNode.Right = newNode;
        }
    }

    public bool Remove(T value)
    {
        var parent = GetParentNode(value, out _, out _);
        if (parent is null)
        {
            // means value does not exist in the tree
            return false;
        }

        Node<T> nodeToRemove = (value < parent ? parent.Left : parent.Right)!; // nodeToRemove can't be null at this point

        if (nodeToRemove.Left is null && nodeToRemove.Right is null)
        {
            // Case 1 (the node is leaf node). Just remove the leaf node.
            if (ReferenceEquals(parent.Left, nodeToRemove))
            {
                parent.Left = null;
            }
            else
            {
                parent.Right = null;
            }

            return true;
        }

        if (nodeToRemove.Left is null || nodeToRemove.Right is null)
        {
            // Case 2. Node has exactly one child.
            // In this case, just skip the node

            if (ReferenceEquals(parent.Left, nodeToRemove))
            {
                parent.Left = nodeToRemove.Left ?? nodeToRemove.Right;
                nodeToRemove.Left = nodeToRemove.Right = null;
            }
            else
            {
                parent.Right = nodeToRemove.Left ?? nodeToRemove.Right;
                nodeToRemove.Right = nodeToRemove.Left = null;
            }

            return true;
        }

        // Case 3. Node has both of its children
        // In this case, we have to replace the node to delete with its in-order successor.
        // In order successor is the smallest value in a node's right subtree or the max value in its left subtree.

        var inOrderSuccessor = FindMinimum(nodeToRemove.Right); // we'll take the min from the right subtree
        nodeToRemove.Value = inOrderSuccessor.Node.Value;
        if (inOrderSuccessor.ParentNode is null)
        {
            // Case 3.1. The min from the right subtree is the right child of the nodeToRemove itself.
            // In this case, do the following:/ (this is the same as nodeToRemove.Right.Value)
            nodeToRemove.Right = inOrderSuccessor.Node.Right; // same as nodeToRemove.Right.Right
        }
        else
        {
            // Case 3.2. The min is not the immdediate child. In that case, it must be a leaf
            inOrderSuccessor.ParentNode.Left = inOrderSuccessor.Node.Right;
        }

        return true;
    }
}
