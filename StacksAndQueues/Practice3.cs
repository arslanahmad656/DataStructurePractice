namespace StacksAndQueues;

public static class Practice3
{
    public static T[] GetMaxInEachWindowBruteForce<T>(T[] arr, int k)
    {
        if (arr == null || arr.Length == 0 || k <= 0 || k > arr.Length)
        {
            return [];
        }

        int windowCount = arr.Length - k + 1;
        var maxes = new T[windowCount];
        var comparer = Comparer<T>.Default;

        for (int i = 0; i <= arr.Length - k; i++)
        {
            var max = arr[i];
            for (int j = i + 1; j < i + k; j++)
            {
                if (comparer.Compare(arr[j], max) > 0)
                {
                    max = arr[j];
                }
            }

            maxes[i] = max;
        }

        return maxes;
    }

    public static int[] GetMaxInEachWindow(int[] nums, int k)
    {
        if (nums == null || nums.Length == 0 || k <= 0 || k > nums.Length)
            return [];

        var result = new List<int>();
        var deque = new LinkedList<int>(); // stores *indices*, not values

        for (int i = 0; i < nums.Length; i++)
        {
            // 1. Remove indices outside the current window
            if (deque.Count > 0 && deque.First!.Value <= i - k)
                deque.RemoveFirst();

            // 2. Remove indices whose values are less than current element
            while (deque.Count > 0 && nums[deque.Last!.Value] <= nums[i])
                deque.RemoveLast();

            // 3. Add current index
            deque.AddLast(i);

            // 4. Record max when first full window is ready
            if (i >= k - 1)
                result.Add(nums[deque.First!.Value]); // front of deque is max in window
        }

        return result.ToArray();
    }

    public static int FindCelebrity(int[][] M, int n)
    {
        var stack = new System.Collections.Generic.Stack<int>();
        for (int i = 0; i < n; i++)
            stack.Push(i);

        while (stack.Count > 1)
        {
            int A = stack.Pop();
            int B = stack.Pop();

            if (M[A][B] == 1)
                stack.Push(B); // A can't be celebrity
            else
                stack.Push(A); // B can't be celebrity
        }

        int candidate = stack.Pop();

        // Verify candidate
        for (int i = 0; i < n; i++)
        {
            if (i != candidate)
            {
                if (M[candidate][i] == 1 || M[i][candidate] == 0)
                    return -1;
            }
        }

        return candidate;
    }

    public class LRUCache
    {
        private class Node
        {
            public int Key;
            public int Value;
            public Node Prev, Next;
            public Node(int key, int value)
            {
                Key = key;
                Value = value;
            }
        }

        private readonly int _capacity;
        private readonly Dictionary<int, Node> _cache;
        private readonly Node _head, _tail;

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _cache = new Dictionary<int, Node>();

            // Dummy head and tail
            _head = new Node(0, 0);
            _tail = new Node(0, 0);
            _head.Next = _tail;
            _tail.Prev = _head;
        }

        public int Get(int key)
        {
            if (!_cache.ContainsKey(key)) return -1;

            Node node = _cache[key];
            Remove(node);
            InsertToFront(node);
            return node.Value;
        }

        public void Put(int key, int value)
        {
            if (_cache.ContainsKey(key))
            {
                Remove(_cache[key]);
            }

            Node newNode = new Node(key, value);
            InsertToFront(newNode);
            _cache[key] = newNode;

            if (_cache.Count > _capacity)
            {
                // Remove LRU
                Node lru = _tail.Prev;
                Remove(lru);
                _cache.Remove(lru.Key);
            }
        }

        private void Remove(Node node)
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
        }

        private void InsertToFront(Node node)
        {
            node.Next = _head.Next;
            node.Prev = _head;
            _head.Next.Prev = node;
            _head.Next = node;
        }
    }


}
