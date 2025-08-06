using System.Data;

namespace StacksAndQueues;

public static class Practice2
{
    public static IEnumerable<char> FirstNonRepeatingCharacterInStream(Stream stream, char emptyChar = '_')
    {
        // Queue to maintain characters in order of arrival
        var q = new Queue<char>();

        // Dictionary to store frequency of each character
        var frequencyMap = new Dictionary<char, int>();

        do
        {
            // Read the next byte from the stream
            int nextByte = stream.ReadByte();

            // If end of stream is reached, stop iteration
            if (nextByte == -1)
            {
                yield break;
            }

            // Convert byte to char
            var ch = (char)nextByte;

            // Enqueue the character into the queue
            q.Enqueue(ch);

            // Update its frequency in the map
            frequencyMap.TryGetValue(ch, out var frequency);
            frequencyMap[ch] = frequency + 1;

            // Remove characters from the front of the queue
            // until the front is a non-repeating character
            while (!q.IsEmpty && frequencyMap[q.Peek(out _)] > 1)
            {
                q.Dequeue(out _);
            }

            // If queue is empty, no non-repeating character exists
            if (q.IsEmpty)
            {
                yield return emptyChar;
            }
            else
            {
                // The front of the queue is the first non-repeating character
                yield return q.Peek(out _);
            }

        } while (true); // Keep reading until stream ends
    }

    public static bool IsPalindrome(string str)
    {
        if(str == "")
        {
            return true;
        }

        var q = new Queue<char>();
        var s = new Stack<char>();

        foreach (var ch in str)
        {
            q.Enqueue(ch);
            s.Push(ch);
        }

        while (!q.IsEmpty)
        {
            var ch1 = q.Dequeue(out _);
            var ch2 = s.Pop(out _);

            if (ch1 != ch2)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Generates the first N binary numbers as strings using a queue-based BFS-style approach.
    /// </summary>
    /// <param name="n">The number of binary numbers to generate.</param>
    /// <returns>An array of the first N binary numbers in string format.</returns>
    public static string[] GenerateFirstNBinaryNumbers(int n)
    {
        // Handle edge case: if n is less than or equal to 0, return an empty array
        if (n <= 0)
        {
            return [];
        }

        // Special case: if n is 1, return an array with only "1"
        if (n == 1)
        {
            return ["1"];
        }

        // Array to hold the result binary strings
        var bins = new string[n];

        // Initialize a queue and enqueue the first binary number "1"
        var q = new Queue<string>();
        q.Enqueue("1");

        // Loop n times to generate the first n binary numbers
        for (int i = 0; i < n; i++)
        {
            // Dequeue the front element — this is the current binary number
            var ithNum = q.Dequeue(out _);
            bins[i] = ithNum!; // Store it in the result array

            // If this is the last required number, stop early (optional optimization)
            if (i == n - 1)
            {
                break;
            }

            // Enqueue the next two binary numbers by appending '0' and '1' to the current one
            q.Enqueue($"{ithNum}0");
            q.Enqueue($"{ithNum}1");
        }

        return bins;
    }


    /// <summary>
    /// Interleaves the first half of the queue with the second half using the first-approach strategy.
    /// For odd-sized queues, the second half will contain one more element.
    /// The original queue is modified in-place to contain the interleaved result.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue">The queue to interleave.</param>
    public static void InterleaveHalfHalf<T>(Queue<T> queue)
    {
        // If the queue is empty, there's nothing to interleave
        if (queue.IsEmpty)
        {
            return;
        }

        // Total number of elements in the queue
        var length = queue.Count;

        // First half will contain floor(length / 2) elements
        var half = length / 2;

        // Temporary queues to hold the two halves
        var q1 = new Queue<T>(); // First half
        var q2 = new Queue<T>(); // Second half

        // Dequeue the first half elements into q1
        for (int i = 1; i <= half; i++)
        {
            q1.Enqueue(queue.Dequeue(out _)!);
        }

        // Remaining elements go into q2
        while (!queue.IsEmpty)
        {
            q2.Enqueue(queue.Dequeue(out _)!);
        }

        // At this point:
        // - q1 has floor(n/2) elements
        // - q2 has ceil(n/2) elements
        // - original queue is empty

        // Reconstruct the original queue by interleaving elements from q1 and q2
        for (int i = 1; i <= length; i++)
        {
            // For odd positions, dequeue from q1 if it's not empty
            if (!q1.IsEmpty && i % 2 == 1)
            {
                queue.Enqueue(q1.Dequeue(out _)!);
            }
            else
            {
                // Otherwise, dequeue from q2
                queue.Enqueue(q2.Dequeue(out _)!);
            }
        }

        // Now the original queue contains the interleaved result
    }

    /// <summary>
    /// Reverses the first k elements of the queue in-place using a stack and an auxiliary queue.
    /// If k is greater than the number of elements, the entire queue is reversed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="k">The number of elements from the front to reverse.</param>
    /// <param name="q">The queue to operate on.</param>
    public static void ReverseFirstK<T>(int k, Queue<T> q)
    {
        // If k is less than or equal to 1, no reversal is needed
        if (k <= 1)
        {
            return;
        }

        var stack = new Stack<T>();   // Used to reverse the first k elements
        var queue = new Queue<T>();   // Temporary queue to hold remaining elements

        // Step 1: Dequeue the first k elements (or all if fewer) and push them onto the stack
        for (int i = 1; !q.IsEmpty && i <= k; i++)
        {
            stack.Push(q.Dequeue(out _)!);
        }

        // Step 2: Dequeue the remaining elements and enqueue them into a temporary queue
        while (!q.IsEmpty)
        {
            queue.Enqueue(q.Dequeue(out _)!);
        }

        // Step 3: Pop the reversed first k elements from the stack and enqueue them back into the original queue
        while (!stack.IsEmpty)
        {
            q.Enqueue(stack.Pop(out _)!);
        }

        // Step 4: Enqueue the untouched remaining elements (from temp queue) back into the original queue
        while (!queue.IsEmpty)
        {
            q.Enqueue(queue.Dequeue(out _)!);
        }

        // The original queue now has the first k elements reversed, with the rest in original order
    }

    public static void RotateByK<T>(int k, Queue<T> q)
    {
        if (k == 0 || q.IsEmpty)
        {
            return;
        }

        var length = q.Count;
        var normalizeK = ((k % length) + length) % length;  // handles both clockwise and counter-clockwise rots

        for (int i = 1; i <= normalizeK; i++)
        {
            q.Enqueue(q.Dequeue(out _)!);
        }
    }

}
