namespace LinkedLists;

internal static class Sorting
{
    public static void BubbleSort<T> (this T[] arr)
    {
        if (arr.Length == 0)
        {
            return;
        }

        var comparer = Comparer<T>.Default;
        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < arr.Length - 1 - i; j++)
            {
                if (comparer.Compare(arr[j], arr[j + 1]) > 0)
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }

            if (!swapped)
            {
                break;
            }
        }
    }

    public static void SelectionSort<T> (this T[] arr)
    {
        if (arr.Length == 0)
        {
            return;
        }

        var comparer = Comparer<T>.Default;
        
        for (int i = 0; i < arr.Length - 1;  ++i)
        {
            var minIndex = i;
            
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (comparer.Compare(arr[minIndex], arr[j]) > 0)
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
            }
        }
    }

    public static void InsertionSort<T> (this T[] arr)
    {
        if (arr.Length == 0)
        {
            return;
        }

        var comparer = Comparer<T>.Default;

        for (var i = 1; i < arr.Length; i++)
        {
            var key = arr[i];

            int j;
            for (j = i - 1; j >= 0 && comparer.Compare(key, arr[j]) < 0; j--)
            {
                arr[j + 1] = arr[j];
            }

            arr[j + 1] = key;
        }
    }
}
