using BST;

Search();

void Search()
{
    var bst = BinarySearchTree<int>.BuildRandomTree();

    new List<int> { 12, 10, 5, 3, -12, 35 }.ForEach(v =>
    {
        var found = bst.Search(v);

        Console.WriteLine($"Found {v}: {found}");
    });
}