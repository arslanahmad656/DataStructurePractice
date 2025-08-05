using StacksAndQueues;
using IntStack = StacksAndQueues.Stack<int>;


//TestCreation();
//TestBalancedExpression();
//TestReverseString();
//PostfixToInfix();
//PrefixToInfix();
//EvaluatePostfixExpression();
EvaluatePrefixExpressions();

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

    foreach(var expr in exprs2)
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

Console.WriteLine();
Console.WriteLine();

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