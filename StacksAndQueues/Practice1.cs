using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace StacksAndQueues;

public static class Practice1
{
    public static bool IsExpressionBalanced(string expression, IReadOnlySet<char> openingCharacters, IReadOnlyDictionary<char, char> closingToOpeningCharactersMap)
    {
        var stack = new Stack<char>();

        foreach (var ch in expression)
        {
            // if it's an opening character, push onto the stack
            if (openingCharacters.Contains(ch))
            {
                stack.Push(ch);
                continue;
            }

            // otherwise assume that the character is a closing character. In that case, get the corresponding opening character
            if (!closingToOpeningCharactersMap.TryGetValue(ch, out var correspondingOpeningChar))
            {
                return false;   // since it's neither the opening nor the closing character
            }

            // we have got the opening character corresponding to the closing ch. The top of the stack at this point must contain the opening character otherwise it's not a balanced expression
            // but if the stack is already empty, it means we have exhausted the stack but there are still characters in the expression. this indicates that the expression is not balanced

            var poppedChar = stack.Pop(out var wasEmpty);

            if (wasEmpty)
            {
                return false;   // means more number of closing characters than the opening one.
            }

            if (poppedChar != correspondingOpeningChar)
            {
                // the top character in the stack must be the corresponding opening character otherwise the expression is not balanced.
                return false;
            }
        }

        // at this point, the expression is balanced if the stack is empty.
        return stack.IsEmpty;
    }

    public static string ReverseString(string original)
    {
        var stack = new Stack<char>();

        original.ToList().ForEach(stack.Push);

        var sb = new StringBuilder();

        while (!stack.IsEmpty)
        {
            sb.Append(stack.Pop(out _));
        }

        return sb.ToString();
    }

    public static string? PrefixToInfix(string infix, ISet<string> binaryOperators, ISet<string> unaryOperators, [NotNullWhen(true)] out bool success)
        => PrePostfixToInfix(infix, binaryOperators, unaryOperators, false, out success);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="postFix">tokens separated by space</param>
    /// <param name="binaryOperators"></param>
    /// <param name="success"></param>
    /// <returns></returns>
    public static string? PostfixToInfix(string postFix, ISet<string> binaryOperators, ISet<string> unaryOperators, [NotNullWhen(true)]out bool success)
        => PrePostfixToInfix(postFix, binaryOperators, unaryOperators, true, out success);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="postFix">tokens separated by space</param>
    /// <param name="binaryOperators"></param>
    /// <param name="success"></param>
    /// <returns></returns>
    private static string? PrePostfixToInfix(string postFix, ISet<string> binaryOperators, ISet<string> unaryOperators, bool isPostFix, [NotNullWhen(true)] out bool success)
    {
        success = false;
        if (string.IsNullOrWhiteSpace(postFix))
        {
            return default;
        }

        var stack = new Stack<string>();
        //var tokens = (isPostFix ? postFix : string.Join(string.Empty, postFix.Reverse())).Split(' ', StringSplitOptions.RemoveEmptyEntries);  // bug
        var tokens = postFix.Trim().Split(' ');
        if (!isPostFix)
        {
            tokens = tokens.Reverse().ToArray();
        }

        foreach (var token in tokens)
        {
            // if it is an operator, the last two items (and last one in case of unary) in the stack are the operands of this operator. the top one is right and the next one is the left operand
            string? expr = null;
            if (unaryOperators.Contains(token))
            {
                var operand = stack.Pop(out var wasEmpty);
                if (wasEmpty)
                {
                    return default;
                }

                expr = $"{token}{operand}";
            }
            else if (binaryOperators.Contains(token))
            {
                var right = stack.Pop(out var wasEmpty);
                var left = stack.Pop(out wasEmpty);

                if (!isPostFix)
                {
                    // means it's prefix
                    (right, left) = (left, right);
                }

                if (wasEmpty)
                {
                    // there are not enough operands available which means that the input postFix expression is not correct.
                    return default;
                }

                expr = $"({left} {token} {right})";    // create the expression.
                // push the expr back to the stack
            }

            if (expr != null)
            {
                stack.Push(expr);
            }
            else
            {
                // otherwise it's an operand. Just push it. It'll be popped when the operator comes
                stack.Push(token);
            }
        }

        // At the end, the stack must only contain exactly one entry which is the infix expression. If it's not, the given postFix is malformed.
        if (stack.Count != 1)
        {
            return default;
        }

        // return the infix expression
        success = true;
        return stack.Pop(out _);
    }

    public static double? EvaluatePostFixExpression(string expression)
        => EvaluateExpression(expression, false);

    public static double? EvaluatePreFixExpression(string expression)
        => EvaluateExpression(expression, true);

    private static double? EvaluateExpression(string expression, bool isPrefix)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return default;
        }

        var supportedBinaryOperators = "+ - * / ^".Split(' ').ToHashSet();
        var supportedUnaryOperators = "! ~".Split(' ').ToHashSet();

        var stack = new Stack<double>();
        var tokens = expression.Trim().Split(' ');
        if (isPrefix)
        {
            tokens = [.. tokens.Reverse()];
        }

        foreach (var token in tokens)
        {
            double? expressionResult = null;
            if (supportedUnaryOperators.Contains(token))
            {
                var operand = stack.Pop(out var wasEmpty);
                if (wasEmpty)
                {
                    return default;
                }

                expressionResult = GetOperatorResult(token, operand);
            }
            else if (supportedBinaryOperators.Contains(token))
            {
                var right = stack.Pop(out var wasEmpty);
                var left = stack.Pop(out wasEmpty);

                if (wasEmpty)
                {
                    return default;
                }

                if (isPrefix)
                {
                    (right, left) = (left, right);
                }

                expressionResult = GetOperatorResult(token, left, right);
            }

            if (expressionResult.HasValue)
            {
                stack.Push(expressionResult.Value);
            }
            else
            {
                stack.Push(double.Parse(token));
            }
        }

        if (stack.Count != 1)
        {
            return default;
        }

        return stack.Pop(out _);
    }

    private static double GetOperatorResult(string @operator, params double[] operands) => @operator switch
    {
        "+" => operands[0] + operands[1],
        "-" => operands[0] - operands[1],
        "*" => operands[0] * operands[1],
        "/" => operands[0] / operands[1],
        "^" => Math.Pow(operands[0], operands[1]),
        "!" or "~" => operands[0] * -1, // change sign,
        _ => throw new NotSupportedException($"Operator ${@operator} not supported."),
    };
}
