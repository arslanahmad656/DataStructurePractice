using System.ComponentModel.Design.Serialization;

namespace Trees;

public class BinaryTree<T>
{
    public Node<T>? root;

    public BinaryTree()
    {
    }

    public BinaryTree(params T[] items)
    {
        if (items is null || items.Length == 0)
        {
            return;
        }

        root = new(items[0]);
        var q = new Queue<Node<T>>();
        q.Enqueue(root);
        
        var i = 1;
        while (i < items.Length && q.Count > 0)
        {
            var current = q.Dequeue();

            if (i < items.Length)
            {
                var left = new Node<T>(items[i]);
                current.Left = left;
                q.Enqueue(left);
            }
            i++;

            if (i < items.Length)
            {
                var right = new Node<T>(items[i]);
                current.Right = right;
                q.Enqueue(right);
            }
            i++;
        }
    }

    public Node<T>? GetNode(T val) => GetNode(root, val);

    private static Node<T>? GetNode(Node<T>? root, in T targetValue)
    {
        if (root is null)
        {
            return null;
        }

        if (root == targetValue)
        {
            return root;
        }

        var nodeFoundFromLeft = BinaryTree<T>.GetNode(root.Left, in targetValue);
        if (nodeFoundFromLeft is not null)
        {
            return nodeFoundFromLeft;
        }

        var nodeFoundFromRight = BinaryTree<T>.GetNode(root.Right, in targetValue);
        if (nodeFoundFromRight is not null)
        {
            return nodeFoundFromRight;
        }

        return null;
    }

    public int GetHeight() => GetNodeHeight(root);

    public int GetCount() => GetSubtreeNodesCount(root);

    private static int GetNodeHeight(Node<T>? node)
    {
        if (node is null)
        {
            return -1;
        }

        var nodeHeightFromLeftSubtree = 1 + GetNodeHeight(node.Left);
        var nodeHeightFromRightSubtree = 1 + GetNodeHeight(node.Right);

        var nodeHeight = Math.Max(nodeHeightFromLeftSubtree, nodeHeightFromRightSubtree);
        return nodeHeight;
    }

    public int GetNodeDepth(T nodeValue) => BinaryTree<T>.GetNodeDepth(root, nodeValue);

    private static int GetNodeDepth(Node<T>? root, in T val)
    {
        // Logic: Try to check that the root is the val. If so, return 0.
        // If not, search the depth of val in the left subtree. If found, we know that the depth will be one level more than the depth from the left subtree.
        // If not found in the left, do the same in the right subtree.
        // otherwise, the element val does not exist in the tree. Hence, return -1.

        if (root is null)
        {
            // root does not exist, means tree is empty hence -1
            return -1;
        }

        if (root == val)
        {
            // by definition, depth of the root is 0. Since the root is the same as val, we needed to find the depth of this node.
            return 0;
        }

        var depthInLeftSubtree = BinaryTree<T>.GetNodeDepth(root.Left, val);
        if (depthInLeftSubtree != -1)
        {
            return 1 + depthInLeftSubtree;
        }

        var depthInRightSubtree = BinaryTree<T>.GetNodeDepth(root.Right, val);
        if (depthInRightSubtree != -1)
        {
            return 1 + depthInRightSubtree;
        }

        return -1;
    }

    public bool IsFull()
    {
        // All nodes must have the degree exactly 2 or 0
        return IsFull(root);
    }

    private static bool IsFull(Node<T>? currentNode)
    {
        if (currentNode is null)
        {
            return true;
        }

        var currentNodeFull = currentNode.Degree is 0 or 2;
        if (!currentNodeFull)
        {
            return false;
        }

        var leftNodeFull = IsFull(currentNode.Left);
        if (!leftNodeFull)
        {
            return false;
        }

        var rightNodeFull = IsFull(currentNode.Right);
        if (!rightNodeFull)
        {
            return false;
        }

        return true;
    }

    private static int GetSubtreeNodesCount(Node<T>? node)
    {
        // Logic used:
        // Number of nodes below a node (n1) = number of nodes in left subtree + number of nodes in right subtree
        // total number of nodes in a tree starting from a node: n1 + 1

        if (node is null)
        {
            return 0;
        }

        var numberOfNodesInLeftSubtree = GetSubtreeNodesCount(node.Left);
        var numberOfNodesInRightSubtree = GetSubtreeNodesCount(node.Right);

        var totalNumberOfNodesIncludingTheNodeItself = 1 + numberOfNodesInLeftSubtree + numberOfNodesInRightSubtree;

        return totalNumberOfNodesIncludingTheNodeItself;
    }

    public bool IsPerfect()
    {
        // Logic used:
        // Given a tree with height h, maximum number n of nodes which can be occupied by the tree = 2 ^ (h + 1) - 1 where h >= 0. For root, h = 0
        // It can be derived from the definition of a perfect tree that in a perfect tree, there is no empty spot. It means that if n < 2 ^ (h + 1) - 1, there are some gaps in the tree and hence it is not perfect.
        // the case n > 2 ^ (h + 1) - 1 is not possible for a binary tree. This means that there are more number of nodes than how many can fit.

        var height = GetHeight();
        var nodesCount = GetCount();

        var expectedNumberOfNodes = Math.Pow(2, height + 1) - 1;

        return nodesCount == expectedNumberOfNodes;
    }

    public bool IsComplete()
    {
        // logic:
        // We start the level order traversal. we check if there is a non-null node after a null node. it means that in a level, there is a gap hence not a complete tree.

        if (root is null)
        {
            return true;
        }

        var q = new Queue<Node<T>?>();
        q.Enqueue(root);

        var foundNull = false;
        while (q.Count > 0)
        {
            var currNode = q.Dequeue();

            if (currNode is null)
            {
                foundNull = true;
            }
            else
            {
                if (foundNull == true)
                {
                    return false;   // violation of the definition of a complete tree. can't have a non-null node after a null node since we are traversing in the level order.
                }

                q.Enqueue(currNode.Left);
                q.Enqueue(currNode.Right);
            }
        }

        return true;
    }

    public class Node<TVal>(TVal? value)
    {
        private readonly int hash = Guid.NewGuid().GetHashCode();

        public Node()
            : this(default)
        {

        }

        public TVal? Value { get; set; } = value;

        public Node<TVal>? Left { get; set; }

        public Node<TVal>? Right { get; set; }

        public int Degree
        {
            get
            {
                if (Left is null && Right is null) return 0;

                if (Left is null) return 1;

                if (Right is null) return 1;

                return 2;
            }
        }

        public override int GetHashCode() => hash;

        public static bool operator ==(Node<TVal> node1, Node<TVal> node2)
        {
            if (node1 is null || node2 is null) return false;

            if (ReferenceEquals(node1, node2)) return true;

            return EqualityComparer<TVal>.Default.Equals(node1.Value, node2.Value);
        }

        public static bool operator ==(Node<TVal> node, TVal value)
        {
            if (node is null)
            {
                return false;
            }

            return EqualityComparer<TVal>.Default.Equals(node.Value, value);
        }

        public static bool operator !=(Node<TVal> node, TVal value) => !(node == value);

        public static bool operator !=(Node<TVal> node1, Node<TVal> node2) => !(node1 == node2);

        public override bool Equals(object? obj) => obj is Node<TVal> node && this == node;

        public override string ToString() => $"Value: {Value}. Degree: {Degree}. Left: {(Left is null ? default : Left.Value)}, Right: {(Right is null ? default : Right.Value)}";
    }
}