using StacksAndQueues;
using System.Runtime.CompilerServices;
using System.Text;
using Trees;
using IntStack = StacksAndQueues.Stack<int>;
using Practice1 = StacksAndQueues.Practice1;
using TreePractice1 = Trees.Practice1;

namespace App1;

internal class Archive1
{


void Test()
    {
        //TestCreation();
//TestBalancedExpression();
//TestReverseString();
//PostfixToInfix();
//PrefixToInfix();
//EvaluatePostfixExpression();
//EvaluatePrefixExpressions();

//ArrayQueue();
//UnboundQueue();
//FirstNonRepeatingChars();
//FirstNBins();
//InterleaveElementsQueue();
//ReverseKInQueue();
//MaxMinQueue();
//MaxInKBruteForce();

//Recurse();

//Tree1();
//NodeHeight();
//TreeOps();
IsFull();
    }

void IsFull()
{
    var tree1 = new BinaryTree<int>(1, 2, 3, 4, 5);
    var tree1Full = tree1.IsFull();
    Console.WriteLine($"Tree 1 is full: {tree1Full}");

    var tree2 = new BinaryTree<int>(1, 2, 3, 4);
    var tree2Full = tree2.IsFull();
    Console.WriteLine($"Tree 2 is full: {tree2Full}");

    var tree3 = new BinaryTree<int>(Enumerable.Range(50, 100).ToArray());

    new[] { tree1, tree2, tree3 }.ToList().ForEach(t => Console.WriteLine($"Total nodes: {t.GetCount()}"));

    var tree4 = new BinaryTree<int>(Enumerable.Range(10, 7).ToArray());
    var tree5 = new BinaryTree<int>(Enumerable.Range(10, 15).ToArray());

    Console.WriteLine();
    Console.WriteLine();

    new[] { tree1, tree2, tree3, tree4, tree5 }.ToList().ForEach(t => Console.WriteLine($"Total nodes: {t.GetCount()}. Is Perfect: {t.IsPerfect()}"));
}

void TreeOps()
{
    var tree = new BinaryTree<int>([.. Enumerable.Range(1, 50)]);

    new[] { 0, 1, 3, 6, 12, 21, 27, 45, 55 }.ToList().ForEach(x => Console.WriteLine($"Depth of {x}: {tree.GetNodeDepth(x)}"));

    Console.WriteLine();
    Console.WriteLine();

    new[] { 0, 1, 21, 45, 56 }.ToList().ForEach(x =>
    {
        var node = tree.GetNode(x);
        Console.WriteLine($"Finding node with value {x}: {node?.ToString() ?? "NA"}");
    });

    Console.WriteLine();
    Console.WriteLine();
}

void NodeHeight()
{
    var tree = new BinaryTree<int>
    {
        root = new()
        {
            Value = 1,
            Left = new()
            {
                Value = 2,
                Left = new()
                {
                    Value = 5
                },
                Right = new()
                {
                    Value = 4,
                    Left = new()
                    {
                        Value = 7,
                        Left = new()
                        {
                            Value = 10
                        },
                        Right = new()
                        {
                            Value = 8,
                            Left = new()
                            {
                                Value = 9
                            }
                        }
                    }
                }
            },
            Right = new()
            {
                Value = 3
            },
        }
    };

    var height = tree.GetHeight();
    Console.WriteLine($"Height: {height}");
}

void Tree1()
{
    var tree = new BinaryTree<int?>(1, 2, 3, 4, 5, null, 7);

    Console.Write($"Pre Order: ");
    TreePractice1.TraverseTreePreOrder(tree.root, i => Console.Write(i));
    Console.WriteLine();

    Console.Write($"In Order: ");
    TreePractice1.TraverseTreeInOrder(tree.root, i => Console.Write(i));
    Console.WriteLine();

    Console.Write($"Post Order: ");
    TreePractice1.TraverseTreePostOrder(tree.root, i => Console.Write(i));
    Console.WriteLine();

    Console.Write($"Level Order: ");
    TreePractice1.TraverseTreeLevelOrder(tree.root, i => Console.Write(i));
    Console.WriteLine();
}

void Recurse()
{
    Trees.Practice1.ActRecursively([1, 2, 3, 4, 5, 6], Console.WriteLine);
}

void MaxInKBruteForce()
{
    var arr = Enumerable.Range(1, 10).ToArray();
    var k = 3;
    var maxes = StacksAndQueues.Practice3.GetMaxInEachWindowBruteForce(arr, k);

    Console.WriteLine($"Original Array: {string.Join(", ", arr.Select(x => x.ToString()))}");
    Console.WriteLine($"Maxes Array: {string.Join(", ", maxes.Select(x => x.ToString()))}");
}

void MaxMinQueue()
{
    var maxMinQueue1 = new MinMaxQueue<int>();

    for (int i = 1; i <= 5; i++)
    {
        maxMinQueue1.Enqueue(i);
    }

    for (int i = 1; i <= 5; i++)
    {
        maxMinQueue1.Dequeue(out _);
    }
}

void ReverseKInQueue()
{
    var q = new StacksAndQueues.Queue<int>();

    for (int i = 1; i <= 5; i++)
    {
        q.Enqueue(i);
    }

    PrintQueue(ref q, "Starting state");

    for (int i = -1; i <= 6; i++)
    {
        StacksAndQueues.Practice2.ReverseFirstK(i, q);

        PrintQueue(ref q, $"Reverse first {i}");

        Console.WriteLine();
    }
}

void InterleaveElementsQueue()
{
    for (int i = 1; i <= 11; i++)
    {
        var q1 = new StacksAndQueues.Queue<int>();

        for (int j = 1; j < i; j++)
        {
            q1.Enqueue(j);
        }

        PrintQueue(ref q1, "Original");

        StacksAndQueues.Practice2.InterleaveHalfHalf(q1);

        PrintQueue(ref q1, "Interleaved");
    }
}

void PrintQueue(ref StacksAndQueues.Queue<int> q, string title)
{
    Console.Write($"{title}:");

    var q2 = new StacksAndQueues.Queue<int>();
    while (!q.IsEmpty)
    {
        var el = q.Dequeue(out _);
        Console.Write($" {el}");
        q2.Enqueue(el!);
    }

    Console.WriteLine();
    q = q2;
}

void FirstNBins()
{
    var bins = StacksAndQueues.Practice2.GenerateFirstNBinaryNumbers(100);
    foreach (var b in bins)
    {
        Console.WriteLine($"{b,15}");
    }
}

void FirstNonRepeatingChars()
{
    var str = "abcabdefcgdehifghi";
    var stream = new MemoryStream(Encoding.ASCII.GetBytes(str));

    Console.WriteLine($"String: {str}");
    Console.WriteLine();
    Console.WriteLine();

    foreach (var ch in StacksAndQueues.Practice2.FirstNonRepeatingCharacterInStream(stream))
    {
        Console.Write(ch);
    }

    Console.WriteLine();
}

void UnboundQueue()
{
    var queue = new StacksAndQueues.Queue<int?>();

    for (int i = 1; i < 10; i++)
    {
        Console.WriteLine($"""
            Total Elements: {queue.Count}
            Front: {queue.Peek(out _)}


            """);
        queue.Enqueue(i);
    }

    Console.WriteLine();
    Console.WriteLine();

    while (!queue.IsEmpty)
    {
        Console.WriteLine($"Dequeued: {queue.Dequeue(out _)}. Next element: {queue.Peek(out _)}\n");
    }
}

void ArrayQueue()
{
    var queue = new QueueBound<int>(5);
    for (int i = 1; i < 7; i++)
    {
        Console.WriteLine($"""
            Current number of elements: {queue.Count}
            Element at the front: {queue.Peek(out _)}
            Enqueue success: {queue.Enqueue(i)}
            """);
    }

    Console.WriteLine($"Elements inserted.\n");

    while (!queue.IsEmpty)
    {
        Console.WriteLine($"Dequeued: {queue.Dequeue(out _)}");
    }
}

void EvaluatePrefixExpressions()
{
    string[] exprs =
    [
        "+ 5 ~ 6",
        "+ 10 * ~ 4 3",
        "+ 2 * 3 ~ 4",
        "* / 8 2 + 3 ~ 1",
        "^ 5 - ~ 6 * 7 8",
    ];

    foreach (var expr in exprs)
    {
        Console.WriteLine($"Postfix: {expr}");
        var result = Practice1.EvaluatePreFixExpression(expr);
        if (result.HasValue)
        {
            Console.WriteLine($"Answer: {result.Value}");
        }
        else
        {
            Console.WriteLine($"The expression {expr} is malformed.");
        }

        Console.WriteLine();
    }
}

void EvaluatePostfixExpression()
{
    string[] exprs1 =
    [
        "5 6 +",
        "2 3 4 * +",
        "10 2 /",
        "2 3 + 5 *",
        "8 2 3 * + 5 -",
        "3 !",
    ];

    string[] exprs2 =
    [
        "4 5 7 2 + - *",
        "3 4 + 2 5 * +",
        "10 2 8 * + 3 -",
        "2 3 + 4 5 * + 6 -",
        "5 1 2 + 4 * + 3 -",
    ];

    foreach (var expr in exprs2)
    {
        Console.WriteLine($"Postfix: {expr}");
        var result = Practice1.EvaluatePostFixExpression(expr);
        if (result.HasValue)
        {
            Console.WriteLine($"Answer: {result.Value}");
        }
        else
        {
            Console.WriteLine($"The expression {expr} is malformed.");
        }

        Console.WriteLine();
    }
}

void PrefixToInfix()
{
    string[] expressions =
    [
        "~ A",
        "+ A ~ B",
        "+ A * ~ B C",
        "- + ~ A B / C D",
        "+ * A B ^ - ~ C D E",
    ];

    var binaryOperators = "+ - * / ^".Split().ToHashSet();
    var unaryOperators = "! ~".Split().ToHashSet();

    foreach (var expr in expressions)
    {
        Console.WriteLine($"Postfix: {expr}");
        var infix = Practice1.PrefixToInfix(expr, binaryOperators, unaryOperators, out var success);
        if (!success)
        {
            Console.WriteLine($"Prefix expression is malformed.");
        }
        else
        {
            Console.WriteLine($"Infix: {infix}");
        }

        Console.WriteLine();
    }
}

void PostfixToInfix()
{
    string[] expressions =
    [
        "A B +",
        " A B C * +   ",
        "A B + C D - *",
        "A B C D ^ E - F G H * + ^ * + I -",
        "A B C ^ D E / - F G H * +", // invalid. Stack will run out in the mid way,
        "A B C D * +",  // invalid. stack will contain more than one elem at the end,
        "A !",
        "A B + ~",
        "A B + C * ~",
        "A B + C D + * !",
        "A B + C D + * E F + G ~ + /",
        "A B C ~ * + D E + ! * -",
        "A B C ~ * + D E + ! * - ~ !",
    ];

    var binaryOperators = "+ - * / ^".Split().ToHashSet();
    var unaryOperators = "! ~".Split().ToHashSet();

    foreach (var expr in expressions)
    {
        Console.WriteLine($"Postfix: {expr}");
        var infix = Practice1.PostfixToInfix(expr, binaryOperators, unaryOperators, out var success);
        if (!success)
        {
            Console.WriteLine($"Postfix expression is malformed.");
        }
        else
        {
            Console.WriteLine($"Infix: {infix}");
        }

        Console.WriteLine();
    }
}

static void TestReverseString()
{
    var str1 = "abc";
    var str2 = string.Empty;

    var str1R = Practice1.ReverseString(str1);
    var str2R = Practice1.ReverseString(str2);

    Console.WriteLine($"Reverse of {str1}: {str1R}");
    Console.WriteLine($"Reverse of {str2}: {str2R}");
}

static void TestBalancedExpression()
{
    var openingChars = "[{(<$".ToCharArray().ToHashSet();
    var closingToOpeningCharsMap = new Dictionary<char, char>
    {
        [']'] = '[',
        ['}'] = '{',
        [')'] = '(',
        ['>'] = '<'
    };

    var expr1 = "[{(<>)}]";
    var res1 = Practice1.IsExpressionBalanced(expr1, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr1} is balanced: {res1}");

    var expr2 = "{[()]";
    var res2 = Practice1.IsExpressionBalanced(expr2, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr2} is balanced: {res2}");

    var expr3 = "{[()]}]";
    var res3 = Practice1.IsExpressionBalanced(expr3, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr3} is balanced: {res3}");

    var expr4 = "{[()";
    var res4 = Practice1.IsExpressionBalanced(expr4, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr4} is balanced: {res4}");

    var expr5 = "{[(";
    var res5 = Practice1.IsExpressionBalanced(expr5, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr5} is balanced: {res5}");

    var expr6 = "{[()A]}";
    var res6 = Practice1.IsExpressionBalanced(expr6, openingChars, closingToOpeningCharsMap);
    Console.WriteLine($"Expression {expr6} is balanced: {res6}");
}

static void TestCreation()
{
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


}
}
