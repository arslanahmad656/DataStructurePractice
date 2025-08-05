using IntStack = StacksAndQueues.Stack<int>;

var stack = new IntStack();

for (int i = 0; i < 5; i++)
{
    stack.Push(i);

    var topValue = stack.Peek();

    Console.WriteLine($"Inserted {topValue}. Count: {stack.Count}");
}

Console.WriteLine("Emptying the stack");

while (!stack.IsEmpty)
{
    var topValue = stack.Pop(out _);
    Console.WriteLine($"Popped {topValue}. Count: {stack.Count}");
}

