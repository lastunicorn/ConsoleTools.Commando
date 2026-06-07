namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal static class EnumerableExtension
{
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int, bool> action)
    {
        if (collection == null)
            return;

        int index = -1;
        T previousItem = default;

        foreach (T item in collection)
        {
            index++;

            if (index > 0)
            {
                int previousIndex = index - 1;
                action(previousItem, previousIndex, false);
            }

            previousItem = item;
        }

        if (index >= 0)
            action(previousItem, index, true);
    }
}