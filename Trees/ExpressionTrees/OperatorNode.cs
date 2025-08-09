namespace Trees.ExpressionTrees;

public class OperatorNode<T> : ExpressionNode<T>
{
    public required string Operator { get; set; }

    public required ExpressionNode<T> Left { get; set; }

    public required ExpressionNode<T> Right { get; set; }
}
