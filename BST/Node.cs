using System.Text;

namespace BST;

public class Node<T>(T value)
{
    private readonly int hash = Guid.NewGuid().GetHashCode();

    public T Value { get; set; } = value;

    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }

    public int Degree =>
        Left is null && Right is null ? 0 :
        Left is null ? 1 :
        Right is null ? 1 :
        2;

    public override int GetHashCode() => hash;

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public static bool operator ==(Node<T>? node, Node<T>? other)
    {
        if (ReferenceEquals(node, other)) return true;
        if (node is null || other is null) return false;
        return EqualityComparer<T>.Default.Equals(node.Value, other.Value);
    }

    public static bool operator !=(Node<T> node, Node<T>? other) => !(node == other);

    public static bool operator ==(Node<T> node, T? value) => EqualityComparer<T>.Default.Equals(node.Value, value);

    public static bool operator !=(Node<T> node, T? value) => !(node == value);

    public static bool operator <(Node<T> node, Node<T>? other) => Comparer<T>.Default.Compare(node.Value, other is null ? default : other.Value) < 0;

    public static bool operator >(Node<T> node, Node<T>? other) => Comparer<T>.Default.Compare(node.Value, other is null ? default : other.Value) > 0;

    public static bool operator <(Node<T> node, T? value) => Comparer<T>.Default.Compare(node.Value, value) < 0;

    public static bool operator >(Node<T> node, T? value) => Comparer<T>.Default.Compare(node.Value, value) > 0;

    public static bool operator <(T? value, Node<T> node) => node < value;

    public static bool operator >(T? value, Node<T> node) => node > value;

    public override string ToString() => $"Value: {Value} Degree: {Degree} Left: {(Left is null ? default : Left.Value)} Right: {(Right is null ? default : Right.Value)}";
}
