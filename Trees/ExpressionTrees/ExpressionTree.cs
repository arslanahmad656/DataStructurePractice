namespace Trees.ExpressionTrees;

public class ExpressionTree<T>(ExpressionNode<T> root)
{
    public ExpressionNode<T> Root { get; set; } = root;
}
