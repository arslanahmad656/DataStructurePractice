namespace LinkedLists.Nodes;

public class NodeDoubly<T> : NodeBase<T>
{
    public NodeDoubly<T>? Previous { get; set; }
    public NodeDoubly<T>? Next { get; set; }
}
