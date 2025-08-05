namespace LinkedLists.Helpers;

public static class Helper
{
    /// <summary>
    /// Calculates the normalized K for list rotation so that even for the negative values of K, only right shift may be performed.
    /// </summary>
    /// <param name="k">The value (positive or negative or 0) about which to rotate a linked list.</param>
    /// <param name="n">Total number of nodes in the list.</param>
    /// <returns>Non negative integer</returns>
    public static int GetNormalizedKForRotation(int k, int n) => (k % n + n) % n;
}