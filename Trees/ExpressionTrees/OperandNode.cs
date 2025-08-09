namespace Trees.ExpressionTrees;

public class OperandNode<T> : ExpressionNode<T>
{
    public required T Operand { get; set; }
}
